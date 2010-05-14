using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using Proxy;
using System.Runtime.Remoting.Messaging;
using System.Net.Sockets;
using System.Runtime.Remoting.Lifetime;
using System.Configuration;

namespace TriviaExpert
{
    public delegate void ErrorHandler(String message);
    public delegate void ThemeHandler(String theme);
    public delegate void ResponseHandler(Int32 questionNumber, String answer);

    public class Client : IClient
    {
        public event ErrorHandler OnError;
        public event QuestionHandler OnQuestionAnswered;
        public event ThemeHandler OnExpertsGetComplete;
        public event ResponseHandler OnAnswerReceived;

        private readonly object monitor = new object();
        private static readonly string configFile = ConfigurationManager.AppSettings["clientConfigFile"];

        private List<IExpert> _myExperts; //List of experts created locally
        private Dictionary<String, List<IExpert>> _ringExperts; //List of remote experts
        private ITriviaSponsor _expertsSponsor;

        private int _nrQuestions; //Question counter
        private bool _isConnected;

        private IZoneServer _server;
        private string _serverUrl;
        private ITriviaSponsor _serverSponsor;

        static Client()
        {
            RemotingConfiguration.Configure(configFile, false);
        }

        public void Connect()
        {
            WellKnownClientTypeEntry[] entries = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
            _serverUrl = entries[0].ObjectUrl;
            _server = (IZoneServer)Activator.GetObject(entries[0].ObjectType, entries[0].ObjectUrl);
            _nrQuestions = 0;
            _isConnected = true;
            AttachServerSponsor();
        }

        public void Disconnect()
        {
            ReleaseServerSponsor();
            _server = null;
            _myExperts = null;
            _ringExperts = null;
            _isConnected = false;
        }

        public int GetQuestionCount()
        { lock (monitor) { return _nrQuestions; } }

        public string GetServerUrl()
        { return _serverUrl; }

        public bool IsConnected()
        { return _isConnected; }

        #region Server-Sponsor relation methods
        private void AttachServerSponsor()
        {
            _serverSponsor = _server.GetSponsor();
            ILease lease = (ILease)RemotingServices.GetLifetimeService((MarshalByRefObject)_server);
            lease.Register(_serverSponsor);
        }

        private void ReleaseServerSponsor()
        {
            if (_serverSponsor != null)
            {
                try
                {
                    _serverSponsor.SetNotRenew();
                    ILease lease = (ILease)RemotingServices.GetLifetimeService((MarshalByRefObject)_server);
                    lease.Unregister(_serverSponsor);
                    _serverSponsor = null;
                }
                catch (SocketException)
                {
                    OnError("Problems occured while unregistering server sponsor.");
                }
            }
        }
        #endregion

        #region Local experts management
        public void AddLocalExpert(String theme)
        {
            lock (monitor)
            {
                if (_myExperts == null)
                    _myExperts = new List<IExpert>();
                IExpert newExpert = new Expert(theme);
                newExpert.OnQuestionAnswered += OnExpertQuestionAnswered;
                _myExperts.Add(newExpert);
            }
        }

        private void OnExpertQuestionAnswered(
            IExpert sender, string keyWords, string answer)
        {
            if (OnQuestionAnswered != null)
            {
                OnQuestionAnswered(sender, keyWords, answer);
            }
        }
        #endregion

        #region Expert interactions and obtainance
        public void Ask(String theme, List<String> keywords)
        {
            List<IExpert> experts = null;
            lock (monitor)
            {
                experts = _ringExperts[theme];
                _nrQuestions++;

                foreach (IExpert e in experts)
                {
                    Func<List<String>, String> askQuestion = new Func<List<String>, String>(e.Ask);
                    askQuestion.BeginInvoke(keywords, OnAskEnd, new QuestionState() { Index = _nrQuestions.ToString(), Theme = theme, Expert = e });
                }
            }
        }

        private void OnAskEnd(IAsyncResult resp)
        {
            AsyncResult res = (AsyncResult)resp;
            Func<List<String>, String> askQuestion = (Func<List<String>, String>)res.AsyncDelegate;
            QuestionState state = null;
            try
            {
                state = (QuestionState)resp.AsyncState;
                String answer = askQuestion.EndInvoke(resp);
                if (OnAnswerReceived != null)
                {
                    OnAnswerReceived(int.Parse(state.Index), answer);
                }
            }
            catch (SocketException ex)
            {
                if (OnError != null)
                {
                    OnError("Can't contact expert");
                }
                new Action<String, IExpert>(_server.NotifyClientFault).
                    BeginInvoke(state.Theme, state.Expert, OnNotifyComplete, null);
            }
        }

