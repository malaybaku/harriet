# -*- encoding: utf-8 -*-

import clr
clr.AddReference("System.Drawing")
from System.Drawing import Graphics

from System import IntPtr

def get_scalefactor():
    g = Graphics.FromHwnd(IntPtr.Zero)
    return g.DpiX / 96.0, g.DpiY / 96.0

