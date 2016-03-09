using BoardGame.Contracts;
using BoardGame.Contracts.Responses;
using BoardGame.Domain.Enums;
using BoardGame.Domain.Interfaces;
using BoardGame.Domain.Logger;
using BoardGame.Server.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoardGame.Server.Services
{
    public class GameService : GameServiceAsync, IGameService
    {
        public GameService()
        {
        }

        public GameService(IGameServer logic, ILogger logger)
            :base(logic, logger)
        {
            logger.Info("GameService created.");
        }

        public IAsyncResult BeginVerifyConnection(int testNumber, AsyncCallback callback, object state)
        {
            var tcs = new TaskCompletionSource<int>(state);
            var task = VerifyConnection(testNumber);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    tcs.TrySetException(t.Exception.InnerExceptions);
                else if (t.IsCanceled)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(t.Result);

                if (callback != null)
                    callback(tcs.Task);
            });
            return tcs.Task;
        }
        public int EndVerifyConnection(IAsyncResult asyncResult)
        {
            try
            {
                return ((Task<int>)asyncResult).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public IAsyncResult BeginOnlineGameRequest(int playerId, AsyncCallback callback, object state)
        {
            var tcs = new TaskCompletionSource<OnlineGameResponse>(state);
            var task = OnlineGameRequest(playerId);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    tcs.TrySetException(t.Exception.InnerExceptions);
                else if (t.IsCanceled)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(t.Result);

                if (callback != null)
                    callback(tcs.Task);
            });
            return tcs.Task;
        }
        public OnlineGameResponse EndOnlineGameRequest(IAsyncResult asyncResult)
        {
            try
            {
                return ((Task<OnlineGameResponse>)asyncResult).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public IAsyncResult BeginConfirmToPlay(int playerId, AsyncCallback callback, object state)
        {
            var tcs = new TaskCompletionSource<StartGameResponse>(state);
            var task = ConfirmToPlay(playerId);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    tcs.TrySetException(t.Exception.InnerExceptions);
                else if (t.IsCanceled)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(t.Result);

                if (callback != null)
                    callback(tcs.Task);
            });
            return tcs.Task;
        }
        public StartGameResponse EndConfirmToPlay(IAsyncResult asyncResult)
        {
            try
            {
                return ((Task<StartGameResponse>)asyncResult).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public IAsyncResult BeginGetFirstMove(int playerId, AsyncCallback callback, object state)
        {
            var tcs = new TaskCompletionSource<MoveResponse>(state);
            var task = GetFirstMove(playerId);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    tcs.TrySetException(t.Exception.InnerExceptions);
                else if (t.IsCanceled)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(t.Result);

                if (callback != null)
                    callback(tcs.Task);
            });
            return tcs.Task;
        }
        public MoveResponse EndGetFirstMove(IAsyncResult asyncResult)
        {
            try
            {
                return ((Task<MoveResponse>)asyncResult).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        public IAsyncResult BeginMakeMove(int playerId, int column, AsyncCallback callback, object state)
        {
            var tcs = new TaskCompletionSource<MoveResponse>(state);
            var task = MakeMove(playerId, column);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    tcs.TrySetException(t.Exception.InnerExceptions);
                else if (t.IsCanceled)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(t.Result);

                if (callback != null)
                    callback(tcs.Task);
            });
            return tcs.Task;
        }
        public MoveResponse EndMakeMove(IAsyncResult asyncResult)
        {
            try
            {
                return ((Task<MoveResponse>)asyncResult).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
