Feature: SCRUM-14 Create Basic Bracket, SCRUM-34 Create Double Elimination Bracket Feature

Visitors can make a bracket via the form submit and be displayed as a bracket.
Visitors have the option between bracket formats with single elimination and double elimination

@Will
Scenario: CreateBracket page has a Generate Bracket buttton
	Given I am a visitor
	When I am on the "CreateBracket" page
	Then The page presents a Generate Bracket buttton

Scenario: CreateBracket page has a textbox form
	Given I am a visitor
	When I am on the "CreateBracket" page
	Then I am able to enter text into the textbox form

Scenario: CreateBracket page Generates bracket
	Given I am a visitor
	When I am on the "CreateBracket" page
		and enter "Player1, Player2" into the textbox
		and click the Generate Bracket button 
	Then a Bracket container will appear

Scenario: CreateBracket page Generates Double Elimination bracket
	Given I am a visitor
	When I am on the "CreateBracket" page
		and enter "Player1, Player2, Player3, Player4" into the textbox
		and select "Double Elimination" from the drop down menu
		and click the Generate Bracket button 
	Then a loserBracket container will appear
