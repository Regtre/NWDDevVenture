using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace NWDUnityEditor.Coroutine
{
    public class NWDEditorCoroutine
    {
        
        private struct NWDYieldConstruction
        {
            
            enum NWDEditorCoroutineDataType : byte
            {
                None = 0,
                EditorCoroutine = 1,
            }
            
            struct NWDEditorCoroutineData
            {
                public NWDEditorCoroutineDataType DataType;
                public object DataObject;
            }
            
            NWDEditorCoroutineData ProcessorData;
            
            public void Set(object sData)
            {
                if (sData != ProcessorData.DataObject)
                {
                    Type tType = sData.GetType();
                    NWDEditorCoroutineDataType tDataType = NWDEditorCoroutineDataType.None;
                    if (tType == typeof(NWDEditorCoroutine))
                    {
                        tDataType = NWDEditorCoroutineDataType.EditorCoroutine;
                    }
                    ProcessorData = new NWDEditorCoroutineData { DataObject = sData, DataType = tDataType };
                }
            }
            
            public bool MoveNext(IEnumerator sEnumerator)
            {
                bool tAdvancement = false;
                switch (ProcessorData.DataType)
                {
                    case NWDEditorCoroutineDataType.EditorCoroutine:
                        {
                            tAdvancement = (ProcessorData.DataObject as NWDEditorCoroutine).OperationIsDone;
                        }
                        break;
                    default:
                        {
                            tAdvancement = ProcessorData.DataObject == sEnumerator.Current;
                        }
                        break;
                }
                if (tAdvancement)
                {
                    ProcessorData = default(NWDEditorCoroutineData);
                    return sEnumerator.MoveNext();
                }
                return true;
            }
            
        }
        
        internal WeakReference Owner;
        IEnumerator Routine;
        NWDYieldConstruction YieldConstruction;
        bool OperationIsDone;
        
        static Stack<IEnumerator> kStack = new Stack<IEnumerator>(32);
        
        internal NWDEditorCoroutine(IEnumerator sRoutine, object sOwner)
        {
            YieldConstruction = new NWDYieldConstruction();
            if (sOwner != null)
            {
                Owner = new WeakReference(sOwner);
            }
            else
            {
                Owner = null;
            }
            Routine = sRoutine;
            EditorApplication.update += Next;
        }
        
        internal void Next()
        {
            if (Owner != null && !Owner.IsAlive)
            {
                EditorApplication.update -= Next;
                return;
            }
            else
            {
                OperationIsDone = !ProcessMove(Routine);
                if (OperationIsDone)
                {
                    EditorApplication.update -= Next;
                }
            }
        }
        
        private bool ProcessMove(IEnumerator sEnumerator)
        {
            IEnumerator root = sEnumerator;
            bool result = false;
            if (sEnumerator != null)
            {
                while (sEnumerator.Current as IEnumerator != null)
                {
                    kStack.Push(sEnumerator);
                    sEnumerator = sEnumerator.Current as IEnumerator;
                }
                YieldConstruction.Set(sEnumerator.Current);
                result = YieldConstruction.MoveNext(sEnumerator);
                while (kStack.Count > 1)
                {
                    if (!result)
                    {
                        result = kStack.Pop().MoveNext();
                    }
                    else
                    {
                        kStack.Clear();
                    }
                }
                if (kStack.Count > 0 && !result && root == kStack.Pop())
                {
                    result = root.MoveNext();
                }
            }
            else
            {
                result = false;
            }
            return result;
        }
        
        internal void Stop()
        {
            EditorApplication.update -= Next;
            Owner = null;
            Routine = null;
        }
        
    }
    
}
