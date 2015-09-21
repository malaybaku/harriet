# -*- coding: utf-8 -*-

from System import Action, Uri
from System.Threading.Tasks import Task
from System.Diagnostics import Process

import clr
clr.AddReference("PresentationFramework")
from System.Windows.Controls import TextBlock, ScrollViewer, ScrollBarVisibility
from System.Windows.Documents import Run, Hyperlink, LineBreak

import rssparser

#テキストブロックをUIスレッド上で作成して表示する
def show(rssItems):

    def _OnRequestNavigate(sender, e):
        Task.Run.Overloads[Action](lambda : Process.Start(e.Uri.AbsoluteUri))

    #タイトルだけ拾ってテキストブロックを構成する
    def _getTextBlock(rss):
        
        result = TextBlock()
        for item in rss:
            run = Run(item.Title)
            hlink = Hyperlink(run)

            hlink.NavigateUri = Uri(item.Url)
            hlink.Foreground = harriet.Setting.ChatWindowColor.Foreground
            hlink.RequestNavigate += _OnRequestNavigate

            result.Inlines.Add(hlink)
            result.Inlines.Add(LineBreak())
            result.Inlines.Add(LineBreak())

        return result

    #TextBlockをスクロールビューアに包んで表示する
    def _showTextBlock():
        sv = ScrollViewer()
        sv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto
        sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
        sv.Content = _getTextBlock(rssItems)
        harriet.ChatWindow.RenderContent(sv)

    harriet.Invoke(_showTextBlock)
