using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GeneralLogic
{
    public interface ICreator
    {
        void Create();
    }
    public interface IBuilder<in T>
    {
        IEnumerator Build(T count);
    }
    public interface IBuilder
    {
        IEnumerator Build();
    }
    public interface ICleaner
    {
        void Clear();
    }
    public interface ITool
    {
        Image GetLine();
    }
}