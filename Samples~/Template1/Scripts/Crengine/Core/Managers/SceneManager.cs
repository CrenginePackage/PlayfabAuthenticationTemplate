using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

namespace Crengine.Core
{
    public class SceneEventLoading : EventCore
    {
        public float progress;
        public string sceneName;
        public SceneEventLoading(float _progress, string _sceneName)
        {
            progress = _progress;
            sceneName = _sceneName;
        }
    }
    public class SceneEventDone: EventCore
    {
        public bool isDone;
        public SceneEventDone(bool _isDone)
        {
            isDone = _isDone;
        }
    }
    public class SceneEventStart: EventCore
    {
        public bool isStart;
        public SceneEventStart(bool _isStart)
        {
            isStart = _isStart;
        }
    }

    public class SceneManager : Manager
    {

        public void LoadScene(string _name,LoadSceneMode _mode)
        {
            StartCoroutine(LoadSceneRoutine(_name, _mode));

        }
        IEnumerator LoadSceneRoutine(string _name, LoadSceneMode _mode)
        {
            AsyncOperation async;
            async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_name, _mode);
            RaiseEvent<SceneEventStart>(new SceneEventStart(true));
            while (!async.isDone)
            {
                RaiseEvent<SceneEventLoading>(new SceneEventLoading(async.progress,_name));
                yield return async.progress;
            }
            RaiseEvent<SceneEventDone>(new SceneEventDone(async.isDone));
            yield return null;
        }
    }
}
