# Richie MVP

Richie is an AI agent that answers a business's customers on the business's website. A business adds one script tag and Richie appears in a chat widget. Richie answers every visitor from the business's own knowledge base, in the visitor's language, and brings in a person only when it cannot help. The people who take over work in a shared inbox that belongs to the same product.

Richie is the agent. The widget, the dashboard, and the inbox around it carry the same name, so this document says "the widget", "the dashboard", or "the inbox" when it means one of those parts and "Richie" when it means the agent.

This document is the single definition of the MVP. It records what the first shippable version does, what it deliberately leaves out, the domain model it is built on, and the demo script that decides whether it is done. The engineering plan is a separate document written from this one.

Decided on 2026-10-28. The product idea is borrowed from the MarshalDesk project. No implementation detail is borrowed from it.

## Richie comes first, people take over

Richie handles every conversation from its first message. A teammate is never the default responder. This is the rule every feature below follows.

- Richie answers from the knowledge base and nothing else. It refuses anything outside the business's domain.
- Richie hands a conversation to the team when it cannot find an answer, when its confidence is low, or when the visitor asks for a person.
- A teammate can take over any conversation Richie is handling, at any time, with one action.
- A teammate can hand a conversation back to Richie.
- A conversation that reopens goes to Richie first, not to the team.
- Everything Richie does is visible to the team. The team sees which sources it read, what it decided, and why.

## Who uses Richie

- A **visitor** is a person on the business's website. A visitor may be anonymous or identified by the website as a logged-in user. Visitors never create an account.
- A **teammate** is a person who works for the business. A teammate signs in to the dashboard. The teammate who creates a workspace is its **owner**. The owner invites other teammates. Every teammate shares the same inbox.
- **Richie** is the agent. It is a participant in every conversation, it has a display name and an avatar the owner can change, and the dashboard shows what it did and why.

A **workspace** is one business. Every piece of data belongs to exactly one workspace. A teammate can belong to more than one workspace.

## The core loop

The MVP exists to make this loop feel finished.

1. A visitor opens the widget and sends a message.
2. Richie reads the knowledge base and answers, in the visitor's language, within a few seconds.
3. The visitor keeps talking to Richie. Most conversations end here.
4. If Richie is not confident, or the visitor asks for a person, Richie hands the conversation to the team. It lands in the inbox under **Needs a human**.
5. A teammate sees it arrive without refreshing, takes it, and replies. The visitor sees the reply at once, even on a phone, even after a page reload.
6. The teammate closes the conversation or hands it back to Richie. A new message from the visitor reopens it, with Richie answering first.

Every feature in scope serves this loop. A feature that does not is out.

## In scope

### Richie

- Richie answers every new visitor message in a conversation it handles. It uses only the workspace's knowledge base. It answers in the visitor's language and streams the reply as it is written.
- Richie refuses requests outside the business's domain with a short polite message. A request for a poem or a math solution is refused, not answered.
- Richie hands off to the team when it cannot find an answer in the knowledge base, when its confidence is low, or when the visitor asks for a person. The handoff posts a short message to the visitor saying a person will reply.
- When the knowledge base has no Ready source, Richie hands off at once and records "no knowledge base" as the reason.
- Every Richie reply records a run. A run stores the retrieved sources, the decision (answered, handed off, or refused), the reason in plain words, the model, the latency, and the token counts.
- Richie never closes a conversation.
- The owner can test Richie from the dashboard with a question and see the same run details a real conversation produces.

### The knowledge base

- The owner adds a **text source** by typing or pasting text with a title.
- The owner adds a **file source** by uploading a PDF, Markdown, or plain text file. The size limit is 10 MB per file.
- Each source has a status of Processing, Ready, or Failed. Only Ready sources are used in answers. A Failed source shows the reason.
- The owner can delete a source. Richie stops using it at once.

### The widget

