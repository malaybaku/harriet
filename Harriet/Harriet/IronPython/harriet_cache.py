# -*- coding: utf-8 -*-

#スクリプト間のデータやり取りをファイル経由で行う為のサポートモジュール
from System.IO import File, Path, Directory

_basePath = Path.Combine(
    "characters", 
    harriet.CharacterName, 
    "script", 
    "cache"
    )

def get_path(filename):
    Directory.CreateDirectory(_basePath)
    return Path.Combine(_basePath, filename)

def exists(filename):
    Directory.CreateDirectory(_basePath)
    return File.Exists(get_path(filename))

