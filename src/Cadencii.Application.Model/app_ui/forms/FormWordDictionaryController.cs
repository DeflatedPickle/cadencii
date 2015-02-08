/*
 * FormWordDictionaryController.cs
 * Copyright © 2011 kbinani
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
using System.Collections.Generic;
using Cadencii.Gui.Toolkit;
using Cadencii.Utilities;

namespace Cadencii.Application.Forms
{
    using System;
    using cadencii.apputil;
    using Cadencii.Media.Vsq;
    using cadencii;

    public class FormWordDictionaryController : ControllerBase, FormWordDictionaryUiListener
    {
        private FormWordDictionaryUi ui;
        private static int mColumnWidth = 256;
        private static int mWidth = 327;
        private static int mHeight = 404;

	public FormWordDictionaryController(Func<FormWordDictionaryController,FormWordDictionaryUi> createUi)
        {
            ui = createUi (this);
            applyLanguage();
            ui.setSize(mWidth, mHeight);
            ui.listDictionariesSetColumnWidth(mColumnWidth);
        }


        #region FormWordDictionaryUiListenerの実装

        public void buttonCancelClick()
        {
            ui.setDialogResult(false);
        }

        public void buttonDownClick()
        {
			var selected = ui.listDictionaries.SelectedIndices;
			int index = selected.Count > 0 ? (int) selected [0] : -1;
			if (0 <= index && index + 1 < ui.listDictionaries.Items.Count) {
                try {
					ui.listDictionaries.Items.Clear();
					string upper_name = ui.listDictionaries.Items[index].Text;
					bool upper_enabled = ui.listDictionaries.Items[index].Checked;
					string lower_name = ui.listDictionaries.Items[index + 1].Text;
					bool lower_enabled = ui.listDictionaries.Items[index + 1].Checked;

					ui.listDictionaries.Items[index + 1].Text = upper_name;
					ui.listDictionaries.Items[index + 1].Checked = upper_enabled;
					ui.listDictionaries.Items[index].Text = lower_name;
					ui.listDictionaries.Items[index].Checked = lower_enabled;

                    ui.SetSelectedRow(index + 1);
                } catch (Exception ex) {
                    Logger.StdErr("FormWordDictionary#btnDown_Click; ex=" + ex);
                }
            }
        }

        public void buttonUpClick()
        {
			var selected = ui.listDictionaries.SelectedIndices;
			int index = selected.Count > 0 ? (int) selected [0] : -1;
            if (index >= 1) {
                try {
					ui.listDictionaries.SelectedIndices.Clear();
					string upper_name = ui.listDictionaries.Items[index - 1].Text;
					bool upper_enabled = ui.listDictionaries.Items[index - 1].Checked;
					string lower_name = ui.listDictionaries.Items[index].Text;
					bool lower_enabled = ui.listDictionaries.Items[index].Checked;

					ui.listDictionaries.Items[index - 1].Text = lower_name;
					ui.listDictionaries.Items[index - 1].Checked = lower_enabled;
					ui.listDictionaries.Items[index].Text = upper_name;
					ui.listDictionaries.Items[index].Checked = upper_enabled;

                    ui.SetSelectedRow(index - 1);
                } catch (Exception ex) {
                    Logger.StdErr("FormWordDictionary#btnUp_Click; ex=" + ex);
                }
            }
        }

        public void buttonOkClick()
        {
            ui.setDialogResult(true);
        }

        public void formLoad()
        {
			ui.listDictionaries.Items.Clear();
            for (int i = 0; i < SymbolTable.getCount(); i++) {
                string name = SymbolTable.getSymbolTable(i).getName();
                bool enabled = SymbolTable.getSymbolTable(i).isEnabled();
				ui.listDictionaries.AddRow(new string[] {name}, enabled);
            }
        }

        public void formClosing()
        {
            mColumnWidth = ui.listDictionariesGetColumnWidth();
            mWidth = ui.getWidth();
            mHeight = ui.getHeight();
        }

        #endregion


        #region public methods

        public void close()
        {
            ui.close();
        }

        public UiForm getUi()
        {
            return ui;
        }

        public int getWidth()
        {
            return ui.getWidth();
        }

        public int getHeight()
        {
            return ui.getHeight();
        }

        public void setLocation(int x, int y)
        {
            ui.setLocation(x, y);
        }

        public void applyLanguage()
        {
            ui.setTitle(_("User Dictionary Configuration"));
            ui.labelAvailableDictionariesSetText(_("Available Dictionaries"));
            ui.buttonOkSetText(_("OK"));
            ui.buttonCancelSetText(_("Cancel"));
            ui.buttonUpSetText(_("Up"));
            ui.buttonDownSetText(_("Down"));
        }

        public List<ValuePair<string, Boolean>> getResult()
        {
            List<ValuePair<string, Boolean>> ret = new List<ValuePair<string, Boolean>>();
#if DEBUG
			Logger.StdOut("FormWordDictionary#getResult; count=" + ui.listDictionaries.Items.Count);
#endif
			for (int i = 0; i < ui.listDictionaries.Items.Count; i++) {
				string name = ui.listDictionaries.Items[i].Text;

                ret.Add(new ValuePair<string, Boolean>(
					ui.listDictionaries.Items[i].Text, ui.listDictionaries.Items[i].Checked));
            }
            return ret;
        }

        #endregion


        #region private methods

        private static string _(string id)
        {
            return Messaging.getMessage(id);
        }

        #endregion
    }

}
