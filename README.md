# AdmireLogging

### Description
Dll library for logging exceptions to Elasticsearch <br/>
**Elasticsearch index:** admire-error-log <br/>
**Elasticsearch document example:**<br/>
```
{
  "_index": "admire-error-log",
  "_type": "_doc",
  "_id": "LP6ysm8Bfx8MbVIwD_tC",
  "_score": 1.0,
  "_source": {
    "PluginName": "AdmirePluginOorahK4K",
    "FileName": "admFrmCustom01.vb",
    "Method": "InitForm",
    "Line": "321",
    "Exception": "I/O error occurred.",
    "Details": "InitForm at offset 283 in file:line:column C:\\Git\\AdmirePluginOorahK4K\\admFrmCustom01.vb:321:17\r\n",
    "DateTime": "2020-1-17 03:50:10.365"
  }
}
```


### Dll Dependencies
Newtonsoft.Json version 12.0.0.0

### Usage
```
Imports AdmireLogging
...
ElasticsearchLogger.LogException(exception)
```
