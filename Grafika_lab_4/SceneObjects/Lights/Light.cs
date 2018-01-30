﻿using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika_lab_4.Lights
{
    public abstract class Light
    {
        public Vector3 Position=Vector3.UnitZ;
        public Vector3 LightColor=Vector3.One;

    }
}