- The business pastes one script tag. The script loads the widget and shows a launcher button in the configured corner.
- On a phone-sized screen the open widget fills the screen. On a desktop screen it is a panel anchored to the launcher.
- The widget opens with Richie's name, avatar, and greeting, so the visitor knows they are talking to an agent.
- An anonymous visitor gets a conversation without any sign-up. The conversation survives a page reload and a browser restart on the same device.
- The visitor sees messages arrive live, sees a typing indicator while Richie or a teammate writes, and sees when their own message was read.
- When a person takes over, the widget shows the teammate's name and avatar on their messages, so the visitor can tell Richie and a person apart.
- The visitor can ask for a person at any time with one button.
- The widget renders in light and dark mode and respects the brand color from the settings.
- The widget is usable with a keyboard and a screen reader. Focus stays inside the open widget. Text contrast meets WCAG AA.
- Messages are plain text with line breaks and clickable links. No attachments in the MVP.

### Sign in and workspaces

- A teammate signs in with a passwordless email code or a passkey. There is no password.
- A new teammate creates a workspace during first sign-in. An invited teammate lands in the workspace that invited them.
- The owner invites teammates by email. The invite is a link that expires after 7 days. An invited teammate who signs in with that email becomes an active member.
- Roles are **Owner** and **Agent**. The owner can change Richie's settings, the knowledge base, and membership. An agent can work the inbox and read everything else.
- A teammate in more than one workspace switches between them from the dashboard.

### Identified users

- The business's website can tell the widget who the visitor is. It passes a user id, an email, and a name, plus an HMAC of the user id signed with the workspace's identity secret.
- The widget accepts the identity only when the HMAC verifies. A failed HMAC leaves the visitor anonymous and records the failure for the owner.
- When an anonymous visitor becomes identified on the same device, their existing conversations move to the identified contact.
- The owner can require identity verification, which makes the widget refuse unsigned identities.

### The inbox

- The inbox lists conversations in views named **Needs a human**, **Mine**, **Richie**, **All open**, and **Closed**. Each view shows its count. **Needs a human** is the queue of handed-off conversations nobody has taken yet.
- A conversation row shows the contact's name or a generated visitor name, the last message, the time, who handles it, and an unread marker.
- New conversations and new messages appear in the list and in the open conversation without a refresh.
- The browser tab title shows the number of conversations under **Needs a human**. A sound plays when Richie hands off, and the teammate can mute it.
- In a conversation a teammate can take over from Richie, reply, assign to themself or another teammate, unassign, hand back to Richie, close, and reopen.
- A teammate can leave an internal **note** on a conversation. The visitor never sees notes. Notes are styled apart from messages.
- The conversation sidebar shows the contact's details, the visitor context, Richie's runs for this conversation, and the contact's earlier conversations.
- A teammate sees a typing indicator while the visitor writes.

### Contacts and visitor context

- Every visitor is a **contact**. A contact is a **Visitor** until identified, then a **User**.
- The widget records for each contact the first and last seen time, the country and city from the IP address, the local time, the browser language, the device type, the browser, the operating system, and the current page URL.
- The contacts page lists contacts with search by name and email, and opens a contact's conversations.
- Deleting a contact deletes its conversations.

### Richie settings

- The owner sets Richie's display name, avatar, and greeting, the brand color, and the launcher corner. The display name is "Richie" by default.
- The owner lists the website origins allowed to load the widget. The widget refuses to load from any other origin. An empty list allows no origin, so the owner must add one before the widget works.
- The owner sees a live preview of the widget while editing.
- The owner copies the install snippet from the settings page.
- The owner views the identity secret, rotates it, and toggles whether identity verification is required.

### Dashboard

- The dashboard has light and dark mode and follows the system setting by default.
- The dashboard works at tablet width and above. Phone width is out of scope for the dashboard.

## Out of scope

Each item below is a product area in itself. The MVP does not include it, and the domain model does not need to change to add it later unless noted.

- Billing, plans, and usage limits.
- Email as a channel. No inbound email, no email replies, no email transcripts.
- A public help center or articles.
- Outbound messages such as campaigns, tours, or proactive chats.
- Canned replies and macros.
- Snoozing a conversation.
- Attachments, images, emoji pickers, and rich text in messages.
- Conversation tags, custom contact attributes, and segments.
- Reporting and analytics beyond the view counts.
- Integrations, webhooks, and a public API.
- More than one widget per workspace.
- Roles beyond Owner and Agent.
- A mobile app for teammates.
- Crawling a website URL as a knowledge source.
- Suggested questions generated from a source.
- Richie taking actions such as looking up an order or booking a slot. Richie only answers from text in the MVP.
- Dashboard localization. The dashboard is in English. Richie answers in the visitor's language.
- Visitor-side conversation history across devices.

