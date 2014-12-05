/*
 * ProgressBarWithLabel.cs
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
using System;

namespace Cadencii.Application.Controls
{

    /// <summary>
    /// 実行内容を表示するためのラベル付きのプログレスバー
    /// </summary>
    public class ProgressBarWithLabelModel
    {
        private ProgressBarWithLabel ptrUi = null;

        /// <summary>
        /// UIのセットアップを行います
        /// </summary>
        /// <param name="ui"></param>
        public void setupUi(ProgressBarWithLabel ui)
        {
            if (ptrUi == null) {
                ptrUi = ui;
            }
        }

        /// <summary>
        /// セットアップされているUIを取得します
        /// </summary>
        /// <returns></returns>
        public ProgressBarWithLabel getUi()
        {
            return ptrUi;
        }

		public int Width {
			get { return ptrUi.Width; }
			set { ptrUi.Width = value; }
		}

		public string Text {
			get { return ptrUi.Text; }
			set { ptrUi.Text = value; }
		}

		public int Progress {
			get { return ptrUi.Progress; }
			set { ptrUi.Progress = value; }
		}
    }
}
