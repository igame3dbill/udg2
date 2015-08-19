﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

    // Make sure to rename the file name to TransformLooper.cs
    [ExecuteInEditMode]
    public class TransformDuplicator : MonoBehaviour
    {
        public bool doDuplicate = false;
        public bool prefabHasMultiplied = false;
        public Vector3 Direction = new Vector3(1, 0, 0);
        public int TileCount = 1;
        public float Distance = 1;
        public float Speed = 1;
        public Transform _prefab;
        private Transform _lerper;

#if UNITY_EDITOR
        private void MultiplyMyModel()
        {
            prefabHasMultiplied = true;
            doDuplicate = false;
            _lerper = new GameObject().transform;
            _lerper.name = "Lerper";
            _lerper.transform.SetParent(transform);
            for (int i = 0; i < TileCount; i++)
            {
                var instance = Instantiate(_prefab);
                instance.transform.SetParent(_lerper.transform);
                instance.transform.position = i * Direction * Distance;
            }

            //StartLoop();
        }

        public void StartLoop()
        {
            StartCoroutine(Loop());
        }

        private IEnumerator Loop()
        {
            var wait = new WaitForFixedUpdate();
            var startPos = _lerper.transform.localPosition;
            print(_lerper.name);
            while (true)
            {
                _lerper.transform.localPosition = GetLoopLerp(startPos, Direction, Distance, Speed);
                yield return wait;
            }
        }

        public static Vector3 GetLoopLerp(Vector3 start, Vector3 direction, float length, float speed)
        {
            var timespan = length / speed;
            var t = Time.time % timespan / timespan;
            return Vector3.Lerp(start, start + direction * length, t);
        }

        public void Update()
        {
            if (doDuplicate == true)
            {

                MultiplyMyModel();
            }
        }
#endif
    }
