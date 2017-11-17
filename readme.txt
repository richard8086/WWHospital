
Hospital Simulator Project

Build: Need Visual Studio 2015 or higher. I did not have time to do a MS build script
Run: Use the Visual Studio to run. Or use the deploy package

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
Did complete the unit test code due to time constraint.

Other considerations:
right now there's not resource optimization technique implemented, e.g., flu patients don't need room with machine, 
but the current implementation does not take this into account to save the room for cancer patients to get the same earliest date



