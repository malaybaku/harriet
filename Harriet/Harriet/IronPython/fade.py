#-*- encoding:utf-8 -*-

#名前通りフェードイン演出のためのモジュール

import clr
clr.AddReference("PresentationCore")
clr.AddReference("System.Xaml")
clr.AddReference("PresentationFramework")

from System.IO import StringReader
from System.Xaml import XamlServices
from System.Windows import PropertyPath
from System.Windows.Media.Animation import Storyboard

def _makeFadeStoryboard(start, end, duration):
    xamlStr = """
<Storyboard xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
    <DoubleAnimation From="{0:.2f}" To="{1:.2f}" Duration="0:0:{2:.2f}" />
</Storyboard>
""".format(start, end, duration)

    with StringReader(xamlStr) as sr:
        sb = XamlServices.Load(sr)

    Storyboard.SetTarget(sb.Children[0], harriet.Window.Window)
    Storyboard.SetTargetProperty(
        sb.Children[0], 
        PropertyPath("(UIElement.Opacity)")
        )

    return sb


def fadein(duration=0.5):
    harriet.Invoke(lambda : _makeFadeStoryboard(0.0, 1.0, duration).Begin())

def fadeout(duration=0.5):
    harriet.Invoke(lambda : _makeFadeStoryboard(1.0, 0.0, duration).Begin())