## Domain model

The model below is the data shape the engineering plan starts from. Every entity carries a workspace id and every query is scoped by it.

### Workspace and membership

- **Workspace.** Id, name, slug, created at.
- **Teammate.** Id, which is the subject from the identity provider, name, email, avatar URL.
- **Membership.** Workspace, teammate, role of Owner or Agent, status of Invited or Active, invited at, joined at.
- **Invite.** Workspace, email, token, role, expires at, accepted at.

A workspace has exactly one Owner membership. Ownership transfer is out of scope.

### Contacts

- **Contact.** Workspace, kind of Visitor or User, external id, email, name, first seen, last seen.
- **VisitorContext.** Contact, country, city, time zone, language, device type, browser, operating system, last page URL, updated at.
- **Device.** Contact, opaque device token issued to the widget, created at, last seen.

A contact moves from Visitor to User once and never back. A device belongs to one contact. When a device's visitor is identified, the device and its conversations move to the User contact. Merging two User contacts is out of scope.

### Conversations

- **Conversation.** Workspace, contact, status, handler, created at, last message at, closed at, closed by, visitor read at.
- **Message.** Conversation, author, body, created at, delivered at.
- **Note.** Conversation, teammate, body, created at.

Status is **Open** or **Closed**.

Handler is one of **Richie**, **Team** (meaning handed off and not yet taken), or **Teammate** with a teammate id.

Author is one of **Contact**, **Teammate** with a teammate id, **Richie**, or **System**. System messages record events such as "Richie handed this to the team" and "Closed by Ana".

The conversation state machine is the rule the inbox enforces. Every conversation starts with Richie and every reopen returns to Richie.

| From | Event | To |
| --- | --- | --- |
| (new) | Visitor sends first message | Open, Richie |
| Open, Richie | Richie hands off | Open, Team |
| Open, Richie | Visitor asks for a person | Open, Team |
| Open, Richie | Teammate takes over or assigns anyone | Open, Teammate |
| Open, Team | Teammate takes or assigns anyone | Open, Teammate |
| Open, Teammate | Teammate unassigns | Open, Team |
| Open, Teammate | Teammate reassigns | Open, Teammate |
| Open, Team or Teammate | Teammate hands back to Richie | Open, Richie |
| Open, any | Teammate closes | Closed, same handler |
| Closed | Visitor sends a message | Open, Richie |
| Closed | Teammate reopens | Open, Richie |

Richie replies only when the handler is Richie. A visitor message in a Closed conversation reopens it to Richie. If the follow-up is one Richie already failed on, Richie's handoff rules send it to the team within one run.

### Knowledge base and runs

- **KnowledgeSource.** Workspace, kind of Text or File, title, status of Processing, Ready, or Failed, failure reason, file name, byte size, created at, ready at.
- **SourceChunk.** Source, ordinal, text, search index.
- **RichieRun.** Conversation, trigger message, decision of Answered, HandedOff, or Refused, reason, reply message, model id, latency in milliseconds, input tokens, output tokens, created at.
- **RichieRunSource.** Run, chunk, rank, excerpt.

A source is Processing from upload until its chunks are indexed. Deleting a source deletes its chunks. Runs keep their excerpts, so a run stays readable after its source is deleted.

### Richie settings

- **RichieSettings.** Workspace, display name, avatar URL, greeting, brand color, launcher corner of BottomRight or BottomLeft, allowed origins, identity secret, identity verification required, updated at.

One row per workspace, created with the workspace.

## Non-functional requirements

These are part of done. The demo script checks the ones it can.

