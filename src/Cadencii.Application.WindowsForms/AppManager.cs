/*
 * AppManager.cs
 * Copyright © 2009-2011 kbinani
 *
 * This file is part of cadencii.
 *
 * cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
//#define ENABLE_OBSOLUTE_COMMAND
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Threading;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.CSharp;
using cadencii.apputil;
using cadencii.java.awt;
using cadencii.java.io;
using cadencii.java.util;
using cadencii.media;
using cadencii.vsq;
using cadencii.xml;
using cadencii.utau;
using ApplicationGlobal = cadencii.core.ApplicationGlobal;
using Keys = cadencii.java.awt.Keys;
using DialogResult = cadencii.java.awt.DialogResult;

namespace cadencii
{


    public partial class AppManager
    {
        #region Static Readonly Fields
        #endregion

        #region Private Static Fields

        #endregion
        /// <summary>
        /// 歌詞入力に使用するテキストボックス
        /// </summary>
        public static LyricTextBox mInputTextBox = null;

		#if ENABLE_PROPERTY
		/// <summary>
		/// プロパティウィンドウが分離した場合のプロパティウィンドウのインスタンス。
		/// メインウィンドウとプロパティウィンドウが分離している時、propertyPanelがpropertyWindowの子になる
		/// </summary>
		public static FormNotePropertyController propertyWindow;
		#endif
		/// <summary>
		/// アイコンパレット・ウィンドウのインスタンス
		/// </summary>
		public static FormIconPalette iconPalette = null;

        static string _(string id)
        {
            return Messaging.getMessage(id);
        }

		static VsqFileEx mVsq { get { return MusicManager.getVsqFile (); } }
    }

}
