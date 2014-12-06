#if ENABLE_PROPERTY
/*
 * PropertyPanel.cs
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
using System.Collections.Generic;
using cadencii.apputil;
using cadencii.java.util;
using Cadencii.Media.Vsq;
using Cadencii.Gui;
using Cadencii.Gui.Toolkit;
using cadencii;

namespace Cadencii.Application.Controls
{

    public class PropertyPanelImpl : UserControlImpl, PropertyPanel
    {
        public event CommandExecuteRequiredEventHandler CommandExecuteRequired;
        private List<SelectedEventEntry> m_items;
        private int m_track;
        private bool m_editing;

        public PropertyPanelImpl()
        {
            InitializeComponent();
            registerEventHandlers();
            setResources();
            m_items = new List<SelectedEventEntry>();
            AwtHost.Current.ApplyFontRecurse(this, EditorManager.editorConfig.getBaseFont());
        }

        public bool isEditing()
        {
            return m_editing;
        }

        public void setEditing(bool value)
        {
            m_editing = value;
        }

        private void popUiGridItemExpandStatus()
        {
            if (propertyGrid.SelectedGridItem == null) {
                return;
            }

            UiGridItem root = findRootGridItem(propertyGrid.SelectedGridItem);
            if (root == null) {
                return;
            }

            popUiGridItemExpandStatusCore(root);
        }

        private void popUiGridItemExpandStatusCore(UiGridItem item)
        {
            if (item.Expandable) {
                string s = getGridItemIdentifier(item);
                foreach (var v in EditorManager.editorConfig.PropertyWindowStatus.ExpandStatus) {
                    string key = v.getKey();
                    if (key == null) {
                        key = "";
                    }
                    if (key.Equals(s)) {
                        item.Expanded = v.getValue();
                        break;
                    }
                }
            }
            foreach (UiGridItem child in item.GridItems) {
                popUiGridItemExpandStatusCore(child);
            }
        }

        private void pushUiGridItemExpandStatus()
        {
            if (propertyGrid.SelectedGridItem == null) {
                return;
            }

            UiGridItem root = findRootGridItem(propertyGrid.SelectedGridItem);
            if (root == null) {
                return;
            }

            pushUiGridItemExpandStatusCore(root);
        }

        private void pushUiGridItemExpandStatusCore(UiGridItem item)
        {
            if (item.Expandable) {
                string s = getGridItemIdentifier(item);
                bool found = false;
                foreach (var v in EditorManager.editorConfig.PropertyWindowStatus.ExpandStatus) {
                    string key = v.getKey();
                    if (key == null) {
                        continue;
                    }
                    if (v.getKey().Equals(s)) {
                        found = true;
                        v.setValue(item.Expanded);
                    }
                }
                if (!found) {
                    EditorManager.editorConfig.PropertyWindowStatus.ExpandStatus.Add(new ValuePairOfStringBoolean(s, item.Expanded));
                }
            }
            foreach (UiGridItem child in item.GridItems) {
                pushUiGridItemExpandStatusCore(child);
            }
        }

        public void updateValue(int track)
        {
            m_track = track;
            m_items.Clear();

            // 現在のUiGridItemの展開状態を取得
            pushUiGridItemExpandStatus();

            Object[] objs = new Object[EditorManager.itemSelection.getEventCount()];
            int i = -1;
            foreach (var item in EditorManager.itemSelection.getEventIterator()) {
                i++;
                objs[i] = item;
            }

            propertyGrid.SelectedObjects = objs;
            popUiGridItemExpandStatus();
            setEditing(false);
        }

        public void propertyGrid_PropertyValueChanged()
        {
            Object[] selobj = propertyGrid.SelectedObjects;
            int len = selobj.Length;
            VsqEvent[] items = new VsqEvent[len];
            for (int i = 0; i < len; i++) {
                SelectedEventEntry proxy = (SelectedEventEntry)selobj[i];
                items[i] = proxy.editing;
            }
            CadenciiCommand run = new CadenciiCommand(VsqCommand.generateCommandEventReplaceRange(m_track, items));
            if (CommandExecuteRequired != null) {
                CommandExecuteRequired(this, run);
            }
            for (int i = 0; i < len; i++) {
                EditorManager.itemSelection.addEvent(items[i].InternalID);
            }
            propertyGrid.Refresh();
            setEditing(false);
        }

        /// <summary>
        /// itemが属しているUiGridItemツリーの基点にある親を探します
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private UiGridItem findRootGridItem(UiGridItem item)
        {
            if (item.Parent == null) {
                return item;
            } else {
                return findRootGridItem(item.Parent);
            }
        }

        /// <summary>
        /// itemが属しているUiGridItemツリーの中で，itemを特定するための文字列を取得します
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string getGridItemIdentifier(UiGridItem item)
        {
            if (item.Parent == null) {
                if (item.PropertyDescriptor != null) {
                    return item.PropertyDescriptor.Name;
                } else {
                    return item.Label;
                }
            } else {
                if (item.PropertyDescriptor != null) {
                    return getGridItemIdentifier(item.Parent) + "@" + item.PropertyDescriptor.Name;
                } else {
                    return getGridItemIdentifier(item.Parent) + "@" + item.Label;
                }
            }
        }

        private void propertyGrid_SelectedGridItemChanged()
        {
            setEditing(true);
        }

        public void propertyGrid_Enter(Object sender, EventArgs e)
        {
            setEditing(true);
        }

        public void propertyGrid_Leave(Object sender, EventArgs e)
        {
            setEditing(false);
        }

        private void registerEventHandlers()
        {
			propertyGrid.SelectedGridItemChanged += (o,e) => propertyGrid_SelectedGridItemChanged ();
            propertyGrid.Leave += propertyGrid_Leave;
            propertyGrid.Enter += propertyGrid_Enter;
			propertyGrid.PropertyValueChanged += (o,e) => propertyGrid_PropertyValueChanged ();
        }

        private void setResources()
        {
        }

        #region UI Impl for C#
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.propertyGrid = ApplicationUIHost.Create<UiPropertyGrid>();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = DockStyle.Fill;
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.Location = new Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.PropertySort = PropertySort.Categorized;
			this.propertyGrid.Size = new Dimension(191, 298);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // PropertyPanel
            // 
			var ctrl = (UiUserControl) this;
			ctrl.AutoScaleMode = Cadencii.Gui.Toolkit.AutoScaleMode.None;
			ctrl.Controls.Add(this.propertyGrid);
			ctrl.Name = "PropertyPanel";
			ctrl.Size = new Dimension(191, 298);
			ctrl.ResumeLayout(false);

        }

        #endregion

        UiPropertyGrid propertyGrid;
        #endregion
    }

}
#endif