        private void OnNotifyComplete(IAsyncResult resp)
        {
            AsyncResult res = (AsyncResult)resp;
            Action<String, IExpert> notifyCall = (Action<String, IExpert>)res.AsyncDelegate;
            try
            {
                notifyCall.EndInvoke(resp);
            }
            catch (SocketException ex)
            {
                if (OnError != null)
                {
                    OnError("Can't notify zone server");
                }
            }
        }

        public void GetExperts(String theme)
        {
            Func<String, List<IExpert>> getThemeExperts = new Func<string, List<IExpert>>(_server.GetExpertList);
            getThemeExperts.BeginInvoke(theme, OnExpertsGet, theme);
        }

        private void OnExpertsGet(IAsyncResult resp)
        {
            String theme = null;
            AsyncResult res = (AsyncResult)resp;
            Func<String, List<IExpert>> del = (Func<String, List<IExpert>>)res.AsyncDelegate;
            try
            {
                List<IExpert> experts = del.EndInvoke(resp);
                if (experts != null && experts.Count > 0)
                {
                    foreach (IExpert expert in experts)
                        AttachExpertSponsor(expert);

                    lock (monitor)
                    {
                        if (_ringExperts == null)
                        {
                            _ringExperts = new Dictionary<string, List<IExpert>>();
                        }
                        _ringExperts.Add((String)resp.AsyncState, experts);
                    }
                    theme = (String)resp.AsyncState;
                }
            }
            catch (SocketException ex)
            {
                if (OnError != null)
                {
                    OnError("Can't get experts");
                }
            }
            finally
            {
                if (OnExpertsGetComplete != null)
                {
                    OnExpertsGetComplete(theme);
                }
            }
        }

        private void AttachExpertSponsor(IExpert expert)
        {
            Func<ITriviaSponsor> getExpertSponsor =
                new Func<ITriviaSponsor>(expert.GetSponsor);
            getExpertSponsor.BeginInvoke(OnAttachExpertSponsor, expert);
        }

        private void OnAttachExpertSponsor(IAsyncResult resp)
        {
            AsyncResult res = (AsyncResult)resp;
            Func<ITriviaSponsor> del = (Func<ITriviaSponsor>)res.AsyncDelegate;
            IExpert expert = (IExpert)resp.AsyncState;

            if (_expertsSponsor == null)
                _expertsSponsor = del.EndInvoke(resp);
            ILease lease = (ILease)RemotingServices.GetLifetimeService(
                                (MarshalByRefObject)expert);
            lease.Register(_expertsSponsor);
            OnAnswerReceived(69, "Attached sponsor to expert");
        }

        private void ReleaseExpertSponsor(IExpert expert)
        {
            ILease lease = (ILease)RemotingServices.GetLifetimeService(
                                (MarshalByRefObject)expert);
            if (_expertsSponsor != null)
                lease.Unregister(_expertsSponsor);
        }

        #endregion

        #region Expert registration and unregistration on server
        public void RegisterAll()
        {
            lock (monitor)
            {
                // Begin registering the experts
                Action<String, IExpert> registerAction = new Action<string, IExpert>(_server.Register);
                foreach (IExpert expert in _myExperts)
                {
                    registerAction.BeginInvoke(
                        expert.GetTheme(),
                        expert,
                        OnRegisterComplete,
                        expert
                    );
                }
            }
        }

        private void OnRegisterComplete(IAsyncResult resp)
        {
            AsyncResult res = (AsyncResult)resp;
            Action<String, IExpert> registerAction = (Action<String, IExpert>)res.AsyncDelegate;
            IExpert expert = (IExpert)resp.AsyncState;
            try
            {
                registerAction.EndInvoke(resp);
            }
            catch (SocketException)
            {
                if (OnError != null)
                {
                    OnError(String.Format("Register failed for theme {0}.", expert.GetTheme()));
                }
            }
        }

        public void UnregisterAll()
        {
            if (_myExperts != null)
            {
                lock (monitor)
                {
                    foreach (IExpert expert in _myExperts)
                    {
                        Action<String, IExpert> unregister = new Action<string, IExpert>(_server.UnRegister);
                        unregister.BeginInvoke(
                            expert.GetTheme(),
                            expert,
                            OnUnregisterComplete,
                            expert
                        );
                    }
                    _myExperts.Clear();
                }
            }

            ReleaseServerSponsor();
        }

        private void OnUnregisterComplete(IAsyncResult resp)
        {
            AsyncResult res = (AsyncResult)resp;
            Action<String, IExpert> unregisterAction = (Action<String, IExpert>)res.AsyncDelegate;
            IExpert expert = (IExpert)resp.AsyncState;
            try
            {
                unregisterAction.EndInvoke(resp);
                ReleaseExpertSponsor(expert);
            }
            catch (SocketException ex)
            {
                if (OnError != null)
                {
                    OnError(String.Format("Unregister failed for theme {0}.", (String)resp.AsyncState));
                }
            }
        }
        #endregion
    }
}
