Query

```
{
  testCounter {
    availability(month: "2020-12-01") {
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

the fields are all null in the http request to the remote schema
```
{
   "query": "query fetch($__scopedContextData_customer: String!, $__fields_serviceName: String, $__fields_hostname: String, $__fields_location: String, $__arguments_month: Date = null, $__fields_threshold: Float) { metricsByCustomer(customer: $__scopedContextData_customer) { device(counter: $__fields_serviceName, hostname: $__fields_hostname, location: $__fields_location) { availability(month: $__arguments_month, displayLastMonth: true, threshold: $__fields_threshold) { counter { values { date value __typename } __typename } __typename } } } }",
   "variables": {
      "__scopedContextData_customer": "TST",
      "__fields_serviceName": null,
      "__fields_hostname": null,
      "__fields_location": null,
      "__arguments_month": "2020-12-01",
      "__fields_threshold": null
   }
}
```