- Richie streams its first token within 3 seconds for a knowledge base of up to 200 chunks.
- A message sent by a visitor or a teammate appears on the other side within 1 second on a local network.
- The loader script the business embeds is at most 5 KB compressed. The widget bundle loads only after the launcher is clicked or the widget is set to open on load.
- Every API query is scoped by the caller's workspace. A request for another workspace's conversation returns not found, never forbidden, so ids leak nothing.
- Widget endpoints are rate limited per device token and per IP address.
- The identity HMAC uses SHA-256 and the secret never leaves the server except on the settings page.
- The widget and the dashboard pass an automated accessibility check with no serious violations.
- The widget opens full screen at 375 pixels wide and as a panel at 1280 pixels wide.

## Definition of done

The MVP is done when a person can run the script below from a clean database, in order, and every step passes. Each step names what the person sees. The engineering plan turns these steps into automated end-to-end tests where it can, and the rest are run by hand before release.

1. Open the dashboard. Sign in with an email code. Create the workspace "Acme". You see an empty inbox and a banner saying the widget has no allowed origin yet.
2. Invite a second email as an Agent. Sign in as that email in a second browser. You see the Acme inbox with the Agent role and no settings menu.
3. As the owner, add a text source titled "Shipping" with a few paragraphs of shipping policy. Upload a PDF. Both sources show Processing, then Ready, within 30 seconds.
4. Set Richie's display name, greeting, the brand color, and the launcher corner. The preview updates as you type. Add the demo site's origin to the allowed list. Copy the snippet.
5. Open the demo site in a phone-sized window. The launcher shows in the configured corner with the brand color. Open the widget. It fills the screen and shows Richie's name, avatar, and greeting.
6. Ask a shipping question in Georgian. Richie streams an answer in Georgian within 3 seconds. The answer matches the Shipping source. In the dashboard the conversation shows under **Richie**.
7. Ask for a poem. Richie refuses in Georgian and offers to help with the business's topics.
8. Ask a question the sources do not cover. Richie hands off. The widget shows a system message that a person will reply. In the dashboard the conversation appears under **Needs a human** within 1 second, the tab title shows "(1)", and the sound plays.
9. As the agent teammate, take the conversation and reply. The visitor sees the typing indicator, then the reply with the teammate's name, within 1 second. Reload the demo site. The conversation and all messages are still there.
10. Add a note. The visitor's widget does not show it. Hand the conversation back to Richie. Ask another shipping question as the visitor. Richie answers.
11. Close the conversation. It moves to **Closed**. Send a shipping question as the visitor. The conversation reopens under **Richie** and Richie answers.
12. Open a second conversation as a new visitor and let Richie answer it. From the inbox, take it over while Richie is handling it. Richie stops replying and your reply reaches the visitor.
13. Open Richie's runs in the conversation sidebar. Each run shows the decision, the reason, the sources with excerpts, the model, and the latency.
14. On the demo site, sign in as the demo user "Nino". The widget now identifies the contact. In the contacts page Nino is a User with name and email, and her earlier anonymous conversation is attached to her.
15. Tamper with the HMAC in the demo site's snippet. The widget treats the visitor as anonymous and the settings page shows a verification failure count.
16. Load the snippet from an origin not in the allowed list. The widget does not render and the console shows the refusal.
17. Switch the dashboard to dark mode. Switch the system theme. Both the dashboard and the widget follow.
18. Run the accessibility check on the open widget and on the inbox. No serious violations.

## Technical constraints the engineering plan inherits

These are decided. The engineering plan does not reopen them.

- Backend on .NET 10 with the Aspire 13.6 AppHost that already exists in this repository. PostgreSQL for all data, Redis for caching and real-time backplane, RabbitMQ for background work such as source processing, Azure Key Vault for secrets. Warnings are errors and SonarAnalyzer runs on every build.
- Domain events through the SharedKernel project that already exists.
- Teammate sign-in through Duende IdentityServer with Duende User Management, passwordless with email codes and passkeys, and the dashboard behind a Duende BFF.
- Dashboard in React with TypeScript.
- The widget is a separate bundle from the dashboard, loaded by a small loader script, so the dashboard's size never affects the business's website.
- Real-time delivery over SignalR.
- Richie calls Claude through the official Anthropic .NET SDK. The default model is `claude-opus-5-5`. The owner of this repository may lower it to `claude-sonnet-5-5` for cost after measuring answer quality.
- A demo website lives in this repository so the widget can be exercised without a third-party site.

