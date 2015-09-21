# -*- encoding: utf-8 -*-

from System.Net import WebClient, WebException
from System.Text.Encoding import UTF8

import clr
clr.AddReference("HtmlAgilityPack")
from HtmlAgilityPack import HtmlDocument

def _get_htmldoc(url, encode=UTF8):
    """指定したURLのHTMLテキストを取得。失敗するとNoneを返す"""
    wc = WebClient()
    wc.Encoding = encode
    try:
        html = wc.DownloadString(url)
    except WebException as ex:
        return None

    htmlDoc = HtmlDocument()
    htmlDoc.LoadHtml(html)
    return htmlDoc

def get_first_node(url, xpath, encode=UTF8):
    """指定したURLのHTMLテキストをダウンロードし、
    指定したXPathに該当するHTMLノード(タグ)で最初に現れたものを取得する。
    失敗するとNoneを返す

    url: HTMLのURLを表す文字列
    xpath: ノード(タグ)を指定するためのXPath文字列
    encode: HTMLの文字列エンコード(規定値: UTF8)

    return: XPathで指定したノードのうち最初に見つかったもの
            失敗した場合はNone
    """
    htmlDoc = _get_htmldoc(url, encode)
    if htmlDoc:
        return htmlDoc.DocumentNode.SelectSingleNode(xpath)
    else:
        return None

def get_nodes(url, xpath, encode=UTF8):
    """指定したURLのHTMLテキストから
    指定したXPathに該当するHTMLノード(タグ)一覧を
    リストとして取得する。失敗すると空のリストを返す

    url: HTMLのURLを表す文字列
    xpath: ノード(タグ)を指定するためのXPath文字列
    encode: HTMLの文字列エンコード(規定値: UTF8)

    戻り値: XPathで指定したノードのリスト。失敗した場合は空リスト
    """

    htmlDoc = _get_htmldoc(url, encode)
    if not htmlDoc:
        return []

    result = htmlDoc.DocumentNode.SelectNodes(xpath)
    if result:
        return result
    else:
        return []

