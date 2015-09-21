#-*- encoding:utf-8 -*-

#OpacityMask用のブラシを定義し、ロールイン演出を定義する

import clr
clr.AddReference("PresentationCore")
clr.AddReference("System.Xaml")
clr.AddReference("PresentationFramework")

from System.IO import StringReader
from System.Xaml import XamlServices

def getOpacityMaskAndStoryboard(duration):
    xamlStr = """
<Rectangle xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" 
           xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
           Width="10" Height="10">
    <Rectangle.Resources>
        <Storyboard x:Key="Appear">
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(GradientBrush.GradientStops)[0].(GradientStop.Offset)"
                                           Storyboard.TargetName="OpacityBrush">
                <EasingDoubleKeyFrame KeyTime="0:0:0" Value="-0.1"/>
                <EasingDoubleKeyFrame KeyTime="0:0:{0}" Value="1">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <CubicEase EasingMode="EaseInOut"/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty="(GradientBrush.GradientStops)[1].(GradientStop.Offset)"
                                           Storyboard.TargetName="OpacityBrush">
                <EasingDoubleKeyFrame KeyTime="0:0:0" Value="0"/>
                <EasingDoubleKeyFrame KeyTime="0:0:{0}" Value="1.1">
                    <EasingDoubleKeyFrame.EasingFunction>
                        <CubicEase EasingMode="EaseInOut"/>
                    </EasingDoubleKeyFrame.EasingFunction>
                </EasingDoubleKeyFrame>
            </DoubleAnimationUsingKeyFrames>
        </Storyboard>
    </Rectangle.Resources>
    <Rectangle.OpacityMask>
        <LinearGradientBrush x:Name="OpacityBrush" EndPoint="0.5,1" StartPoint="0.5,0">
            <GradientStop Offset="0" Color="Transparent" />
            <GradientStop Offset="0" Color="Black" />
        </LinearGradientBrush>
    </Rectangle.OpacityMask>
</Rectangle>
""".format(duration)

    with StringReader(xamlStr) as sr:
        rect = XamlServices.Load(sr)  

    sb = rect.TryFindResource("Appear")
    opMask = rect.OpacityMask
    return opMask, sb


def start_animation(duration):
    try:
        brush, sb = getOpacityMaskAndStoryboard(duration)
        harriet.Character.OpacityMask = brush
        sb.Begin()
    except AttributeError:
        #OpacityMaskプロパティが無いキャラではこの効果は使えない
        pass

def rollout(duration):
    harriet.Invoke(lambda : start_animation(duration))