These are open and the engineering plan settles them.

- Retrieval for the knowledge base. The default is PostgreSQL full-text search over chunks. Vector embeddings are the upgrade if measured answer quality is not good enough.
- The widget's rendering library, chosen by bundle size.
- Whether source processing runs in the API process or a worker, and what RabbitMQ carries in the MVP.
- How the conversation list paginates and how far back it loads.

## Milestones

The milestones are ordered so that each one is demoable on its own and the product's identity lands first. Each milestone names the steps of the definition of done it makes pass.

1. **Richie in the widget.** Workspace with one hard-coded owner, the knowledge base with text and file sources, the widget with an anonymous visitor, conversation persistence, Richie answering with streaming, language matching, refusal, and runs with sources. Handoff posts the system message but there is no inbox yet. Passes steps 3, 5, 6, 7, and 13. This lands first because the streaming answer and widget persistence are both the product's identity and the riskiest unknowns.
2. **People take over.** The inbox with its views, real-time delivery both ways, take over, assign, hand back, close, reopen, notes, typing indicators, tab title and sound. Passes steps 8, 9, 10, 11, and 12.
3. **Sign-in and team.** Identity server, passwordless sign-in, workspace creation, invites, roles, workspace switching. Passes steps 1 and 2.
4. **Contacts and identity.** Visitor context, contacts page, HMAC identification, device merge, verification failures. Passes steps 14 and 15.
5. **Settings and polish.** Richie settings page with preview, allowed origins, install snippet, dark mode, accessibility pass, bundle budget. Passes steps 4, 16, 17, and 18.

## Appendix A. Decisions and why

**Richie comes first and people take over.** The owner decided this on 2026-10-28. The product is the agent. A human-only chat tool with an AI added later would have put the inbox at the center and made Richie a feature. Putting Richie first shapes the conversation model. Every conversation starts with Richie, every reopen returns to Richie, and the inbox is the place people go when Richie asks for them.

**Workspaces are teams, not single owners.** Intercom's inbox is shared with assignment. MarshalDesk stops at one owner per workspace. Richie's domain model carries memberships and handlers from day one so the inbox never has to be rebuilt for teams.

**Sign-in is Duende IdentityServer with User Management.** Earlier work on this project built passwordless sign-in with email codes and passkeys on Duende, and the AppHost already names an identity database. Reusing it costs less than building ASP.NET Core Identity and gives the dashboard a BFF with no tokens in the browser.

**Identified users are in the MVP.** Without identification every contact is an anonymous visitor and the contacts page is a list of browser tokens. The HMAC scheme is small and it turns contacts into people with names, which is what the inbox sidebar needs to be useful.

**Help center, email channel, and billing are out.** Each is its own product area with its own data. None of them is needed to make the core loop feel finished.

**Retrieval defaults to full-text search.** Full-text search needs no second vendor and no vector extension. For a knowledge base of a few hundred chunks it finds the right source most of the time. Richie's runs record the retrieved sources, so answer quality can be measured before deciding on embeddings.

**A reopened conversation goes to Richie, not to the team.** Richie is the default responder. If the follow-up is one Richie cannot answer, its handoff rules send it to the team within one run, so the visitor loses a few seconds at most. Sending every reopen to the team would make the team the default for returning visitors, which contradicts the first decision.

## Appendix B. Assumptions the owner can overturn

Each line is a default chosen without asking. Change the line and the scope follows.

- Invites expire after 7 days.
- Knowledge files are PDF, Markdown, and plain text, at most 10 MB each.
- With no Ready source, Richie hands off at once rather than answering from general knowledge.
- Richie's display name is "Richie" by default and the owner can change it.
- Visitor names for anonymous contacts are generated, for example "Amber Fox", and stay stable per contact.
- The sound on handoff is on by default and can be muted per teammate.
- The dashboard is English only.
- One widget per workspace, one allowed-origin list per workspace.
- The default model is `claude-opus-5-5`.
- The dashboard supports tablet width and above.
