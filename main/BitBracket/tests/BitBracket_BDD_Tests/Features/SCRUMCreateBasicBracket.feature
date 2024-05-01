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
		And enter "Player1, Player2" into the textbox
		And click the Generate Bracket button 
	Then a Bracket container will appear

Scenario: CreateBracket page Generates Double Elimination bracket
	Given I am a visitor
	When I am on the "CreateBracket" page
		And enter "Player1, Player2, Player3, Player4" into the textbox
		And select "Double Elimination" from the drop down menu
		And click the Generate Bracket button 
	Then a loser Bracket container will appear

Scenario: CreateBracket page Generates bracket with editable match scores
	Given I am a visitor
	When I am on the "CreateBracket" page
		and enter "Player1, Player2, Player3, Player4" into the textbox
		and click the Generate Bracket button 
	Then a Bracket container will appear with matches with editable scores

Scenario: CreateBracket page Generates bracket and the bracket can progress in Single Elimination
	Given I am a visitor
	When I am on the "CreateBracket" page
		and enter "Player1, Player2, Player3, Player4" into the textbox
		and click the Generate Bracket button 
		and enter a winning score for one of the matches
	Then the second round of the bracket appear with the player that won

Scenario: CreateBracket page Generates bracket and the bracket can progress in Single Elimination
	Given I am a visitor
	When I am on the "CreateBracket" page
		and enter "Player1, Player2, Player3, Player4" into the textbox
		and select "Double Elimination" from the drop down menu
		and click the Generate Bracket button 
		and enter a winning score for one of the matches
	Then the first round of the loser bracket appear with the player that lost

