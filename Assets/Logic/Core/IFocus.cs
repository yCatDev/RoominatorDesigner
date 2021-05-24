using System;
using UnityEngine;

namespace Logic.Core
{
    public interface IFocusable
    {
        public bool Focused { get; set; }
        public string Focusable { get;  set; }
        public void OnFocus();
        public void OnUnFocus();
    }

}