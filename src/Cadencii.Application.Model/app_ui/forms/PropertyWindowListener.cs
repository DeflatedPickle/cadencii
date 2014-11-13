/*
 * PropertyWindowListener.cs
 * Copyright © 2012 kbinani
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
using Cadencii.Application.Models;


namespace Cadencii.Application.Forms
{

    public interface PropertyWindowListener
    {
	FormMainModel Model { get; }
    }

}
