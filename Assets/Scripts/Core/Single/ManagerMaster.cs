/*
 added by Shinedo @ 2020/6/18
 通过工厂模式所有常用实例的管理类
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OF.Core
{
    public interface ISingle
    {
        int Priority { get; set; }

        void OnCreate(ManagerMaster master);

        void OnUpdate(ManagerMaster master);

        void OnDispose(ManagerMaster master);
    }

    public class BaseMonoBehaviour : MonoBehaviour
    {
        private void Update()
        {
            ManagerMaster.Instance.OnUpdate();
        }

        private void OnDestroy()
        {
            ManagerMaster.Instance.OnDispose();
        }
    }

    public sealed class ManagerMaster
    {
        private static ManagerMaster s_Instance;
        private static readonly object s_Lock = new object();

        private static Dictionary<Type, ISingle> S_SinglesDic = new Dictionary<Type, ISingle>();

        private static BaseMonoBehaviour s_BaseMono;

        public static ManagerMaster Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (s_Lock)
                    {
                        GameObject go = new GameObject("Single");
                        s_BaseMono = go.AddComponent<BaseMonoBehaviour>();
                        Object.DontDestroyOnLoad(go);
                        s_Instance = new ManagerMaster();
                    }
                }

                return s_Instance;
            }
        }

        public T GetSingle<T>() where T : ISingle, new()
        {
            var type = typeof(T);
            S_SinglesDic.TryGetValue(type, out var single);
            if (single != null)
            {
                return (T) single;
            }

            lock (s_Lock)
            {
                // 二次验证
                S_SinglesDic.TryGetValue(type, out single);
                if (single != null) return (T) single;
                var tSingle = new T();
                tSingle.OnCreate(Instance);
                S_SinglesDic[type] = tSingle;
                S_SinglesDic = S_SinglesDic.OrderBy(_single => _single.Value.Priority)
                    .ToDictionary(_single => _single.Key, _single => _single.Value);

                return (T) tSingle;
            }
        }

        public void ClearSingle<T>() where T : ISingle, new()
        {
            var type = typeof(T);
            if (S_SinglesDic.ContainsKey(type))
            {
                var single = S_SinglesDic[type];
                single.OnDispose(Instance);
                S_SinglesDic.Remove(type);
            }
        }

        public void OnUpdate()
        {
            foreach (var single in S_SinglesDic.Values)
            {
                single.OnUpdate(Instance);
            }
        }

        public void OnDispose()
        {
            Object.Destroy(s_BaseMono.gameObject);
            s_Instance = null;
        }
    }
}