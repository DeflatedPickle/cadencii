/*
 * FormNotePropertyController.cs
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
using System;
using cadencii.apputil;
using cadencii;
using Keys = Cadencii.Gui.Toolkit.Keys;

namespace Cadencii.Application.Forms
{

    public class FormNotePropertyController : FormNotePropertyListener
    {
        private bool mPreviousAlwaysOnTop;
        private UiFormNoteProperty ui;
		private FormMain main;

		public FormNotePropertyController(Func<FormNotePropertyController,UiFormNoteProperty> createUi, FormMain propertyWindowListener)
        {
            this.main = propertyWindowListener;
            this.ui = createUi (this);
            applyLanguage();
        }


        #region public methods

        /// <summary>
        /// AlwaysOnTopが強制的にfalseにされる直前の，AlwaysOnTop値を取得します．
        /// </summary>
        public bool getPreviousAlwaysOnTop()
        {
            return mPreviousAlwaysOnTop;
        }

        /// <summary>
        /// AlwaysOnTopが強制的にfalseにされる直前の，AlwaysOnTop値を設定しておきます．
        /// </summary>
        public void setPreviousAlwaysOnTop(bool value)
        {
            mPreviousAlwaysOnTop = value;
        }

        public void applyLanguage()
        {
            this.ui.setTitle(_("Note Property"));
        }

        public void applyShortcut(Keys value)
        {
            this.ui.setMenuCloseAccelerator(value);
        }

        public UiFormNoteProperty getUi()
        {
            return this.ui;
        }

        #endregion


        #region helper methods

        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        #endregion


        #region FormNotePropertyUiListenerの実装

        public void onLoad()
        {
            this.ui.setAlwaysOnTop(true);
        }

        public void menuCloseClick()
        {
            this.ui.close();
        }

        public void windowStateChanged()
        {
			this.main.Model.OtherItems.propertyWindowStateChanged();
        }

        public void locationOrSizeChanged()
        {
			this.main.Model.OtherItems.propertyWindowLocationOrSizeChanged();
        }

        public void formClosing()
        {
			this.main.Model.OtherItems.propertyWindowFormClosing();
        }

        #endregion

    }

}
