# -*- coding: utf-8 -*- 

#デスクトップマスコットらしく右下に陣取るためのモジュール

import clr
clr.AddReference("System.Windows.Forms")

#単一ディスプレイならコレ
from System.Windows.Forms.Screen import PrimaryScreen
#複数ディスプレイでなんかする場合
#from System.Windows.Forms.Screen import AllScreens

import dpi_getter

def goto_bottomleft():
    """キャラを強制的に画面左下へ移動させる"""

    psArea = PrimaryScreen.WorkingArea
    left = psArea.X
    bottom = psArea.Y + psArea.Height

    dpiX, dpiY = dpi_getter.get_scalefactor()

    w, h = harriet.Window.Width, harriet.Window.Height

    harriet.Warp(left / dpiX + w * 0.5, bottom / dpiY - h * 0.5)


def goto_bottomright():
    """キャラを強制的に画面右下へ移動させる"""

    psArea = PrimaryScreen.WorkingArea
    right = psArea.X + psArea.Width
    bottom = psArea.Y + psArea.Height

    dpiX, dpiY = dpi_getter.get_scalefactor()

    w, h = harriet.Window.Width, harriet.Window.Height

    harriet.Warp(right / dpiX - w * 0.5, bottom / dpiY - h * 0.5)

