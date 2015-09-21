#-*- encoding:utf-8 -*-

#ぐにぐにする

import clr
clr.AddReference("PresentationCore")
clr.AddReference("System.Xaml")
clr.AddReference("PresentationFramework")

from System.IO import StringReader
from System.Xaml import XamlServices
from System.Windows import PropertyPath
from System.Windows.Media.Animation import Storyboard, Timeline


def makeAnimation():
    xamlStr = """
<Storyboard xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
    <DoubleAnimationUsingKeyFrames>
        <DiscreteDoubleKeyFrame KeyTime="0" Value="0"/>
        <DiscreteDoubleKeyFrame KeyTime="0:0:0.3" Value="0"/>
        <EasingDoubleKeyFrame KeyTime="0:0:0.7" Value="-70">
            <EasingDoubleKeyFrame.EasingFunction>
                <QuadraticEase EasingMode="EaseOut"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
        <EasingDoubleKeyFrame KeyTime="0:0:1.1" Value="0">
            <EasingDoubleKeyFrame.EasingFunction>
                <QuadraticEase EasingMode="EaseIn"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
    </DoubleAnimationUsingKeyFrames>
    <DoubleAnimationUsingKeyFrames>
        <DiscreteDoubleKeyFrame KeyTime="0" Value="1"/>
        <EasingDoubleKeyFrame KeyTime="0:0:0.15" Value="1.3">
            <EasingDoubleKeyFrame.EasingFunction>
                <QuadraticEase EasingMode="EaseOut"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
        <EasingDoubleKeyFrame KeyTime="0:0:0.3" Value="1.15">
            <EasingDoubleKeyFrame.EasingFunction>
                <QuadraticEase EasingMode="EaseIn"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
        <EasingDoubleKeyFrame KeyTime="0:0:0.7" Value="1">
            <EasingDoubleKeyFrame.EasingFunction>
                <ElasticEase EasingMode="EaseOut" Springiness="1.5" Oscillations="2"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
        <DiscreteDoubleKeyFrame KeyTime="0:0:1.1" Value="1"/>
    </DoubleAnimationUsingKeyFrames>
    <DoubleAnimationUsingKeyFrames>
        <DiscreteDoubleKeyFrame KeyTime="0" Value="1"/>
        <EasingDoubleKeyFrame KeyTime="0:0:0.15" Value="0.77">
            <EasingDoubleKeyFrame.EasingFunction>
                <QuadraticEase EasingMode="EaseOut"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
        <EasingDoubleKeyFrame KeyTime="0:0:0.3" Value="0.84">
            <EasingDoubleKeyFrame.EasingFunction>
                <QuadraticEase EasingMode="EaseIn"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
        <EasingDoubleKeyFrame KeyTime="0:0:0.7" Value="1">
            <EasingDoubleKeyFrame.EasingFunction>
                <ElasticEase EasingMode="EaseOut" Springiness="1.5" Oscillations="2"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
        <DiscreteDoubleKeyFrame KeyTime="0:0:1.1" Value="1"/>
    </DoubleAnimationUsingKeyFrames>
</Storyboard>
"""

    sr = StringReader(xamlStr)
    sb = XamlServices.Load(sr)
    sr.Dispose()

    #FIXME: 入れる先が用意されてないのでダメ
    harriet.Window.Window.ScaleX = 1.3

    Storyboard.SetTarget(sb.Children[0], harriet.Window.Window)
    Storyboard.SetTarget(sb.Children[1], harriet.Character)
    Storyboard.SetTarget(sb.Children[2], harriet.Character)

    Storyboard.SetTargetProperty(sb.Children[0], PropertyPath("TranslateY"))
    Storyboard.SetTargetProperty(
        sb.Children[1], 
        PropertyPath("RenderTransform.Children[2].ScaleX")
        )
    Storyboard.SetTargetProperty(
        sb.Children[2], 
        PropertyPath("RenderTransform.Children[2].ScaleY")
        )

    return sb


def makeEndAnimation():
    xamlStr = """
<Storyboard xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
    <DoubleAnimationUsingKeyFrames>
        <DiscreteDoubleKeyFrame KeyTime="0" Value="1"/>
        <EasingDoubleKeyFrame KeyTime="0:0:0.15" Value="1.2">
            <EasingDoubleKeyFrame.EasingFunction>
                <QuadraticEase EasingMode="EaseOut"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
        <EasingDoubleKeyFrame KeyTime="0:0:0.3" Value="1.0">
            <EasingDoubleKeyFrame.EasingFunction>
                <QuadraticEase EasingMode="EaseIn"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
    </DoubleAnimationUsingKeyFrames>
    <DoubleAnimationUsingKeyFrames>
        <DiscreteDoubleKeyFrame KeyTime="0" Value="1"/>
        <EasingDoubleKeyFrame KeyTime="0:0:0.15" Value="0.84">
            <EasingDoubleKeyFrame.EasingFunction>
                <QuadraticEase EasingMode="EaseOut"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
        <EasingDoubleKeyFrame KeyTime="0:0:0.3" Value="1.0">
            <EasingDoubleKeyFrame.EasingFunction>
                <QuadraticEase EasingMode="EaseIn"/>
            </EasingDoubleKeyFrame.EasingFunction>
        </EasingDoubleKeyFrame>
    </DoubleAnimationUsingKeyFrames>
</Storyboard>
"""

    sr = StringReader(xamlStr)
    sb = XamlServices.Load(sr)
    sr.Dispose()

    #FIXME: 入れる先が用意されてないのでダメ
    Storyboard.SetTarget(sb.Children[0], harriet.Character)
    Storyboard.SetTarget(sb.Children[1], harriet.Character)

    Storyboard.SetTargetProperty(
        sb.Children[0], 
        PropertyPath("RenderTransform.Children[2].ScaleX")
        )
    Storyboard.SetTargetProperty(
        sb.Children[1], 
        PropertyPath("RenderTransform.Children[2].ScaleY")
        )

    return sb

def _jump():
    sb1 = makeAnimation()
    sb2 = makeEndAnimation()
    sb1.Completed += lambda _, __ : sb2.Begin()
    sb1.Begin()

def jump():
    harriet.Invoke(_jump)

def _set_walk_motion():
    sb = makeAnimation()
    sbEnd = makeEndAnimation()
    #FIXME: 作ったアニメーションを入れないとだめ


def set_walk_motion():
    harriet.Invoke(_set_walk_motion)


