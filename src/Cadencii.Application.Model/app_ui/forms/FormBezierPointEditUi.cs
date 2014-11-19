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

        [PureVirtualFunction]
        bool isEnableSmoothSelected();

        [PureVirtualFunction]
        void setEnableSmoothSelected(bool value);

        [PureVirtualFunction]
        void setGroupDataPointTitle(string value);

        [PureVirtualFunction]
        void setLabelDataPointClockText(string value);

        [PureVirtualFunction]
        void setLabelDataPointValueText(string value);

        [PureVirtualFunction]
        void setGroupLeftTitle(string value);

        [PureVirtualFunction]
        void setLabelLeftClockText(string value);

        [PureVirtualFunction]
        void setLabelLeftValueText(string value);

        [PureVirtualFunction]
        void setGroupRightTitle(string value);

        [PureVirtualFunction]
        void setLabelRightClockText(string value);

        [PureVirtualFunction]
        void setLabelRightValueText(string value);

        [PureVirtualFunction]
        void setCheckboxEnableSmoothText(string value);
    }

}
