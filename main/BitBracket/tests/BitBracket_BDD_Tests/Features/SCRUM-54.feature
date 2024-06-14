# Feature: SCRUM-54  Broadcast Integration for Tournament Viewing Convenience

# Organizers can add a broadcast via a form submit on the Tournament page and be displayed.
# Organizers have the option between using Twitch or YouTube as their broadcast platform

# @Will
# Scenario: TournamentPage page has a Set Broadcast buttton
# 	Given I am a User
# 	When I am on the "TournamentPage" page
# 	Then The page presents a Set Broadcast buttton

# Scenario: TournamentPage page sets Twitch broadcast
# 	Given I am a User
# 	When I am on the "TournamentPage" page
# 		And enter "111" into the Broadcast Channel textbox
# 		And click the Set Broadcast buttton
# 	Then the page will reload and a iframe container will appear

# Scenario: TournamentPage page sets YouTube broadcast
# 	Given I am a User
# 	When I am on the "TournamentPage" page
#         And select the YouTube drop down option in the form
# 		And enter "111" into the Broadcast Channel textbox
# 		And click the Set Broadcast buttton
# 	Then the page will reload and a iframe container will appear

# Scenario: TournamentPage page Twitch broadcast is interactable
# 	Given I am a User
# 	When I am on the "TournamentPage" page
# 		And enter "111" into the Broadcast Channel textbox
# 		And click the Set Broadcast buttton
#         And the page reloads with a iframe container
# 	Then the page the user can interact with the bottom left of the iframe

# Scenario: TournamentPage page YouTube broadcast is interactable
# 	Given I am a User
# 	When I am on the "TournamentPage" page
#         And select the YouTube drop down option in the form
# 		And enter "111" into the Broadcast Channel textbox
# 		And click the Set Broadcast buttton
#         And the page reloads with a iframe container
# 	Then the page the user can interact with the bottom right of the iframe