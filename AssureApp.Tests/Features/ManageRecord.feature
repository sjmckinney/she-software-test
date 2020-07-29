Feature: Manage Air Emission Records
	In order to prove my competence
	As a writer of automated tests
	I want to go to https://stirling.she-development.net/automation
	And login using the supplied credentials
	And create a new record in the Air Emissions module
	And verify that the record was created
	And after deleting it, verify it no longer exists

Background:
	Given I navigate to the Assure login page
	And I login with username StephenK

@createRecord
Scenario: Create a new record in the Air Emissions module
	When I open the Air Emissions page from the Environment module
	And create a new record:
		| Date		| Description						| Location	|
		| Current	| Heavy particulate matter detected	|			|
	Then the record should be visible on the page

@deleteRecord
Scenario: Delete an existing record in the Air Emissions module
	Given I open the Air Emissions page
	And I locate the target record
	When I delete the record
	Then the record should no longer be visible on the page
