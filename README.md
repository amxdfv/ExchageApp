# ExchageApp
Simple programm which allows to send data between two Oracle DataBases

First of all you need to fill ini-files in folder SeSH. In set.ini you write databases connection properties from tns-file in oracle client. In FrontToBack.ini you put list of tables which data you you want to send from first database to second. In BackToFront.ini you put list of tables which data you you want to send from second database to first.

Data sends by sql commands INSERT and UPDATE (not DELETE). It's important that tables in each databases have same columns, and first column must be an unique constraint.
