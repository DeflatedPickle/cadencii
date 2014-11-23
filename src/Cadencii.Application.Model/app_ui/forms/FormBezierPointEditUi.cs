/*
 * FormBezierPointEditUi.cs
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
using cadencii;
using Cadencii.Gui.Toolkit;
using Cadencii.Application.Controls;

namespace Cadencii.Application.Forms
{

    public interface FormBezierPointEditUi : UiForm
    {
		NumberTextBox txtDataPointClock { get; }
		NumberTextBox txtDataPointValue { get; }
		NumberTextBox txtLeftClock { get; }
		NumberTextBox txtLeftValue { get; }
		NumberTextBox txtRightClock { get; }
		NumberTextBox txtRightValue { get; }
		UiButton btnLeft { get; set; }
		UiButton btnRight { get; set; }
		UiCheckBox chkEnableSmooth { get; set; }
		UiLabel lblLeftValue { get; set; }
		UiLabel lblLeftClock { get; set; }
		UiLabel lblDataPointValue { get; set; }
		UiLabel lblDataPointClock { get; set; }
		UiLabel lblRightValue { get; set; }
		UiLabel lblRightClock { get; set; }
		UiGroupBox groupDataPoint { get; set; }
		UiGroupBox groupLeft { get; set; }
		UiGroupBox groupRight { get; set; }
    }

}
