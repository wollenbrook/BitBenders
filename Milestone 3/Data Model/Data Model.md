## Data Modeling for Smart Bracket App:

**Entities:**

1. **User:**
   - Attributes: 
     - User ID (Unique identifier)
     - Username
     - Email
     - Password (hashed)
     - User type (Organizer, Player, Spectator)

2. **Tournament:**
   - Attributes:
     - Tournament ID (Unique identifier)
     - Tournament name
     - Game title
     - Date and time
     - Venue
     - Status (Upcoming, Ongoing, Completed)

3. **Bracket:**
   - Attributes:
     - Bracket ID (Unique identifier)
     - Tournament ID (Foreign key)
     - Bracket name
     - Number of rounds
     - Seeding method (Smart Seeding Algorithm)

4. **Match:**
   - Attributes:
     - Match ID (Unique identifier)
     - Bracket ID (Foreign key)
     - Match number
     - Date and time
     - Status (Upcoming, Ongoing, Completed)
     - Participants (Player IDs)
