Query

```
{
	testCounter {
    availability {
      counter {
        values {
          date
          value
        }
      }
    }
  }
}
```

fails with this error
```
{
  "errors": [
    {
      "message": "Variable `__fields_serviceName` is required.",
      "extensions": {
        "code": "HC0018",
        "variable": "__fields_serviceName",
        "remote": {
          "message": "Variable `__fields_serviceName` is required.",
          "extensions": {
            "code": "HC0018",
            "variable": "__fields_serviceName"
          }
        },
        "schemaName": "network"
      }
    }
  ],
  "data": {
    "testCounter": {
      "availability": null
    }
  }
}
```