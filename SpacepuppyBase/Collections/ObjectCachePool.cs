﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace com.spacepuppy.Collections
{

    /// <summary>
    /// Creates a pool that will cache instances of objects for later use so that you don't have to construct them again. 
    /// There is a max cache size, if set to 0 or less, it's considered endless in size.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectCachePool<T> where T : class
    {

        #region Fields

        private Stack<T> _inactive = new Stack<T>();

        private Func<T> _constructorDelegate;
        private Action<T> _resetObjectDelegate;

        #endregion

        #region CONSTRUCTOR

        public ObjectCachePool(int cacheSize)
        {
            this.CacheSize = cacheSize;
            _constructorDelegate = this.SimpleConstructor;
        }

        public ObjectCachePool(int cacheSize, Func<T> constructorDelegate)
        {
            this.CacheSize = cacheSize;
            _constructorDelegate = (constructorDelegate != null) ? constructorDelegate : this.SimpleConstructor;
        }

        public ObjectCachePool(int cacheSize, Func<T> constructorDelegate, Action<T> resetObjectDelegate)
        {
            this.CacheSize = cacheSize;
            _constructorDelegate = (constructorDelegate != null) ? constructorDelegate : this.SimpleConstructor;
            _resetObjectDelegate = resetObjectDelegate;
        }

        private T SimpleConstructor()
        {
            return Activator.CreateInstance<T>();
        }

        #endregion

        #region Properties

        public int CacheSize { get; set; }

        #endregion

        #region Methods

        public T GetInstance()
        {
            if(_inactive.Count > 0)
            {
                return _inactive.Pop();
            }
            else
            {
                return _constructorDelegate();
            }
        }

        public void Release(T obj)
        {
            if (obj == null) throw new System.ArgumentNullException("obj");

            if(this.CacheSize > 0 && _inactive.Count < this.CacheSize)
            {
                if (_resetObjectDelegate != null) _resetObjectDelegate(obj);
                _inactive.Push(obj);
            }
        }

        #endregion

    }

}
