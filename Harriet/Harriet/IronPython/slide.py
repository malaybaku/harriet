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



def _getStoryboardAppliedToWindowProperty(xamlStr, propertyPath):
    with StringReader(xamlStr) as sr:
        sb = XamlServices.Load(sr)

    Storyboard.SetTarget(sb.Children[0], harriet.Window.Window)
    Storyboard.SetTargetProperty(
        sb.Children[0], 
        PropertyPath(propertyPath)
        )

    return sb

def _makeLinearStoryboardXamlStr(start, end, duration):
    return """
<Storyboard xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
    <DoubleAnimation From="{0:.2f}" To="{1:.2f}" Duration="0:0:{2:.2f}"/>
</Storyboard>
""".format(start, end, duration)


def slide_x(start, end, duration):
    
    def _subroutine():
        xamlStr = _makeLinearStoryboardXamlStr(start, end, duration)
        sb = _getStoryboardAppliedToWindowProperty(xamlStr, "Left")
        sb.Begin()

    harriet.Invoke(lambda : _subroutine())
    
def slide_y(start, end, duration):
    
    def _subroutine():
        xamlStr = _makeLinearStoryboardXamlStr(start, end, duration)
        sb = _getStoryboardAppliedToWindowProperty(xamlStr, "Top")
        sb.Begin()

    harriet.Invoke(lambda : _subroutine())
    

