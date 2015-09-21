# -*- encoding: utf-8 -*-

import System
from System import Uri
from System.Net import WebClient
from System.Text.Encoding import UTF8

import clr
clr.AddReference("System.Core")
clr.AddReference("System.Xml")
clr.AddReference("System.Data.DataSetExtensions")
clr.AddReference("dotNetRDF")
clr.AddReference("System.ServiceModel")

from System.Xml import XmlDocument, XmlElement, XmlReader
from System.ServiceModel.Syndication import SyndicationFeed
from System.Data import DataTableExtensions

from VDS.RDF import Graph
from VDS.RDF.Parsing import UriLoader

clr.ImportExtensions(System.Linq)
clr.ImportExtensions(DataTableExtensions)


class RSSItem(object):

    def __init__(self, title, summary, url):
        self.Title = title
        self.Summary = summary
        self.Url = url

def get_rss1(url):
    g = Graph()
    UriLoader.Load(g, Uri(url))
    return [
        RSSItem(str(f.ItemArray[2]), "", str(f.ItemArray[0]))
            for f in g.ToDataTable().AsEnumerable()
            if "title" in str(f.ItemArray[1])
        ]

def get_rss2(url):
    try:
        with XmlReader.Create(url) as reader:
            return [
                RSSItem(
                    i.Title.Text, 
                    i.Summary.Text, 
                    i.Links[0].Uri.AbsoluteUri if i.Links.Count > 0 else ""
                    ) for i in 
                SyndicationFeed.Load(reader).Items
                ]
    except XmlException:
        wc = WebClient()
        wc.Encoding = UTF8
        xmlstr = wc.DownloadString(url)
        xdoc = XmlDocument()
        xdoc.LoadXml(xmlstr)
        xelem = xdoc.DocumentElement

        titles = [
            i.InnerText.Replace("\n", "").Replace("\r", "")
            for i in xelem.SelectNodes("//item//title")]
        links = [i.InnerText for i in xelem.SelectNodes("//item//link")]
        descriptions = [
            i.InnerText for i in xelem.SelectNodes("//item//description")
            ]
        return [
            RSSItem(t, d, l) for t, d, l 
            in zip(titles, descriptions, links)
            ]

def get_rss(url):
    try:
        rssXml = XmlDocument()
        rssXml.Load(url)
        rssElem = rssXml.DocumentElement
        rootNodeTag = rssElem.Name
        if rootNodeTag == "rdf:RDF":
            return get_rss1(url)
        elif rootNodeTag == "rss":
            return get_rss2(url)
        else:
            return None
    except:
        return None
