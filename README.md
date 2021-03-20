# NICE.Timelines
This repository contains a proof-of-concept / alpha spike, to retrieve data from ClickUp's API and populate a local database, in order to enable existing SQL Reporting Services reports to run.


## More details
The codebase supports that data replication in 2 ways:

1. An API which is hit by ClickUp webhooks when data is changed. - i.e. the data is pushed
2. A console app which runs on-demand, or on schedule, which pulls all data matching a filter the ClickUp API.


There are four solutions in the code:

### NICE.Timelines.API

This is a WebAPI project, which uses a swagger front-end. It exposes 2 endpoints - SaveOrUpdate and Delete.

These endpoints are hit by ClickUp's Webhook automation feature, when the data changes in ClickUp. 

### NICE.Timelines.ConsoleApp

This is a console application that hits the ClickUp API. It works off a ClickUp Space id, then from that, hits the API to retrieve all the folders within the space, then the lists within the folder.
It then iterates those lists, pulling back all the tasks that match a filter. The database is updated with each task, or data is deleted if it no longer matches the filter.

### NICE.Timelines.Common

The data model sent by the webhook matches the data retrieved by the API calls, so the data models are stored here.

Also the field id's are stored here in constants. The fields will need to be locked in ClickUp due to this coupling.

### NICE.Timelines.DB

The responsibility of this project is to save or delete the data from the database. It uses Entity Framework Core.




