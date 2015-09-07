#-*- encoding:utf-8 -*-

#堅めのジャンプ

import clr
clr.AddReference("PresentationCore")
clr.AddReference("System.Xaml")
clr.AddReference("PresentationFramework")

from System.IO import StringReader
from System.Xaml import XamlServices
from System.Windows import PropertyPath
from System.Windows.Media.Animation import Storyboard


def makeAnimation(top):
    xamlStr = """
<Storyboard xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
    <DoubleAnimationUsingKeyFrames>
        <EasingDoubleKeyFrame KeyTime="0:0:0.2" Value="{0}">
            <EasingDoubleKeyFrame.EasingFunction>
                <QuadraticEase EasingMode="EaseOut"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
        <EasingDoubleKeyFrame KeyTime="0:0:0.4" Value="{1}">
            <EasingDoubleKeyFrame.EasingFunction>
                <QuadraticEase EasingMode="EaseIn"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
        <EasingDoubleKeyFrame KeyTime="0:0:0.6" Value="{2}">
            <EasingDoubleKeyFrame.EasingFunction>
                <QuadraticEase EasingMode="EaseOut"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
        <EasingDoubleKeyFrame KeyTime="0:0:0.8" Value="{3}">
            <EasingDoubleKeyFrame.EasingFunction>
                <QuadraticEase EasingMode="EaseIn"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
    </DoubleAnimationUsingKeyFrames>
</Storyboard>
""".format(top - 30, top, top - 30, top)

    sr = StringReader(xamlStr)
    sb = XamlServices.Load(sr)
    sr.Dispose()

    Storyboard.SetTarget(sb.Children[0], harriet.Window.Window)
    Storyboard.SetTargetProperty(sb.Children[0], PropertyPath("Top"))
    return sb


def start_animation():
    sb = makeAnimation(harriet.Window.Top)
    sb.Begin()

def jump():
    #NOTE: 理由不明だが、Invokeしてるのにスレッド例外で怒られるケース有り。
    harriet.Invoke(start_animation)
