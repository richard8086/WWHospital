
Hospital Simulator Project

Build: Need Visual Studio 2015 or higher. I did not have time to do an MS build script
Run: Use the Visual Studio to run. Or use the package in the DeployedFiles to run in IIS.

Rest API(assuming the port is 49939)

http://localhost:49939/api/Consultations
// supports GET, to get a list of consulations

http://localhost:49939/api/Patients
// supports GET and POST, to get the list of patients or register a patient
// for POST body, here is the sample
{"Name":"SamplePatient1","Condition":"Cancer","Topology":"Breast"}
{"Name":"SamplePatient2","Condition":"Cancer","Topology":"HeadAndNeck"}
{"Name":"SamplePatient3","Condition":"Flu"}


http://localhost:49939/api/Patients/{PatientName}
// supports GET and DELETE, to get the patient info or delete a patient

Unit Test code:
Did not complete the unit test code due to time constraint. The unit test code is more of a demontration of how it could be done.

Other considerations:
Right now there's no resource optimization technique implemented, e.g., flu patients don't need room with machine, 
but the current implementation does not take this into account to save the room for cancer patients to get the same earliest date with a room without machine.

Also Mocking framework is to be added for unit testing thoroughly.

Hypermedia links could also be added.  




