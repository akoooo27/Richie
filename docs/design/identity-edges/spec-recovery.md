# Richie identity edges: Part D, account recovery (WP3.4)

**Revision 2, 2026-11-05** (Phase 4 fix round: every change is in the fix log, §14; §15 adds the support handoff, the Help article, time limits, tokens, completion failures and the settings delta; the Phase 5 revision round is written into the sections and listed at the end). For the PM who assembles the canvas, the identity and support engineers, the testers and the evaluator. Extends Part A ([`spec.md`](spec.md), binding) and never repeats it; where this file departs from Part A, §1 says so and why.

**Precedence (one order for all five files, ruling B-37):** Part B rulings in `spec.md`, then the domain specs’ page tables and recorded departures, then Part A (Revision 2 synced it to both). A board that disagrees with its spec is a bug in the board.

**What this covers.** Someone can’t get email at the address on their Richie account. Recovery is **passkey first, then support** (Part A D2): with a passkey or a linked Google or Microsoft account they sign in with it and change their email in settings (AS §4.2). Without one, they send an **in-product recovery request** from signed-out pages. Richie support reviews it (first reply promised within 2 business days, P-5), the old address is told at once and can cancel, the email changes **72 hours after approval** (P-5), and the account is then *recently recovered*: every device is signed out, step-up runs for every sensitive action for 7 days, and Sign-in and security says what happened.

**Pages:** R1 (Can’t get the code?), R2–R5 (the three-step request), R6–R10 (the request’s status, including R6a the requester’s cancel confirm, R6b the owner’s view and R10c the expired request), R11 and R15 (emails), R12–R13 (the old address cancels), R14 (settings after recovery). **Boards:** 23 (§10). **Sample data:** Part A §4.12; today is Thursday, November 5, 2026; R8 and R14 are drawn on November 8.

Read with: Part A §3.1 (R rows), §3.3–3.5, §4, §8, §9; AS §4.0, §4.2, §4.4 (step-up, screen C), §4.8 (solo pages, email table format); how-we-design §3–4.

---

## Contents

1. [Decisions and departures](#1-decisions-and-departures)
2. [The journey end to end](#2-the-journey-end-to-end)
3. [Pages R1 to R14](#3-pages-r1-to-r14)
4. [Emails (R11, R15)](#4-emails-r11-r15)
5. [What support sees and decides](#5-what-support-sees-and-decides)
6. [State matrix](#6-state-matrix)
7. [Handoff to the identity canvas, and links other WPs use](#7-handoff-to-the-identity-canvas-and-links-other-wps-use)
8. [CSS (WP3.4 section of `edges.css`)](#8-css-wp34-section-of-edgescss)
9. [Engineering handoff](#9-engineering-handoff)
10. [Boards](#10-boards)
11. [Open questions, with defaults](#11-open-questions-with-defaults)
12. [Testing](#12-testing)
13. [Changelog](#13-changelog)

---

## 1. Decisions and departures

### 1.1 Part A §8 defaults written into copy

| Part A | Default used | Where it shows |
|---|---|---|
| P-5 / Q8 hold | 72 hours from **approval**; the time is computed on the server (`changes_at = approved_at + 72 h`, UTC) and shown as a date and a time, never as "72 hours from now" | R7 `h1` and facts, approved emails |
| P-5 / Q9 reply | "usually replies within 2 business days" | R1, R2 note, R6 lede, received email |
| Q11 one open request | Refused at R2 with a field error; it says where the status went without showing that address | R2 |
| Q12 wait after a decline | 7 days, shown as a date ("Start a new request on November 12"); support can lift it | R9, R2 |
| Q13 undo | Support can undo a completed recovery for 30 days ("tell us by December 8"), the same promise as E5’s owner variant (spec-edge §E5) and S6 (spec-settings §3.4) | Completed email to the old address, R12 "already changed" |
| Q14 completion | Every device signed out and refresh tokens revoked; passkeys, linked accounts and connected apps kept, which R14 asks the person to check | R8, R14, completed emails |
| Q24 deletion scheduled | Recovery allowed; `Deletion-pending` shows at the next sign-in | §9 |

### 1.2 Departures from Part A (each with its reason)

| # | Part A says | This spec | Why |
|---|---|---|---|
| D-R1 | R3 and R5: "same title" as R2 (Recover your account) | Each step has its own `h1` and document title: R3 **Choose a new email**, R5 **Tell us about your account** | Each step is its own page and URL with its own question. A distinct `h1` (and tab title) tells someone who lands on the heading, or returns to the tab, what this page asks (WCAG 2.4.2, 2.4.6). suiss `signup-steps` also titles each step |
| D-R2 | R1 has one `h1`, "Can’t get the code?" | From passkey sign-in (A5) R1 reads **Can’t use your passkey?** and its primary is **Get a code by email instead** (copy-only variant, §3.1) | The A5 link says "Can’t use your passkey?"; landing on "Can’t get the code?" when no code was sent reads as the wrong page |
| D-R3 | §4.8: the account chip on recovery pages | No chip on R6–R12. The account and new address are labelled fact rows | A chip shows the profile’s initials to whoever holds a status or cancel link: more than Sign in reveals (principle 5). A labelled row also says which address is which |
| D-R4 | §4.2 phrases "send your request", "cancel your request", "cancel the request" | Visible R5 **send it**, R6 confirm and R12 **cancel it**, with the noun for assistive tech: `We couldn’t send <span aria-hidden="true">it</span><span class="ra-sr">your request</span> because you’re offline.` (requester "your request", owner "the request"). R2 keeps **continue** | The visible noun phrases measure 2 lines at 358 in SF (§12.3) and §4.6 forbids a taller slot; the line sits under the button it reports on, and a screen reader or braille gets the full phrase (A11Y-07, COPY-10). "We couldn’t check that email" also measures 2 lines; "continue" stands on its own |
| D-R5 | §3.3 rule 5: R8 goes to A1 "with an email **or** `returnUrl=/account/security`" | R8 **Sign in** carries both: the new email as `login_hint` and `returnUrl=/account/security` | After recovery the first job is R14: check passkeys and devices, add a passkey. Landing there closes the loop; the email saves retyping |
| D-R6 | §4.9: R10 has the exclamation badge | R10b ("You canceled your request") has the **check** badge; R10a (the old address canceled it) keeps the exclamation | §4.9’s meanings: canceling your own request is "something you wanted is done" |
| D-R7 | §3.1 R15: declined goes to the new address | Also to the **old** address; one more email when **support undoes** a recovery (§4) | The old address got an alarming email at submit; closing that loop ("Nothing changed") stops a needless support contact. An undo changes the account’s email, which always notifies (AS §4.0 security emails) |
| D-R8 | Q12 covers declines | The same 7-day wait follows a cancel **by the old address** | The owner canceling is a stronger "not you" than a decline; without the wait a persistent requester re-mails the owner every day |
| D-R9 | P-6: `Security-recovered` is a full copy of `Security` with the alert and the entry | Same copy, plus the data a recovered account would have: the new address in the email row and the sidebar (own sidebar copy, AS §6.1), **one** passkey (the lost Pixel 8), Google **not linked**, one device, four activity rows on November 8 | With the base’s MacBook passkey and linked Google, recovery would never have been needed (J6, J7): the board would contradict J8. `drift.mjs` reports the sidebar copy; that drift is intended |
| D-R10 | R12 **Keep the request** → R6 | → **R6b**, R6 in the owner’s words and with the new address masked (`Recover-owner-view`; "Cancel this request" goes back to R12) | R6 shows the new address in full to the requester; the old mailbox must not learn more through Keep than through the email |
| D-R11 | Q11: say "its status went to the new address it named" | Said without showing that address, even masked | The masked domain would tell anyone who types an email where an open request points |
| D-R12 | §3.1 R11: the old address’s email button | **Cancel this request**, with "If this wasn’t you, cancel the request." above it; the completed email (#6) uses **Contact support** | B-10. "This wasn’t me" is used only by settings S6, where it goes to support |
| D-R13 | A6: "Can’t get the code?" on the Verify family | Not on the **sign-up** Verify | R1 offers ways into an existing account; on sign-up there is none yet. Sign-up Verify keeps "Check spam, or use a different email" |
| D-R14 | §4.2: "reach Google / reach Microsoft" | B-6: R1 says **"We couldn’t sign you in…"** for the passkey, Google and Microsoft buttons alike; for Google and Microsoft only the offline pattern occurs | Before the redirect the only client-side failure is being offline; the redirect is a navigation, so a network or server failure is a browser error or E4, and a provider failure comes back as P3 on Sign in (WP3.5) |
| D-R15 | Board sizes in §5.2 | `Recover` and `Recover-dark` 1100, `Recover-details` 1400, `Recover-limited` and `Recover-limited-dark` 1300, `Email-recovery-alert` 720 × 1100, `Security-recovered` 1800; new `Verify-help` 1440 × 900; rows "request", "status" and "owner" move (CANVAS-CHANGES WP3.4-1 to -10) | Measured heights rounded up (§12.4, §14) |
| D-R16 | B-18 names R1’s provider route | All three R1 methods (passkey, Google, Microsoft) sign the person in to `/account/profile#email` with Change email open | One rule for one page: whoever lands on R1 most likely needs a new email, and any sign-in starts the 10-minute step-up window. Cancel in the dialog leaves them on Profile |

### 1.3 Rules kept (Part A §7.3)

- R1 reveals nothing: every visitor sees every way in, whatever the account has.
- R2 reveals no more than Sign in (plus that an open request or a wait exists, §3.2 "Reveals").
- Status pages are token links that show one request and expire (§2.4).
- The wait is a date and a time; completing signs out every device; the old address can cancel until the change happens.

---

## 2. The journey end to end

### 2.1 Map

```
Verify ──"Can’t get the code?"──┐            Passkey sign-in ──"Can’t use your passkey?"──┐
Help article ───────────────────┤                                                          │
                                ▼                                                          ▼
                    R1  Can’t get the code?  (every way in, for everyone)
     ┌──────────────┬──────────────┬───────────────┬───────────────────────────────┐
 Back to the code  Passkey       Google         Microsoft            Start a recovery request
 (Verify, as left) → signed in → Profile → Change email (AS §4.2, B-18)          │
                                                                                   ▼
                    R2 Step 1 of 3  Recover your account (the email on the account)
                    R3 Step 2 of 3  Choose a new email ──Send code──▶ R4 Check your new email
                    R5 Step 3 of 3  Tell us about your account ──Send request──┐
                                                                               ▼
     emails: "received" → new address, "alert" (R11) → old address     R6 We’re reviewing your request
                                                                               │
          ┌────────────── support decides (§5) ──────────────┐                 │
          ▼                                                  ▼                 ▼  7 days, no decision (B-36)
   R9 We couldn’t approve (decline)                 R7 Your email changes   R10c We couldn’t finish your
   emails → new, old                                   on November 8             request in time
                                                                            emails → new, old; no wait
                                                    emails → new, old (with the date and time)
                                                             │  72 h, server time
                                                             ▼
                                                    R8 Your email was changed
                                                    every device signed out; emails → new, old
                                                             ▼
                                                    Sign in (new email) → R14 Sign-in and security

 Old address, any time before the change:  R11 email ─"Cancel this request"─▶ R12 Cancel this recovery request?
                                           ─"Cancel the request"─▶ R13 We canceled the request (requester sees R10a)
                                           ─"Keep the request"─▶ R6b owner’s view (new address masked)
 Old address after the change (30 days):  completed email ─"Contact support"─▶ support (undo); R12 "already changed" the same
 Requester, any time before the change:   R6/R7 "Cancel request" ─▶ R6a Cancel your request? ─▶ R10b
```

### 2.2 With a passkey or a linked provider (J6, J7)

| Step | What happens | Way out |
|---|---|---|
| 1 | Verify → **Can’t get the code?** → R1 | Back to the code |
| 2 | R1 **Sign in with a passkey** (discoverable credential; a passkey on a phone works through the browser’s cross-device prompt, which the R1 note mentions) or **Sign in with Google / Microsoft** (P1 → provider → callback) | The other buttons; Back to the code |
| 3 | Signed in, to `/account/profile#email` with Change email open (B-18, D-R16) | — |
| 4 | **Change email** → new address + code → done. The sign-in in step 2 started the 10-minute step-up window (B-18), so no step-up is asked inside it; after it, step-up is a passkey or an email code (AS §4.4), and a provider-only person signs in with the provider again | AS §4.2 Cancel |

No recovery request, no wait, no support. This is the route R1 puts first, above the recovery block, for everyone.

### 2.3 Without one (J8), and every branch

| # | Moment | Page | Emails | State on the server |
|---|---|---|---|---|
| 1 | Start | R1 → R2 (email carried from the sign-in request, server-side) | — | Draft created (30 min idle life) |
| 2 | Which account | R2 → R3 | — | Draft holds the account; checks: account exists, no open request, no wait, L2, L5 per IP |
| 3 | New address | R3 → R4 | Code to the new address | Checks: format, not the account’s email, no other account uses it, L1, L5 per address |
| 4 | Code | R4 → R5 | — | New address confirmed (30 min) |
| 5 | Details | R5 → R6 | **Received** → new; **Alert (R11)** → old | Request `open`; status and cancel tokens issued |
| 6a | Support asks for more | R6 unchanged | Support’s own email thread with the new address (Reply-To support) | `open` |
| 6b | Support approves (Nov 5, 4:10 PM) | R7 | **Approved** → new and old, with the date and time | `approved`, `changes_at` Nov 8, 4:10 PM |
| 6c | Support declines | R9 | **Declined** → new and old | `declined`; new requests for the account wait 7 days |
| 6d | Old address cancels (any time before the change) | R12 → R13; requester sees R10a | **Canceled** → new and old | `canceled_owner`; 7-day wait |
| 6e | Requester cancels | R6a → R10b | **Canceled** → new and old | `canceled_requester`; no wait |
| 6f | No decision 7 days after submit (B-36) | R10c | **Expired** (#16) → new; (#17) → old | `expired`; no wait: a new request is allowed at once |
| 7 | `changes_at` passes | R7 reloads to R8 | **Completed** → new and old | Email changed; `recently_recovered_until` Nov 15, 4:10 PM; every session and refresh token revoked; activity entry |
| 8 | Sign in | A1 → Verify (code to the new address) → R14 | — | Step-up for every sensitive action until Nov 15 |
| 9 | Owner missed it (J10) | Completed email (#6) **Contact support**, or R12 "already changed" **Contact support** → support form (topic `recovery-not-me`, the dispute token) | — | Support can undo until Dec 8, 4:10 PM (**Undone** emails) |

**Dead ends walked.** Every page has Back, Sign in, Help or Contact support. The ones that needed closing: an owner who opens the cancel link after the change (R12 "already changed" → support, not E5); a requester declined or canceled by the owner (R9 and R10a: Contact support, and a date for a new request); a request nobody decided in 7 days (R10c: Start a new request at once, or Contact support); a requester whose network hit the limit (R5 limited: Contact support); a used cancel link (R13 again, per spec-edge QE-5); a dead status link (E5 → Start a recovery request).

### 2.4 Lifetimes and limits

| Thing | Lifetime or limit | After it |
|---|---|---|
| Draft (R2–R5), server-side, keyed to a cookie | 30 minutes since the last step | R2 with an alert: "Start again" (§3.2) |
| Code (R4) | 5 minutes, 4 tries, 1 send per 60 s, 5 per hour per address (L1, L3, L4) | AS §4.4 expired and locked states |
| New-address confirmation | 30 minutes after R4 | R5 submit → R3 with an alert (§3.5) |
| Open request (B-36) | At most 7 days from submit while support hasn’t approved or declined it; an approved request doesn’t expire (its 72-hour hold runs as specified) | Closed as `expired`: R10c on the status link, emails #16 and #17, the reserved new address released, no wait for a new request |
| Status link (every email to the new address) | From submit until 30 days after the request closes (completed, declined, canceled or expired) | E5 (recovery variant: Start a recovery request) |
| Cancel link (R11 and the approved email to the old address) | Same as the status link | While open: R12 (and R6b through Keep the request). After a cancel: R13 again. After a decline, an expiry or a requester cancel: R13 "already closed". After completion, for 30 days: R12 "already changed". Later: E5 (owner variant) |
| **Contact support** in the completed email (#6, the dispute token) | 30 days after completion | E5 (owner variant) |
| Hold | 72 hours from approval | R8 |
| Recently recovered | 7 days from completion | R14 alert and step-up note disappear |
| Undo by support | 30 days from completion | — |
| New request after a decline or an owner cancel | 7 days (support can lift it); none after an expiry or a requester cancel | R2 accepts it |
| L5 | 1 open request per account; 3 requests per new address per 24 h; 5 per IP per 24 h | R2, R3, R5 (§3) |
| L7 | 30 token lookups (status, cancel) per IP per hour | E10 |

Tokens are 32 random bytes, single purpose (status, cancel, dispute), stored hashed, compared in constant time; an expired, used or wrong token gives the same E5 page and the same 410 (spec-edge §E5).

### 2.5 Time

Every date and time on a page comes from the server (UTC) and is shown in the browser’s time zone with the AS §4.0 date rules ("November 8 at 4:10 PM"; "Today at 4:10 PM" on the day). Emails can’t know the reader’s zone: they use the account’s time zone setting (Preferences, Pacific Time for Jamie) and name it ("November 8 at 4:10 PM Pacific Time"); without one, UTC ("12:10 AM UTC"). The 72 hours are exact; across a daylight-saving change the wall-clock time shown shifts by an hour, which is correct.

R7 never counts down. The page receives `changes_at` and the server’s `now`, and schedules one reload at `changes_at` + 5 s using the offset between them (not the device clock), so an open tab turns into R8 by itself. Nothing on the page is live in the meantime.

---

## 3. Pages R1 to R14

Every page uses Part A’s frame (§4.1), badges (§4.9), slot (§4.6), focus (§4.7) and long-email markup (§4.8). Busy buttons are AS §4.0’s (spinner, gerund, `aria-disabled`). Failure lines are the AS §4.0 three patterns with the phrase named per page. "Slot" is `.ri-slot` under the primary: polite `p[role=status]` and `p[role=alert]` in one cell.

Common to every form page (R2, R3, R5): going offline while idle puts "You’re offline. We’ll keep what you typed." in the slot (polite, once); back online, "You’re back online." for 5 s (Part A §4.4). A field error after submit: focus the first invalid field, the error has no `role` and is read through `aria-describedby`, and the document title gets "Error: ". Values typed on R2, R3 and R5 stay in `sessionStorage` under the draft’s key for 30 minutes, so Back, a reload or an expiry alert never empties a field.

### 3.1 R1 Can’t get the code? (`Recover`, `Recover-mobile`, `Recover-dark`)

**Route** `/signin/help?returnUrl=…` (variant `?from=passkey`) · **Title** "Can’t get the code? – Richie" (variant "Can’t use your passkey? – Richie") · **Reveals** nothing: the same buttons for every visitor and every account · **Limit** L6 on the provider buttons (WP3.5’s countdown on the method buttons) · **Focus** the `h1` (no field on the page) · **Badge** none

| Element | Copy | Notes |
|---|---|---|
| `h1` | Can’t get the code? | |
| Lede | Check your spam or junk folder, or wait a minute and send a new code. | 2 lines at 390 |
| Primary (default, `lg` block) | Back to the code | Back to the code page that opened R1, as it was left (code, tries and countdown are server-side): Verify (`/signin/code?returnUrl=…`), or P6 for `from=link`. If the sign-in request expired, that page shows E1. On this canvas: `Verify-help` (COV-12) |
| Slot | (empty) | Reserved 20 / 40 |
| Divider | or | `.ri-or` |
| Method 1 (outline) | Sign in with a passkey | Discoverable WebAuthn request (no `allowCredentials`), so it works for any account. Hidden when the browser has no WebAuthn, with the note below. All three methods return to `/account/profile#email` with Change email open (B-18, D-R16) |
| Method 2 | Sign in with Google | As A1 (P1 → provider), same `returnUrl` |
| Method 3 | Sign in with Microsoft | As A1 |
| Note | Sign in any of these ways and we’ll open Change email for you. Passkey on your phone? Choose it when your browser asks. | B-18; Part A §3.4 gap |
| `h2` | Lost access to your email? | 18/600, centred, after a hairline |
| Text | We can change your account’s email to one you can get, after checking it’s you. Richie support usually replies within 2 business days. | 16 Slate; the reply sentence is identical everywhere (COPY-12) |
| Last action (outline) | Start a recovery request | → R2, carrying the sign-in’s email server-side (never in the URL) |
| Note under it | Questions? Contact support. | Topic `cant-sign-in` (§15.1), new tab (COV-06) |

**States**

| State | What changes | Layout |
|---|---|---|
| Default, from Verify | as above | **drawn** `Recover`, `-mobile`, `-dark` |
| From P6, linking Microsoft (`from=link`, COV-03) | `h1` and lede as default; **Back to the code** → `/signin/external/link/code` (P6 as left); **Sign in with Microsoft** is hidden (it would loop back to P5); passkey and Google stay | copy, `Recover` |
| From passkey sign-in (A5) | `h1` **Can’t use your passkey?**; lede "Try a passkey on another device, or sign in another way."; primary **Get a code by email instead** (sends a code to the remembered account’s email → Verify; busy "Sending code…"); everything else unchanged | copy, `Recover` |
| No sign-in in progress (Help article, typed URL) | Primary **Back to sign in** (→ A1) | copy, `Recover` |
| Passkey waiting | Method 1 busy "Waiting for your device…"; slot (polite) "Follow the prompt on your screen to finish." (B-28: announced once through the slot) | copy; busy pattern AS `Security-passkey-waiting` |
| Passkey canceled, timed out or none found | Slot (alert) "The request timed out or you canceled it."; the note becomes "No passkey for Richie on this device? Choose your phone when your browser asks, or use another way." (COV-31: the browser can’t tell us which it was) | copy |
| Passkey failure | Slot (alert) "We couldn’t sign you in because you’re offline." / "We couldn’t sign you in. Check your connection." / "We couldn’t sign you in. Try again in a moment." (B-6) | copy |
| Passkey for an account with no sign-in allowed (deleted, pending deletion) | As A1: `Signin-deleted` or `Deletion-pending` | WP3.5 / AS |
| Google or Microsoft busy | The button shows the spinner and "Taking you to Google…" (`aria-disabled`), also written once into the slot’s polite line (sr-only, B-28); the others stay enabled | WP3.5 `Signin-redirecting` |
| Provider, offline | Slot (alert) "We couldn’t sign you in because you’re offline." (B-6, D-R14) | copy |
| Provider canceled or failed | Comes back to **A1** with WP3.5’s P2 / P3 alert (the callback has no R1 variant) | WP3.5 |
| L6 reached | The method buttons count down as on A1 | WP3.5 |
| No WebAuthn | Method 1 and the note are hidden | copy |
| Dark, mobile, 320, short | — | `Recover-dark`, `Recover-mobile`; harness |

**Rationale.** R1 is Sign in with the email form taken away (Part A §1.2): the same column, the same three method buttons in the same order, so the person recognises the ways in they already have. The filled primary is the most likely fix (the code is in spam, or a new one will come); recovery sits last, under a hairline and its own heading, because it is slow and needs support, and it must not look like a fourth way to sign in. Showing every button to everyone is what keeps the page from revealing which methods an account has.

### 3.2 R2 Step 1 of 3: which account (`Recover-account`)

**Route** `/recover` (`?from=settings`, §15.6) · **Title** "Recover your account, step 1 of 3 – Richie" (A11Y-09) · **Reveals** whether an email has an account (as A1), and whether that account has an open request or a waiting period (accepted, Q11; needed so nobody waits on a request that can’t exist) · **Limit** L2 (shared with A1, 30 per IP per hour), L5 per IP · **Focus** the field on a fine pointer; nothing on phones

| Element | Copy | Notes |
|---|---|---|
| Step line | Step 1 of 3 | `.ri-step` |
| `h1` | Recover your account | |
| Lede | Enter the email address you can’t get anymore. | COPY-23 |
| Field | Label **Email**; prefilled from the sign-in (drawn: jamie.rivera@northwind.coffee); placeholder name@example.com | `type=email`, `autocomplete=username`, `inputmode=email`, `autocapitalize=none`, `spellcheck=false`, `enterkeyhint=next` |
| Primary | Continue | busy "Checking…" (B-28: also into the slot’s polite line). Canvas: the form goes to `Recover-email` (COV-22) |
| Slot | (empty) | |
| Note | Richie support usually replies within 2 business days. If we approve your request, your email changes 3 days later. | Sets the whole wait before any effort (COPY-12) |
| Switch line | Back to sign in | → A1 (no return URL: Part A §3.3 rule 5) |

| State | Copy | Where |
|---|---|---|
| Empty | Enter your email address. | Field error |
| Not an address | Enter a full email address, like name@example.com. | Field error |
| No account (or deleted more than 60 days ago) | No Richie account uses this email. Check it for typos, or contact support. | Field error (as A1); "contact support" opens the form, topic `cant-sign-in` (COV-06) |
| Open request (Q11) | This account already has a recovery request, open for up to 7 days. Yours? Use the link in your latest status email, or contact support to get it again. Not yours? Contact support and we’ll look at both. | Field error (D-R11). "contact support to get it again" opens the form with topic `recovery-request`; "Contact support and we’ll look at both" with topic `recovery-not-me` (§15.1). The "Yours?" line gives a requester whose status link died (E5) a way back to their request. Support can decline the open request and let this one through (§5.1, COV-05). A request nobody decides in 7 days expires (B-36), and R2 then accepts a new one at once |
| Waiting after a decline or an owner cancel | This account can’t take a new request until November 12. Contact support if you need one sooner. | Field error; "Contact support" is a link (new tab, topic `recovery-request`) |
| Account deleted, days 30–60 | This Richie account was deleted, so it can’t be recovered. You can still download your data. | Field error; "download your data" → `Export-start` with the address. Reveals what `Signin-deleted` reveals in the same window, without a date (COV-07, COV-38). Days 0–30 are allowed (Q24); after day 60 it is the "No account" line |
| L2 (shared with A1) | Primary **Continue in 41:12** (unavailable, m:ss from `Retry-After`; accessible name "Continue, available at 11:24 AM" with the digits `aria-hidden`, B-27); slot (polite, quiet, once) "This network has made 30 attempts in an hour." (B-34; E9’s L2 line, one bucket, one string). B-34 puts the countdown after the line, but "This network has made 30 attempts in an hour. Try again in 41:12." measures 2 lines at 358 in Inter and SF (spec-edge §7), which breaks Part A §4.6, so the slot carries the line alone and the countdown is the commit’s, directly above it (flagged for the PM); note "Can’t wait until then? Contact support." (COV-06); at 0:00 **Continue** and "You can try again now." | E9 pattern (`Signin-limited`) |
| L5 per IP (checked here first) | Primary **Continue at 9:15 AM** (sr ", tomorrow"); slot "This network has made 5 recovery requests today." (B-12); note "Can’t wait until tomorrow at 9:15 AM? Contact support." | E9 pattern |
| Offline / network / server | We couldn’t continue because you’re offline. / We couldn’t continue. Check your connection. / We couldn’t continue. Try again in a moment. | Slot (alert), focus stays on Continue |
| Time limit warning (B-29) | 2 minutes before the draft runs out: "Still there? This page times out in 2:00." with **Keep going** | spec-edge §3.6, drawn once on `Signin-timeout-warning` (§15.3) |
| Draft expired (arriving back here) | Alert at first paint, title **This took too long**, description "After 30 minutes without activity we clear what you typed, except your email. Start again from here." Focus the alert container (`role="group"`, `tabindex="-1"`, `aria-labelledby` its title, `aria-describedby` its description, no ring; B-38), so the next step is read with the title | Part A §4.10 pattern (`Signin-provider-canceled`), COPY-16 |
| From settings (B-25) | Field prefilled with the account’s email; switch line **Back to Sign-in and security** (→ `/account/security`) | copy, `Recover-account` |

**Rationale.** Step 1 asks one thing we can check at once, so a typo or a closed door shows before the person invests in steps 2 and 3. The note states the whole wait (2 business days, then 3 days) at the start, where it can still change the person’s mind towards a passkey on another device.

### 3.3 R3 Step 2 of 3: a new email (`Recover-email`)

**Route** `/recover/email` · **Title** "Choose a new email, step 2 of 3 – Richie" (D-R1, A11Y-09) · **Reveals** whether the new address already has an account (as AS §9 Q4) · **Limit** L1 for the new address, L5 per new address · **Focus** the field (fine pointer)

| Element | Copy | Notes |
|---|---|---|
| Step line | Step 2 of 3 | |
| `h1` | Choose a new email | |
| Lede | Use an address you can get now. We’ll send it a code to check it’s yours. | |
| Field | Label **New email**; empty; placeholder name@example.com | `type=email`, `autocomplete=email`, `enterkeyhint=send` |
| Primary | Send code | busy "Sending code…" |
| Note | We’ll send updates about your request here. If we approve it, this becomes your Richie email. | |
| Switch line | Back | → R2, values kept |

| State | Copy | Where |
|---|---|---|
| Empty | Enter your new email address. | Field error |
| Not an address | Enter a full email address, like name@example.com. | Field error |
| Same as the account | That’s the email you can’t get. Enter a different one. | Field error |
| Another account uses it | That email already belongs to another Richie account. Use a different one. | Field error (AS §4.2) |
| L5 per new address | This email has been used for 3 recovery requests today. Use a different one, or try again tomorrow at 9:15 AM. | Field error (the B-12 sentence; the person can change the address, so the button stays) |
| L1 | **Send code in 41:12** (accessible name "Send code, available at 11:24 AM", B-27); slot (polite, quiet, once) "You’ve asked for 5 codes in an hour. Try again in 41:12." (B-34; the digits `aria-hidden` with sr "at 11:24 AM", as drawn on `Signin-limited`); note "Can’t wait until then? Contact support." (COV-06); at 0:00 "You can send a code now." | E9 |
| Within 60 s of the last send to the same address | Goes straight to R4 with the countdown running (identity review C2) | — |
| Offline / network / server | We couldn’t send a code because you’re offline. / …Check your connection. / …Try again in a moment. | Slot (alert) |
| Time limit warning; draft expired | As §3.2 (B-29, drawn on spec-edge’s `Signin-timeout-warning`; "This took too long", focus on the alert container, B-38) | §3.2 |

**Rationale.** The new address is the one place support can reach the person, so it is confirmed with a code before anything else (eval R2.5 item 2: the support route must reach a mailbox that works). The note says now that this address gets the updates, so the person picks one they read.

### 3.4 R4 Step 2 of 3: the code (`Recover-code`)

**Route** `/recover/code` · **Title** "Check your new email, step 2 of 3 – Richie" (A11Y-09) · **Reveals** nothing · **Limit** L3, L4 (and L1 for sends) · **Focus** the code input (fine pointer), drawn with `.is-focused` on the active slot · **Grammar** Part A §4.7 code screens (B-39): head, code, meta, **Continue**, then the ways out below it. Tab order: the code, Continue, the note’s support link, Use a different email

| Element | Copy | Notes |
|---|---|---|
| Step line | Step 2 of 3 | |
| `h1` | Check your new email | |
| Lede | Enter the code we sent to **jamie@rivera.studio** to confirm your new email. | The lede names the purpose (B-39); long-email markup. 2 lines at 390 (measured) |
| Code | 8 slots, 4 + 4; sr label "Email code, 8 letters and numbers" | AS §4.4 input rules; no auto-submit (Part A §2 row 12) |
| Meta line 1 | Codes expire after 5 minutes. | |
| Meta line 2 | Send a new code in 1:00 | From the server’s next-allowed time; at 0:00 the plain button **Send a new code**, sr "You can send a new code now." |
| Primary | Continue | busy "Checking code…", sr "Checking your code…" |
| Note | No code after a few minutes? Check your spam folder, or contact support. | Drawn (COV-06); topic `recovery-request` |
| Switch line | Use a different email | → R3 with the field filled |

| State | Copy | Layout |
|---|---|---|
| Incomplete | Enter all 8 characters. (meta line 1, alert) | AS `Stepup-code-error` |
| Wrong, 3 / 2 / 1 tries left | That code isn’t right. You have 3 tries left. / …2 tries left. / …1 try left. | AS `Stepup-code-error` |
| Locked (4th wrong) | Lede: "You’ll need a new code. We’ll send it to **jamie@rivera.studio**." Error "That was the last try for this code." (focus moves to it). Input `readonly` + `aria-disabled`. Primary **Send a new code in 0:41** (accessible name "Send a new code, available at 10:47 AM", B-27) → **Send a new code** | AS `Stepup-code-locked` |
| Expired | Lede as locked; error "This code has expired."; primary **Send a new code** | AS §4.4 |
| New code sent | Meta line 1 "New code sent. Use the code in the newest email." (polite), until the first character is typed (B-20) | Part A §2.1 |
| Send failed | We couldn’t send a code. Check your connection. (and the offline and server variants); primary **Try again** | AS §4.4 |
| Check failed | We couldn’t check your code because you’re offline. / …Check your connection. / …Try again in a moment. (meta line 1) | AS §4.4 |
| Failed submit (B-31) | The document title gets "Error: "; focus stays in the code input with its text selected | AS §4.4 |
| Success | → R5; the new address stays confirmed for 30 minutes without activity (B-29). Canvas: the form goes to `Recover-details` (COV-22) | — |

**Rationale.** It is the code entry people already met on Verify and in step-up, with the commit named for what it does here (Part A §2.1: "Continue"). "Use a different email" stays one tap away because a typo in the new address is the likeliest reason no code arrives.

### 3.5 R5 Step 3 of 3: details for support (`Recover-details`, `-mobile`; `Recover-limited`)

**Route** `/recover/details` · **Title** "Tell us about your account, step 3 of 3 – Richie" (D-R1, A11Y-09) · **Reveals** nothing · **Limit** L5 (authoritative check at submit) · **Focus** the first field (fine pointer)

| Element | Copy | Notes |
|---|---|---|
| Step line | Step 3 of 3 | |
| `h1` | Tell us about your account | |
| Lede | Richie support compares your answers with the account’s history to check it’s you. | |
| Field 1 | **Your name**; hint "As it appears on your Richie account." | Required; `autocomplete=name`, `autocapitalize=words`; up to 100 characters |
| Field 2 | **Devices you use Richie on**; hint "Like “Chrome on a Mac” or “Safari on an iPhone”." | Required; up to 200 characters |
| Field 3 | **When you last signed in (optional)**; hint "Roughly is fine, like “last Tuesday”." | Up to 100 characters |
| Field 4 | **When you created the account (optional)**; hint "A month and year is enough, like “March 2025”." | Up to 100 characters |
| Field 5 | **Anything else (optional)**; textarea, 3 lines; hint "Like why you can’t get email at your old address." | Up to 1,000 characters; resizable |
| Primary | Send request | busy "Sending request…" |
| Note | We use your answers only to check it’s you. Richie support never asks for email codes. | The second sentence is the anti-phishing line support repeats |
| Switch line | Back | → R3 (the new address stays confirmed; R4 is skipped) |

Hints are linked with `aria-describedby`; errors join them. The lengths are checked on submit with the "Too long" errors below; no field has `maxlength`, because a pasted answer would be cut silently (spec-settings §2.1 makes the same call for passkey names). Free text only: no pickers to guess a creation month, which most people don’t know, and nothing workspace-related is asked or suggested (Part A principle 7).

| State | Copy | Layout |
|---|---|---|
| Default (empty) | — | **drawn** `Recover-details`, `-mobile` |
| Name empty | Enter your name. | Field error |
| Devices empty | Enter at least one device you use Richie on. | Field error |
| Too long (on submit; characters counted after trimming) | Use 100 characters or fewer. / Use 200 characters or fewer. / Use 1,000 characters or fewer. | Field error; focus the first such field, its text kept in full |
| Sending | **Sending request…** (busy) | busy pattern |
| Offline / network / server | We couldn’t send it because you’re offline. / We couldn’t send it. Check your connection. / We couldn’t send it. Try again in a moment. (sr: "send your request") | Slot (alert), D-R4, A11Y-07 |
| L5 per IP | Fields keep their values; primary **Send request at 9:15 AM** (unavailable, sr ", tomorrow", `aria-describedby` the slot; 178 px in SF at 16 px, inside 256, RIG-01); slot (polite) "This network has made 5 recovery requests today." (B-12); note "Can’t wait until tomorrow at 9:15 AM? Contact support." When the time comes: **Send request** and "You can send your request now." | **drawn** `Recover-limited`, `Recover-limited-dark` |
| L5 per new address (race) | → R3 with its field error (the B-12 sentence is 2 lines at 358, so it can’t sit in a slot) | copy |
| A request was opened for the account meanwhile | → R2 with its "Open request" error | copy |
| Time limit warning (B-29) | As §3.2; typing in any field renews the draft and the confirmation | spec-edge §3.6, `Signin-timeout-warning` |
| New-address confirmation idle for 30 minutes | → R3 with an alert at first paint: title **Confirm your new email again**, description "It’s been 30 minutes without activity since you confirmed it. We kept your answers." Focus the alert container, labelled by its title and described by its description (B-38) | copy; Part A §4.10 pattern |
| Success | → R6 (the same status page the email links to). Canvas: the form goes to `Recover-review` (COV-22) | — |

**Rationale.** Five short questions, two required, because support decides by comparing answers with records (Q10): devices and recent sign-ins are the strongest signals, the creation date a weak one, and the free text often explains the loss ("I changed jobs"). Saying what the answers are for, and that support never asks for codes, answers the two worries a careful person has before sending personal details to a form.

### 3.6 R6 Under review (`Recover-review`), R6a Cancel your request? (`Recover-review-cancel`), R6b the owner’s view (`Recover-owner-view`)

**Route** `/recover/status?token=…` (and the page after submit) · **Title** "We’re reviewing your request – Richie" · **Reveals** this request’s status and both addresses to whoever has the link (the requester typed both) · **Limit** L7 · **Focus** the `h1` · **Badge** clock

| Element | Copy | Notes |
|---|---|---|
| `h1` | We’re reviewing your request | |
| Lede | Richie support usually replies within 2 business days. We’ll email you at your new address. | |
| Facts (`dl`, Mist group) | **Account** jamie.rivera@northwind.coffee · **New email** jamie@rivera.studio · **Requested** November 5 at 10:42 AM Pacific Time | D-R3; the zone per B-8 |
| Action (outline link) | Cancel request | A link to R6a, the requester’s confirm (below), so this page has no slot (A11Y-10). Canvas: → `Recover-review-cancel` |
| Note | We also emailed your old address, in case someone else asked. Questions? Contact support. | Link opens the support form in a new tab, topic "My recovery request", the request attached |

| State | Copy | Layout |
|---|---|---|
| Support asked for more | Unchanged; support’s email thread carries the questions | — |
| No decision 7 days after submit (B-36) | The request closes as expired: this link shows R10c (§3.9) | **drawn** `Recover-expired` |
| Link expired or wrong | E5 (recovery variant) | WP3.2 `Error-link` |

**Rationale.** A status page answers three questions in order: is it moving (the `h1` and the clock), when will I hear (the lede, with a number), and is it the request I think it is (labelled facts). Cancel is an outline button so a nervous reader doesn’t take it for the next step; the note tells the requester that the owner was told, which is honest and explains the later emails.

**R6a Cancel your request?** (`Recover-review-cancel`, the requester’s confirm)

**Route** `/recover/status/cancel?token=…` (the status token; the POST also needs antiforgery) · **Title** "Cancel your request? – Richie" · **Reveals** as R6 · **Limit** L7 · **Focus** the `h1` · **Badge** none (a question)

| Element | Copy | Notes |
|---|---|---|
| `h1` | Cancel your request? | The requester’s words ("your request", D-R4) |
| Lede | Your email won’t change. You can start a new request at any time. | |
| Facts | **New email** jamie@rivera.studio · **Requested** November 5 at 10:42 AM Pacific Time | No Account row and no From row: the requester typed both addresses; the confirm only names which request |
| Primary (default, in a `form`, POST) | Cancel request | Busy "Canceling…" (B-28: also into the slot’s polite line) → R10b; emails #11 and #12. Canvas: the form has no target (R10b is copy on `Recover-declined`’s layout) |
| Slot | (empty) | Failures (alert, focus stays on **Cancel request**): "We couldn’t cancel it because you’re offline." / "We couldn’t cancel it. Check your connection." / "We couldn’t cancel it. Try again in a moment." (sr "cancel your request", A11Y-07) |
| Second (outline link) | Keep the request | → R6 (R7 when approved); nothing changes on the server. `.ri-rec-second`, 12 below the slot |

| State | Copy | Layout |
|---|---|---|
| Under review | as above | **drawn** `Recover-review-cancel` |
| Approved (from R7) | Facts add **Approved** November 5 at 4:10 PM Pacific Time and **Email changes** November 8 at 4:10 PM Pacific Time; the rest unchanged | copy, `Recover-review-cancel` |
| Closed meanwhile (decided, expired, canceled by the old address) | The confirm URL shows that status page instead (R8, R9, R10a, R10c): there is nothing to cancel | — |
| Link expired or wrong | E5 (recovery variant) | `Error-link` |

**Rationale.** The requester’s confirm speaks to the person who asked: their email won’t change and they can ask again. It drops R12’s warning ("If it wasn’t you…"), which is the owner’s copy and would read as an accusation here. The filled button is the action they came to take; **Keep the request** is the way back.

**R6b Under review, owner’s view** (`Recover-owner-view`, D-R10)

**Route** `/recover/cancel/status?token=…` (the cancel token, §15.4) · **Title** "We’re reviewing this request – Richie" · **Reveals** the masked new address and the request time to the holder of the old mailbox: no more than R11 and R12 · **Limit** L7 · **Focus** the `h1` · **Badge** clock

| Element | Copy | Notes |
|---|---|---|
| `h1` | We’re reviewing this request | "this request", not "your request": the reader may not have asked (the owner’s wording, D-R4) |
| Lede | Richie support usually replies within 2 business days. We’ll email you if they approve it. | The reply sentence verbatim (COPY-12); "you" is the old address, which gets email #4 on approval |
| Facts | **New email** j•••@rivera.studio (sr "j, hidden, at rivera.studio") · **Requested** November 5 at 10:42 AM Pacific Time | No **Account** row: the owner knows their address, and this link never shows the new address in full (D-R10) |
| Action (outline link) | Cancel this request | → R12 (the email’s button name, B-10). No slot: nothing on this page reports |
| Note | You can cancel it at any time before the email changes. | |

| State | Copy | Layout |
|---|---|---|
| Under review | as above | **drawn** `Recover-owner-view` |
| Approved | `h1` **Support approved this request**; lede "We’ll change the email to j•••@rivera.studio on November 8 at 4:10 PM unless it’s canceled before then."; facts add **Approved** and **Email changes**; action and note unchanged | copy, `Recover-owner-view` |
| Closed (declined, expired, or canceled by either side) | R13 with `h1` **Nothing changed** (as R12, §3.10) | copy, `Recover-canceled` |
| Completed (up to 30 days) | R12 "We already changed the email" (§3.10) | copy, `Recover-declined` |
| Link older than 30 days after close | E5, owner variant | `Error-link` |

**Rationale.** The owner who chooses **Keep the request** is usually the person who asked, or isn’t sure. The page gives them the requester’s status in the owner’s words, with the masked address only, and keeps the protective action one tap away under the same name as the email’s button.

### 3.7 R7 Approved, on hold (`Recover-approved`, `-mobile`)

**Route** same · **Title** "{h1} – Richie": "Your email changes on November 8 – Richie" · **Reveals** as R6 · **Focus** the `h1` · **Badge** clock

| Element | Copy | Notes |
|---|---|---|
| `h1` | Your email changes on November 8 | "today" on the day: **Your email changes today** |
| Lede | We approved your request. We’ll change it to **jamie@rivera.studio** at 4:10 PM. | |
| Facts | **Requested** November 5 at 10:42 AM Pacific Time · **Approved** November 5 at 4:10 PM Pacific Time · **Email changes** November 8 at 4:10 PM Pacific Time | A real sequence; zones per B-8 |
| Action (outline link) | Cancel request | → R6a, the requester’s confirm (§3.6, A11Y-10). Canvas: → `Recover-review-cancel` (from `Recover-approved` and, through its import, `Recover-approved-mobile`) |
| Note 1 | We wait 3 days so your old address can cancel if someone else asked. When it changes, we’ll sign you out on every device. | Why the wait, and what will happen |
| Note 2 | Questions? Contact support. | |

| State | Copy | Layout |
|---|---|---|
| Cancel failed | the three patterns with "cancel it", on the confirm page | Slot of the confirm |
| The hold ends while the page is open | Reload at `changes_at` + 5 s; the server shows R8 only when the stored status is `completed`, else the "changing now" or "blocked" variant (§15.5, COV-23) | — |
| Support withdraws the approval | → R9 | — |
| 7 days after submit | No expiry: B-36 closes only requests nobody approved or declined; this hold runs as specified | — |
| The old address cancels | → R10a | copy |
| Mobile | — | **drawn** `Recover-approved-mobile` |

**Rationale.** The date is the news, so it is the `h1`; the time is in the lede and the facts so nobody has to compute "72 hours". The reason for the wait is written for the honest requester, who is the usual reader: it protects them too.

### 3.8 R8 Completed (`Recover-completed`)

**Route** same · **Title** "Your email was changed – Richie" · **Focus** the `h1` · **Badge** check

| Element | Copy | Notes |
|---|---|---|
| `h1` | Your email was changed | |
| Lede | We signed you out on every device. Use **jamie@rivera.studio** to sign in from now on. | |
| Primary | Sign in | → A1 with `login_hint` = the new email and `returnUrl=/account/security` (D-R5) |
| Note | Until November 15, we’ll ask you to confirm it’s you before sensitive changes. Add a passkey so you can get back in without email. | |

**Rationale.** The person’s next move is to sign in with the new address, so that is the only button and the address is in bold in the lede. The note explains the extra step-ups before they meet them, and points at the one setting that prevents a second recovery.

### 3.9 R9 Declined (`Recover-declined`), R10 Canceled (copy) and R10c Expired (`Recover-expired`)

**Route** same · **Titles** "We couldn’t approve your request – Richie"; R10a "This request was canceled – Richie"; R10b "You canceled your request – Richie"; R10c "We couldn’t finish your request in time – Richie" · **Focus** the `h1`

**R9** (badge exclamation)

| Element | Copy | Notes |
|---|---|---|
| `h1` | We couldn’t approve your request | |
| Lede | We couldn’t confirm the account is yours from the details you gave. Your email hasn’t changed. | One reason for every decline (§11 Q-R5) |
| Primary | Contact support | New tab ("Contact support, opens in a new tab"), topic "My recovery request was declined", the request attached; `ArrowSquareOut` icon |
| Second (unavailable, outline look) | New request on November 12 | 214 px in SF at 16 px, inside 256 (RIG-01). On the day: "New request at 4:10 PM"; then an outline link **Start a new request** → R2. `aria-disabled`, focusable |
| Note | Have a passkey, or a linked Google or Microsoft account? Sign in with it instead. | "Sign in with it" → A1 |

**R10a**, the old address canceled (badge exclamation, `Recover-declined` layout): `h1` **This request was canceled**; lede "Someone with access to the account’s current email canceled it. Your email hasn’t changed."; primary **Contact support** (topic "My recovery request was canceled"); second **New request on November 13** (D-R8), then active; note as R9.

**R10b**, the requester canceled (badge check, D-R6): `h1` **You canceled your request**; lede "Your email hasn’t changed. You can start a new request at any time."; primary **Start a new request** → R2; note "Questions? Contact support."

**R10c**, the request expired (B-36; `Recover-expired`, badge exclamation, the status-page family on `Recover-declined`’s structure)

| Element | Copy | Notes |
|---|---|---|
| `h1` | We couldn’t finish your request in time | B-36’s notice. 2 lines at 1440 and 390 (balanced, measured) |
| Lede | Requests close after 7 days without a decision, so we closed yours. Your email hasn’t changed. | Says why without blaming either side: support may have been waiting for a reply. 2 lines at 390 |
| Primary (default link) | Start a new request | → R2 (`Recover-account`) at once: no wait after an expiry (B-36), so not the dated button R9 and R10a show |
| Second (outline link) | Contact support | New tab (accessible name "Contact support, opens in a new tab", `ArrowSquareOut`), topic `recovery-request`, `request` = the status token, `from=R10c` (§15.1) |
| Note | You can start a new one now. Have a passkey, or a linked Google or Microsoft account? Sign in with it instead. | "Sign in with it" → A1 |

| State | Copy | Layout |
|---|---|---|
| Expired | as above | **drawn** `Recover-expired` |
| Opened later through the requester’s confirm URL (`/recover/status/cancel?token=…`) | R10c: there is nothing to cancel | `Recover-expired` |
| A new request was started since | This status link keeps showing R10c (it belongs to the closed request); the new request has its own tokens | `Recover-expired` |
| Link older than 30 days after the expiry | E5 (recovery variant) | `Error-link` |

**Status tokens after an expiry.** The status token keeps working until 30 days after the close and shows R10c, on both its routes. The cancel token (old address) shows R13 "Nothing changed" on `/recover/cancel` and on the owner’s view (`/recover/cancel/status`), as after a decline. No dispute token exists (nothing changed). §15.4 lists the tokens.

**IA row** (for Part A §3.1, after R10b):

| ID | Page or state | Route | Document title | WP | Entry points | Exits | Reveals | Limit |
|---|---|---|---|---|---|---|---|---|
| R10c | **Expired** (`Recover-expired`) | same as R6 | We couldn’t finish your request in time – Richie | 3.4 | No decision 7 days after submit (B-36); email #16 | **Start a new request** → R2 (no wait); **Contact support** (new tab, topic `recovery-request`); note: sign in with a passkey, Google or Microsoft | as R6 | L7 |

**Rationale.** A refusal needs a person to talk to and a date, not a dead end: Contact support is the primary because it is the only thing that can change the outcome, and the new-request button shows its date while it waits (the countdown-button pattern, at the scale of days). The note reminds people of the faster routes without saying whether this account has them. R10c is different: nobody refused anything, so the fastest way forward (a new request, at once) is the primary and support is second, for someone who wants to know why. Its badge is the exclamation (Part A §4.9 "failed or refused": the change didn’t happen), because in this flow the clock means "you’re waiting" (R6, R7) and this page ends the wait.

### 3.10 R12 Cancel this recovery request? (`Recover-cancel`)

**Route** `/recover/cancel?token=…` · **Title** "Cancel this recovery request? – Richie" · **Reveals** the masked new address, the request’s time, browser and city, to the holder of the old mailbox · **Limit** L7 · **Focus** the `h1` · **Badge** none (a question)

| Element | Copy | Notes |
|---|---|---|
| `h1` | Cancel this recovery request? | |
| Lede | Someone asked Richie support to change the email on your account. If it wasn’t you, cancel it. | No address in the lede (it can’t orphan punctuation; CSS-REQUESTS WP3.4-2) |
| Facts | **New email** j•••@rivera.studio (sr "j, hidden, at rivera.studio") · **Requested** November 5 at 10:42 AM Pacific Time · **From** Chrome on macOS in Portland, Oregon | City from the request’s IP, as AS activity rows; zone per B-8 |
| Primary (default, in a `form`, POST) | Cancel the request | busy "Canceling…" (B-28). Canvas: the form goes to `Recover-canceled` (COV-22) |
| Slot | (empty) | Failures: "We couldn’t cancel it because you’re offline." / "…Check your connection." / "…Try again in a moment." (sr "cancel the request", A11Y-07) |
| Second (outline) | Keep the request | → R6b, the owner’s view (`Recover-owner-view`, §3.6; D-R10); nothing changes on the server |
| Note | Not sure? Cancel it. Nothing about your account changes. | Canceling is always safe for the owner |

| State | Copy | Layout |
|---|---|---|
| Under review | as above | **drawn** |
| Approved | Lede "Someone asked Richie support to change the email on your account, and support approved it. If it wasn’t you, cancel it before November 8 at 4:10 PM."; facts add **Email changes** November 8 at 4:10 PM | copy, `Recover-cancel` |
| Already canceled by this link | R13 with lede "You already canceled this request on November 6. Nothing changed." (spec-edge QE-5) | copy, `Recover-canceled` |
| Already closed (declined, expired, or canceled by the requester) | R13 with `h1` **Nothing changed**, lede "This request was already closed, and your account still has its email." | copy, `Recover-canceled` |
| Already completed (up to 30 days) | `h1` **We already changed the email**; lede "We moved your account to j•••@rivera.studio on November 8. If this wasn’t you, tell Richie support by December 8."; primary **Contact support** (topic "Recovery I didn’t ask for", the dispute token attached); note "We’ll check it’s you and can change it back." Badge exclamation | copy, `Recover-declined` |
| Link older than 30 days after close | E5, owner variant (spec-edge §E5) | WP3.2 |

**Rationale.** The owner arrives alarmed from an email. The page repeats what the email said, adds the three facts that help them judge (where to, when, from where) and makes the protective action the filled one. "Not sure? Cancel it." removes the fear of breaking something: a real requester can ask again.

### 3.11 R13 Canceled, owner’s view (`Recover-canceled`)

**Route** `/recover/canceled` (after the POST, no token in the URL) · **Title** "We canceled the request – Richie" · **Focus** the `h1` · **Badge** check

| Element | Copy | Notes |
|---|---|---|
| `h1` | We canceled the request | |
| Lede | Nothing changed: **jamie.rivera@northwind.coffee** is still your account’s email. | |
| Primary | Sign in | → A1 with `returnUrl=/account/security` |
| Note | We won’t accept another request for this account for 7 days. After you sign in, check your passkeys and devices. | D-R8 |

**Rationale.** Confirm the outcome in the owner’s own terms (their address is unchanged), say what we did to stop a repeat, and send them to the one page where they can check nothing else was touched.

### 3.12 R14 Sign-in and security, recently recovered (`Security-recovered`)

**Route** `/account/security` · **Title** unchanged ("Sign-in and security – Richie") · **Focus** the alert on arrival (B-38): the `section` takes `tabindex="-1"`, `aria-labelledby` its title and `aria-describedby` its description, no ring, so the checklist is read with the title; the `h1` keeps `tabindex="-1"` for the skip link · Board drawn on November 8 at 4:32 PM

| Element | Copy | Notes |
|---|---|---|
| Alert (default, `Info`, between the lede and the first section; a `section` named by its title and described by its description, focused on arrival, no live `role`; A11Y-11, B-38) | Title **Your email was changed by account recovery on November 8**. Description "Until November 15, we’ll ask you to confirm it’s you before every sensitive change. Check your passkeys and devices. If you didn’t ask for this, contact Richie support." | "passkeys" → `#passkeys`, "devices" → `#sessions`, "contact Richie support" → support form, new tab, topic "Recovery I didn’t ask for". Shown on every visit until `recently_recovered_until` |
| Email codes row | jamie@rivera.studio | |
| Passkeys | One row, Pixel 8 (Google Password Manager, added October 28, last used October 30) | Kept (Q14); it is the lost phone the person should remove |
| Google and Microsoft | Google "Not linked. Works with any Google account." **Link** (spec-settings wording, COPY-07); Microsoft as the base; both descriptions reserve 2 lines (`ri-set-2l`, WP3.3) | |
| Where you’re signed in | Chrome on macOS, "This device"; foot "You’re not signed in anywhere else." | Completion signed out every device |
| Activity (new rows first) | **Signed in with an email code** "Today at 4:32 PM. Chrome on macOS in Portland, Oregon." · **Email changed by account recovery** "Today at 4:10 PM. Every device was signed out." (`Lifebuoy`) · **Account recovery requested** "November 5 at 10:42 AM. Chrome on macOS in Portland, Oregon." (`Lifebuoy`) · **Signed in with a passkey** "October 30 at 9:14 AM. Chrome on Android in Portland, Oregon." | Passive titles: the person reading may not be the one who asked |
| Sidebar | Jamie Rivera, jamie@rivera.studio | Own copy (D-R9), with the shared short-address class `ri-email-short` |

| State | Copy | Layout |
|---|---|---|
| Step-up during the 7 days (screen A note) | Your account was recovered recently, so we’ll ask every time until November 15. | copy, AS `Stepup` note line |
| Step-up screens B and C | unchanged | AS |
| After November 15 | No alert; step-up back to the 10-minute rule; the activity row stays for 30 days | `Security` |
| Undone by support | The account returns to the old address; every device signed out again; activity "Email changed back by Richie support" (`Lifebuoy`); no alert | copy |

**Rationale.** After a recovery the useful thing is a short checklist in the place where it can be acted on: the passkey list (remove the lost phone, add a new passkey) and the device list. The alert speaks to both possible readers: the owner who recovered (check, and expect step-ups) and the owner who didn’t (contact support), without accusing either.

---

## 4. Emails (R11, R15)

Security emails, always sent, sender "Richie", one button each, Reply-To the support desk for every email to the new address (so a reply joins support’s thread). Greeting "Hi Jamie," only to the account’s own address or after completion; "Hi," to the new address before then (the name isn’t verified). The new address is masked in every email to the old address (Part A §4.8). Times carry the time zone (§2.5).

### 4.1 Every email

| # | When | To | Subject | Preview text | Body | Button → |
|---|---|---|---|---|---|---|
| 1 | Submit | New | We got your Richie recovery request | Richie support usually replies within 2 business days. | Hi, we got your request to change the email on your Richie account to this address, jamie@rivera.studio. Richie support usually replies within 2 business days. We’ve also told the account’s current email address, in case someone else asked. | **See your request** → R6 |
| 2 (R11, drawn) | Submit | Old | Someone asked to change the email on your Richie account | If this wasn’t you, cancel the request. Nothing has changed yet. | Hi Jamie, someone asked Richie support to change your account’s email from jamie.rivera@northwind.coffee to j•••@rivera.studio. They said they can’t get email at this address anymore. / If this wasn’t you, cancel the request. Your account stays as it is. / [button] / If it was you, there’s nothing to do. We’ll email you again if support approves it, and we wait 3 days after that before changing anything. / Facts: Requested November 5 at 10:42 AM Pacific Time; From Chrome on macOS in Portland, Oregon | **Cancel this request** → R12 (B-10) |
| 3 | Approval | New | We approved your Richie recovery request | Your email changes on November 8 at 4:10 PM. | Hi, Richie support approved your request. We’ll change your account’s email to jamie@rivera.studio on November 8 at 4:10 PM Pacific Time, unless the request is canceled before then. When it changes, we’ll sign you out on every device, and you’ll sign in with this address. | **See your request** → R7 |
| 4 | Approval | Old | Your Richie email changes on November 8 unless you cancel | Support approved a request to change your account’s email to j•••@rivera.studio. | Hi Jamie, Richie support approved a request to change your account’s email from jamie.rivera@northwind.coffee to j•••@rivera.studio. We’ll make the change on November 8 at 4:10 PM Pacific Time. If this wasn’t you, cancel it before then. / Facts as #2 | **Cancel this request** → R12 (approved); "If this wasn’t you, cancel it before then." sits above it (B-10) |
| 5 | Completion | New | Your Richie email was changed | Sign in with jamie@rivera.studio from now on. | Hi Jamie, your Richie account now uses jamie@rivera.studio. We signed you out on every device, so sign in again with this address. Until November 15, we’ll ask you to confirm it’s you before sensitive changes. After you sign in, check your passkeys and devices, and add a passkey so you can get back in without email. | **Sign in** → A1 (`login_hint`, `returnUrl=/account/security`) |
| 6 | Completion | Old | The email on your Richie account was changed | If this wasn’t you, tell us by December 8. | Hi Jamie, on November 8 at 4:10 PM Pacific Time we changed your Richie account’s email to j•••@rivera.studio, after a recovery request that Richie support approved. This address won’t get emails about the account anymore. If this wasn’t you, tell us by December 8 and we’ll help you get your account back. | **Contact support** → the support form, topic `recovery-not-me`, `request` = the dispute token (30 days); later E5 owner variant (B-10) |
| 7 | Decline | New | We couldn’t approve your Richie recovery request | Your email hasn’t changed. | Hi, Richie support couldn’t confirm the account is yours from the details you gave, so we didn’t change anything. You can start a new request on November 12, or contact Richie support if you think we got it wrong. | **See your request** → R9 |
| 8 (D-R7) | Decline | Old | We stopped a request to change your Richie email | Nothing changed on your account. | Hi Jamie, Richie support declined the request to change your account’s email to j•••@rivera.studio. Nothing changed. If someone is trying to get into your account, sign in and check your passkeys and devices. | **Sign in** → A1 (`returnUrl=/account/security`) |
| 9 | Owner cancels | New | Your Richie recovery request was canceled | Your email hasn’t changed. | Hi, someone with access to the account’s current email canceled your request, so we didn’t change anything. If that wasn’t you and you still can’t get that email, contact Richie support. | **See your request** → R10a |
| 10 | Owner cancels | Old | You canceled the request to change your Richie email | Your account still uses this address. | Hi Jamie, you canceled the request to change your account’s email to j•••@rivera.studio. Nothing changed, and we won’t accept another request for 7 days. Sign in to check your passkeys and devices. | **Sign in** → A1 (`returnUrl=/account/security`) |
| 11 | Requester cancels | New | You canceled your Richie recovery request | Your email hasn’t changed. | Hi, you canceled your request, so we didn’t change anything. You can start a new one at any time. | **Start a new request** → R2 |
| 12 | Requester cancels | Old | A request to change your Richie email was canceled | Nothing changed on your account. | Hi Jamie, the person who asked to change your account’s email to j•••@rivera.studio canceled the request. Nothing changed. | **Sign in** → A1 |
| 13 (D-R7) | Support undoes (≤30 days) | Old (now current again) | We changed your Richie email back | Your account uses this address again. | Hi Jamie, Richie support changed your account’s email back to jamie.rivera@northwind.coffee and signed you out on every device. Sign in to check your passkeys and devices. | **Sign in** → A1 (`returnUrl=/account/security`) |
| 14 (D-R7) | Support undoes | The recovered address | Richie support reversed an account recovery | This address is no longer on a Richie account. | Hi, Richie support reversed the recovery that moved a Richie account to jamie@rivera.studio, so this address is no longer on it. If you think this is a mistake, contact Richie support. | **Contact support** |
| 16 (B-36) | Expired: no decision 7 days after submit | New | We couldn’t finish your Richie recovery request in time | Your email hasn’t changed. You can start a new request now. | Hi, we couldn’t finish your request in time. Richie support didn’t approve or decline it within 7 days, so we closed it and didn’t change anything. You can start a new request now, with no wait, or contact Richie support if you still can’t get into your account. | **See your request** → R10c |
| 17 (B-36) | Expired | Old | A request to change your Richie email was closed | Nothing changed on your account. | Hi Jamie, we couldn’t finish the request to change your account’s email to j•••@rivera.studio in time, so we closed it. Nothing changed. If someone is trying to get into your account, sign in and check your passkeys and devices. | **Sign in** → A1 (`returnUrl=/account/security`) |

**Fine print** (every email to the account’s address, B-7): "You’re getting this because it’s a security change on your Richie account. We always send these." To the new address before completion: "You’re getting this because someone entered this address in a Richie recovery request and confirmed it with a code." Then "Button not working? Paste this link into your browser:" and the full link. Footer: Richie, Help, Privacy, Terms (COPY-18).

### 4.2 R11 rationale

The owner may read this on a phone in a hurry, so the subject says what happened in plain words, the masked address shows it isn’t theirs, the first decision ("If this wasn’t you") comes before the button, and the reassurance for the honest requester comes after it. The facts (when, browser, city) are the evidence an owner uses to decide. Nothing in it says "hold", "ticket" or "72 hours".

### 4.3 How the drawing maps to the real email

| Drawing (`Email-recovery-alert`, 720 wide) | Real email (MJML → HTML, plus a plain-text part) |
|---|---|
| Inbox lines above the card (From, To, Subject, Preview) | Headers: From "Richie" with engineering’s security sender; Subject; the preview text as a hidden preheader at the top of the body |
| Mist ground, Paper card max 600, radius 18, padding 40 (20 below 600) | `mj-body` Mist `#f5f5f7`, one `mj-section` 600 wide, white, radius 18 where supported (square in Outlook) |
| Mascot PNG 31 × 39, `alt="Richie"` | The same PNG at 2× (62 × 78) scaled to 31 × 39; `alt="Richie"` because many clients block images and the alt text then names the sender |
| Title 24/32/600 | `mj-text` 24 px, the font stack `-apple-system, BlinkMacSystemFont, "Segoe UI", Inter, sans-serif` (no web font in email) |
| Body 16/24 Ink | `mj-text` 16/24 `#1d1d1f` |
| **Cancel this request** (B-10; default `lg`, Signal fill, white text, a pill: the board’s inline `lg` button) | `mj-button` bulletproof (VML `roundrect` for Outlook), `#0071e3`, white 16 px, 44 tall, `border-radius` 22 px (the pill; Outlook without VML arcs draws square corners) |
| Facts group (Mist rows) | A two-row `mj-table` on `#f5f5f7` |
| Fine print under a hairline; link fallback | `mj-divider` `#d2d2d7`, 14/20 `#6e6e73`; the full URL as text |
| Masked address with an sr string | Plain text "j•••@rivera.studio" (email clients don’t honour sr-only CSS); the plain-text part writes "j***@rivera.studio" |
| Dark (the `.dark` build) | `prefers-color-scheme` styles where supported; colours chosen to survive Gmail’s forced inversion |
| — | `lang="en"`, no tracking pixel or click tracking on security emails, no unsubscribe link (security emails can’t be turned off, AS Preferences) |

WP3.3’s S6 email reuses this layout and masking (spec-settings §3.4): keep the board’s structure stable.

---

## 5. What support sees and decides

### 5.1 Procedure outline (support owns the written procedure, how-we-design §6)

1. **Queue.** A new request appears in the support tool’s "Recovery requests" queue, oldest first, with the 2-business-day target and the day it closes (7 days after submit, B-36) shown per request.
2. **Read the request:** the account (old email, as typed), the confirmed new email, the R5 answers, and the request’s time, IP city, browser and device.
3. **Compare with the account’s records**, shown side by side: profile name; creation date; sign-in history (methods, browsers, devices, cities, last sign-in); current devices; passkey count and names; linked providers; whether the old address bounced since the request (a hard bounce or a missing domain supports "I can’t get email there"); whether the owner alert was delivered; earlier recovery requests and their outcomes; deletion state.
4. **Risk signals:** a request from a country or browser the account never used; a sign-in to the account after the request was sent (someone has access: decline unless the requester explains it); a new address on a disposable-mail domain; several accounts recovered to one new address; an account email change in the last 30 days.
5. **Decide:**
   - **Ask for more** by replying to the new address from the support desk (the request stays under review). Support never asks for an email code, a passkey or anything the account itself would use to sign in.
   - **Approve:** starts the 72-hour hold (`changes_at` set by the server); both addresses are emailed (#3, #4).
   - **Decline** with an internal note (never shown to the requester; the page and email give one reason, Q-R5); both emails go (#7, #8); new requests wait 7 days.
   - **No decision in 7 days** (automatic, B-36): the request closes as expired (R10c); both emails go (#16, #17); a new request is allowed at once, so a requester support was waiting on can simply ask again.
6. **During the hold:** support can withdraw an approval (→ declined) if new evidence arrives; the old address and the requester can cancel.
7. **After completion:** a dispute through **Contact support** (completed email #6, R12 "already changed", the R14 alert; topic `recovery-not-me`) opens a priority case with the dispute token. For 30 days support can **undo**: the email returns to the old address, every device is signed out again, and emails #13 and #14 go. Support can also **lift the 7-day wait** on an account.

### 5.2 Support tooling (handoff, internal tool, not drawn)

| Need | Detail |
|---|---|
| Queue | Filters: open, approved (on hold), disputed, expired; sort by age; the 2-business-day clock and the 7-day close (B-36) |
| Request view | Request facts, answers, the account’s records (5.1 step 3) side by side, risk flags |
| Actions | Reply (desk thread to the new address), Approve, Decline (internal note), Withdraw approval, Undo (≤30 days after completion), Lift wait |
| Audit | Every view and action logged with the agent and time; approval and undo need a second agent when the account had a sign-in after the request (Q-R7) |
| Never shown to agents | Email codes, passkey material, session tokens |

---

## 6. State matrix

"copy" = specified here on a drawn layout (named); "harness" = rig viewports, the forced-dark build, forced colours, reduced motion, 200 %; "n/a" with a reason.

| how-we-design §4 item | R1 | R2–R3 | R4 | R5 | R6–R10 status | R11, R15 emails | R12–R13 | R14 |
|---|---|---|---|---|---|---|---|---|
| Default, realistic data | `Recover` | `Recover-account` (email prefilled), `Recover-email` | `Recover-code` | `Recover-details` | `Recover-review`, `Recover-review-cancel` (R6a), `Recover-approved`, `Recover-completed`, `Recover-declined`, `Recover-expired` (R10c) | `Email-recovery-alert` (#2) | `Recover-cancel`, `Recover-owner-view` (R6b), `Recover-canceled` | `Security-recovered` |
| Long or extreme data | copy (no data) | Long email in the field scrolls inside it: harness | Long new address in the lede wraps after the @ (§4.8 markup): copy | Long answers wrap in the textarea: copy | Long addresses in fact rows wrap after the @ (`.ra-email-wrap`): copy | Masked address is always short; the old address wraps: copy | as status | AS `Profile-extreme` rules |
| Prefilled vs empty | n/a (no field) | R2 prefilled (drawn); R3 empty (drawn) | n/a | empty (drawn), filled `Recover-limited` | n/a | n/a | n/a | n/a |
| Arrival focus | `h1` | field (fine pointer); with an expiry alert, the alert container (B-38) | code input, `.is-focused` drawn (B-31; also on `Verify-help`) | first field | `h1` | n/a | `h1` | the alert `section`, labelled and described (A11Y-11, B-38) |
| Busy | Passkey "Waiting for your device…", provider "Taking you to Google…": copy | "Checking…", "Sending code…": copy | "Checking code…": copy | "Sending request…": copy | "Canceling…": copy | n/a | "Canceling…": copy | AS |
| Success, next page | → Verify / signed in | → R3, → R4 | → R5 | → R6 | R7 → R8 reload | n/a | → R13 | n/a |
| Field validation | n/a | copy (§3.2, §3.3; layout `Profile-email-taken`, `Main` family) | AS `Stepup-code-error` | copy (§3.5; on submit, no `maxlength`) | n/a | n/a | n/a | n/a |
| Server error, network error, offline | copy (slot) | copy (slot) | copy (meta) | copy (slot) | copy (slot) | n/a | copy (slot) | AS |
| Page can’t load offline | E8 | E8 | E8 | E8 | E8 | n/a | E8 | AS |
| Rate limit or cooldown | L6 on methods (WP3.5) | L2, L5 countdown: copy (E9 `Signin-limited`), countdown names per B-27; L1: copy | 1:00 resend (drawn), locked: AS `Stepup-code-locked` | **`Recover-limited`**, **`Recover-limited-dark`** | L7 → E10 | n/a | L7 → E10 | AS |
| Sign-in request expired | "Back to the code" → E1 | n/a (recovery drops the sign-in) | n/a | n/a | n/a | n/a | n/a | n/a |
| Time limit warning (B-29) | n/a | "Still there? This page times out in 2:00." + Keep going: copy on the pattern drawn once as spec-edge’s `Signin-timeout-warning` | same | same | n/a | n/a | n/a | n/a |
| Draft or link expired | n/a | "This took too long" alert: copy (§4.10 pattern, focus on the alert container, B-38) | "Confirm your new email again": copy | same | E5; the request itself expired after 7 days: **`Recover-expired`** (R10c, B-36) | n/a | variants: copy; E5 | n/a |
| Completion delayed or blocked | n/a | n/a | n/a | n/a | "Your email is changing now", "We need to check something": copy (§15.5, `Recover-review` layout) | #15: copy | n/a | n/a |
| Codes: wrong 3/2/1, incomplete, locked, expired, new code, send failed | n/a | n/a | copy (§3.4) | n/a | n/a | n/a | n/a | n/a |
| Can’t get the code | is this page; its entry from Verify is **`Verify-help`** (handoff reference, §7.1); from P6 and passkey sign-in: copy | Support link in limit and closed-door states: copy (COV-06) | "Use a different email" and the support note (drawn) | n/a | n/a | n/a | n/a | AS screen C → "Request account recovery" (§15.6) |
| Passkeys: waiting, failed, unsupported | copy | n/a | n/a | n/a | n/a | n/a | n/a | AS |
| Account cases: unknown, deleted, scheduled, open request, waiting | via A1 | copy (§3.2) | n/a | race: copy | R9 drawn; R10a/b: copy; R10c drawn | n/a | already canceled / closed / completed: copy; owner’s view R6b drawn (`Recover-owner-view`), its approved variant copy | undone: copy |
| Enumeration and limits | §3.1 | §3.2, §3.3 | §3.4 | §3.5 | §3.6 | §4 | §3.10 | n/a |
| Destructive confirmation | n/a | n/a | n/a | n/a | Requester’s cancel confirm R6a: **`Recover-review-cancel`** | n/a | `Recover-cancel` | AS |
| Step-up | n/a | n/a | n/a | n/a | n/a | n/a | n/a | Note: copy (AS `Stepup`) |
| Widths 320–1920, ≤540 tall, 200 % | harness | harness | harness | harness | harness | harness | harness | harness |
| Dark | **`Recover-dark`**; others in the forced-dark build (`BOARD_THEME=dark`) | harness | harness | **`Recover-limited-dark`** (B-21); harness | harness | harness | harness | harness |
| Forced colours | harness (`Recover`) | harness | harness (`Recover-code`) | harness | harness (`Recover-approved`, `Recover-declined`) | harness | harness | harness |
| Reduced motion | harness (nothing animates but the spinner and caret) | | | | | | | |
| Mobile drawn | `Recover-mobile` | harness | harness | `Recover-details-mobile` | `Recover-approved-mobile` | harness at 390 | harness | n/a |

**Layout shift (m, 1440 × 900 / 390 × 844):** R2 and R3 `h1` 190 / 158 and primary 414 / 382 on both boards; R5 and its limited state primary 990 / 958; R6, R7 and R12 slots at 594 / 598; R8, R9, R13 primary 290 (R9 294 at 390, two-line `h1`), the same as `Signedout`; R1 primary 290 / 258 and every head as `Main` and `Signedout` (mascot 64 / 32, `h1` 166 / 134 without a step line).

---

## 7. Handoff to the identity canvas, and links other WPs use

### 7.1 A6: Verify family ("Can’t get the code?")

**Drawn on this canvas as `Verify-help`** (Phase 4, COV-12): the sign-in Verify page in this frame with the delta below applied, code input focused (B-31), following the code-screen grammar of Part A §4.7 (B-39), so the entry can be seen and clicked (it links to `Recover`; `Recover` "Back to the code" links back to it). It is a reference for the identity canvas, not a replacement for its boards.

Boards: `Verify`, `Verify-incomplete`, `Verify-error`, `Verify-checking`, `Verify-locked`, `Verify-expired`, `Verify-sent`, `Verify-resend` (and the mobile and dark wrappers, which import them). **Sign-in only**, not the sign-up Verify (D-R13).

| Now | Delta |
|---|---|
| `<p class="rc-switch">` "Can’t find “Your Richie code” from Richie? Check spam, or use a different email." | Two lines in the switch position, centred, 14/20: line 1 a link **Can’t get the code?** → `/signin/help?returnUrl=<the same return URL>` (on this canvas `Recover.dc.html`); line 2 "Not your email? **Use a different one**." (the existing link, unchanged target). "Check spam" moves to R1’s lede |
| Lede "Enter the code we sent to {email}." | "Enter the code we sent to **{email}** to sign in." (B-39: the lede names the purpose; 2 lines at 390, measured on `Verify-help`) |
| — | Code-screen grammar (Part A §4.7, B-39): the alternatives are one switch line **below the commit**. Where a Verify state shows **Use a passkey instead**, it shares the **Can’t get the code?** line, passkey first, 24 apart (`.ri-switch.ri-code-alts`, edges.css §9); Tab order: the code, **Verify code**, the alternatives, then "Not your email? Use a different one" |
| Hit area | The link keeps 44 px on phones (`.ri-switch a::after`) |
| Behaviour | A plain navigation; the code, tries and countdown are server-side, so R1’s **Back to the code** returns to the same state. Focus on R1: its `h1` |

### 7.2 A5: Passkey sign-in ("Can’t use your passkey?")

Boards: `Passkey-signin`, `Passkey-signin-waiting`, `Passkey-signin-failed` (and wrappers).

| Now | Delta |
|---|---|
| Actions: **Sign in with a passkey** / **Try again**, status line, plain **Get a code by email instead** | Add a switch line 32 below the actions: link **Can’t use your passkey?** → `/signin/help?from=passkey&returnUrl=…` (R1 passkey variant, §3.1). The chip’s **Not you?** stays as is |
| Copy on `-failed` | Part A §2.1: "The request timed out or you canceled it." (was "cancelled") |

### 7.3 Other entry points

- Footer **Help** on every identity page → the Help article "Can’t sign in", whose first link is R1 (`/signin/help`, no return URL: R1’s "no sign-in in progress" variant).
- WP3.5 `Provider-link-code` already links **Can’t get the code?** → `Recover.dc.html`.
- Settings step-up screen C (AS §4.4) is the signed-in counterpart: it offers **Request account recovery** (R2 with the account prefilled), as §15.6 specifies (B-25).

### 7.4 Links between my boards and other WPs’

| From | To | Note |
|---|---|---|
| WP3.2 `Error-link` (E5, recovery variant) | `Recover-account` | Already linked |
| `Recover-account` Back to sign in, `Recover-completed`, `Recover-declined` note, `Recover-canceled` | `Main` | |
| `Email-recovery-alert` Cancel this request | `Recover-cancel` | B-10 |
| `Recover-cancel` Keep the request | `Recover-owner-view` | R6b, D-R10 |
| `Recover-owner-view` Cancel this request | `Recover-cancel` | |
| `Recover-review`, `Recover-approved` (and `-mobile` through its import) Cancel request | `Recover-review-cancel` | R6a |
| `Recover-review-cancel` Keep the request | `Recover-review` | |
| `Recover-expired` Start a new request; note | `Recover-account`; `Main` | R10c |
| `Recover` Back to the code | `#verify` (identity canvas) | |
| `Recover` Google / Microsoft | `#google`, `#microsoft` | Out of area; the busy state is WP3.5’s `Signin-redirecting` |
| `Security-recovered` | `Profile.dc.html#email`, sidebar links | From the `Security` copy |
| WP3.3 S6 email | `Email-recovery-alert` layout and masking | spec-settings §3.4 |

---

## 8. CSS (WP3.4 section of `edges.css`)

No new tokens, no colour pairs beyond Part A §6.3 and AS §7.1 (every colour is a token; the textarea and facts reuse measured pairs).

| Rule | suiss mapping | Reason |
|---|---|---|
| `.ri-rec-more` (hairline, 32 / 32), `.ri-rec-h2` (18/28/600), `.ri-rec-text` (16 Slate, max 400, balanced), `.ri-rec-more > [data-slot=button]` (24 above) | `Separator` + heading + `Button variant="outline" size="lg" block` | R1’s last-resort block: a second, quieter choice, not a fourth method |
| `.ri-rec-facts` (24 below), `[data-slot=item-group] > .ri-rec-fact` (column, 4 gap) | `ItemGroup variant="inset"` rendered as `dl`, `Item` as `div` holding `dt` (ItemTitle) + `dd` (ItemDescription) | A `dl` allows one `div` level (axe `definition-list`, `dlitem`), so the row stacks the pair itself; rows are 72 |
| (moved) textarea | `Textarea` | Merged by the PM into shared §2 as `textarea[data-slot=input]` (CSS-REQUESTS WP3.4-1, B-3); the board dropped `.ri-rec-textarea` |
| `.ri-switch.ri-rec-switch-2` (28 above) | — | `Verify-help`’s second switch line: grouped with the first, and 28 keeps both 46 px link hit areas from overlapping (24 measured 43) |
| `.ri-switch > a.ri-rec-back::after` (inline −8) | — | "Back" is 32 px of text; the hit area becomes 48 |
| `.ri-commit + .ri-rec-second` (12 above) | — | A second block button under the slot (R12 Keep the request) |
| `.ri-rec-mail`, `-env`, `-card`, `-title`, `-cta`, `-fine`, `-foot` (+ 44 px footer targets under 600 and on coarse pointers; forced-colours outline) | none (email drawing) | §4.3 |

**Requests filed:** CSS-REQUESTS **WP3.4-1** (shared textarea), **WP3.4-2** (punctuation after an address can wrap alone: make the domain half `inline` in ledes). The shared fixes the PM merged (inline slot icons, forced-colour badges, short local parts `ri-email-short`) apply to my boards with no change: `Security-recovered` carries `ri-email-short`.

---

## 9. Engineering handoff

### 9.1 `@repo/ui` and the identity app

| Part | Build with |
|---|---|
| Frame, head, step line, badge, slot, notes, switch, footer | Part A §6.2 identity components (`IdentityPage`, `StepLine`, `MascotBadge`, `StatusSlot`) |
| Fields | `field` + `input`; R5 textarea `textarea` with the input classes (`border-input`, filled, radius 10) |
| Code entry | `input-otp` as AS §8 row 20; share `useCodeTimer` |
| Buttons | `button` default / outline / plain, `lg` block; countdown and date buttons are the unavailable style |
| Facts | `item` (`ItemGroup variant="inset" render={<dl/>}`, `Item render={<div/>}`, `ItemTitle render={<dt/>}`, `ItemDescription render={<dd/>}`, no `ItemContent`) |
| Emails in prose | `EmailText variant="wrap"`; masked: a new `EmailText masked` prop (first character, `•••`, the domain; sr "j, hidden, at rivera.studio") |
| Alert (R14, expiry alerts) | `alert` with `role` "group" (or the R14 `section`), `tabIndex={-1}`, `aria-labelledby` the `AlertTitle` id and `aria-describedby` the `AlertDescription` id; focused on arrival, no ring (B-38) |
| Emails | MJML templates (§4.3), plain-text alternative, rendered server-side |

### 9.2 IdentityServer and the backend

| Piece | Detail |
|---|---|
| Routes | `/signin/help` (Razor page; reads the current sign-in request for "Back to the code" and the email); `/recover`, `/recover/email`, `/recover/code`, `/recover/details` (antiforgery, draft cookie); `/recover/status?token=` (R6–R10c), `/recover/status/cancel?token=` (R6a); `/recover/cancel?token=` (R12), `/recover/cancel/status?token=` (R6b), `/recover/canceled`; dispute token → support form |
| Return URL | Dropped from R2 on (Part A §3.3 rule 5). R8 and R13 build A1 URLs with `login_hint` and `returnUrl=/account/security` |
| Lookups | R2 uses the same lookup and L2 bucket as A1 |
| New-address code | Duende User Management OTP rules (8 Crockford, 5 min, 4 tries, 60 s), issued against the draft and the new address, not the account (the address isn’t on the account yet) |
| Data | `RecoveryRequest { id, accountId, oldEmailSnapshot, newEmail, newEmailConfirmedAt, answers (encrypted, deleted 30 days after close), requestIp, requestCity, userAgent, createdAt, status (open, approved, completed, declined, canceledByOwner, canceledByRequester, expired, undone), approvedAt, changesAt, expiresAt (createdAt + 7 days, B-36), decidedBy, internalNote, closedAt, statusTokenHash, cancelTokenHash, disputeTokenHash }`; `Account.recoveryWaitUntil`, `Account.recentlyRecoveredUntil` |
| Completion job | At `changesAt`: re-check (still approved, account exists, new address still free), set the email, set `recentlyRecoveredUntil` = +7 days, remove every server-side session for the subject (`ISessionManagementService.RemoveSessionsAsync`) and revoke refresh tokens (keep consent grants: connected apps stay, Q14), keep passkeys and provider links, write the activity entry, send #5 and #6. At the next sign-in call `PublicKeyCredential.signalCurrentUserDetails()` so passkey managers show the new address |
| Step-up | The `auth_time` ≤ 10 min check is skipped (step-up always) while `recentlyRecoveredUntil` is in the future, server-side on every gated endpoint, including link and unlink (spec-settings) |
| Expiry job (B-36) | At `expiresAt`, a request still `open` becomes `expired`: set `closedAt`, release the reserved new address, send #16 and #17, write no wait (`recoveryWaitUntil` unchanged). An `approved` request never expires; its completion job runs at `changesAt`. Idempotent, like the completion job |
| Reserved address | The confirmed new address is reserved while the request is open (Q-R8) |
| Undo | Support action: restore `oldEmailSnapshot`, remove sessions again, activity entry, #13 and #14 |
| Limits | L1, L2, L5, L7 as §2.4; `Retry-After` and next-allowed times returned so buttons count down from server time |

---

## 10. Boards

All in `.design/edges/project/`, board `<title>` = §5.2 title, `$preview` = the size below. Positions are `canvas.json`’s plus CANVAS-CHANGES WP3.3-4 and WP3.4-5.

| File | Board title | Size | Kind | State |
|---|---|---|---|---|
| `Recover` | Can’t get the code, signed out | **1440×1100** | B | R1 from Verify |
| `Recover-mobile` | Can’t get the code, mobile | 390×844 | W | R1 at 390 |
| `Recover-dark` | Can’t get the code, dark | **1440×1100** | W | R1 dark |
| `Verify-help` (new) | Check your email, with Can’t get the code | 1440×900 | B | Verify in this frame with the A6 entry (§7.1) |
| `Recover-account` | Recovery request, which account | 1440×900 | B | R2, email prefilled |
| `Recover-email` | Recovery request, new email | 1440×900 | B | R3 empty |
| `Recover-code` | Recovery request, enter the code | 1440×900 | B | R4, focus drawn on the active slot |
| `Recover-details` | Recovery request, details for support | **1440×1400** | B | R5 empty |
| `Recover-details-mobile` | Recovery request, details for support, mobile | 390×844 | W | R5 at 390 |
| `Recover-limited` | Recovery request, too many requests | **1440×1300** | F (of `Recover-details`) | R5 filled, L5 per network |
| `Recover-limited-dark` (new) | Recovery request, too many requests, dark | 1440×1300 | W | R5 limited dark (B-21) |
| `Recover-review` | Recovery request, under review | 1440×900 | B | R6 |
| `Recover-review-cancel` (new, revision round) | Cancel the recovery request, from the requester | 1440×900 | B (status-page family, `Recover-cancel`’s structure) | R6a: Cancel your request?, Keep the request |
| `Recover-approved` | Recovery request, approved, waiting 3 days | 1440×1000 | B | R7 (title: COPY-26) |
| `Recover-approved-mobile` | Recovery request, approved, waiting 3 days, mobile | 390×844 | W | R7 at 390 |
| `Recover-completed` | Recovery request, email changed | 1440×900 | B | R8 |
| `Recover-declined` | Recovery request, declined | 1440×900 | B | R9 |
| `Recover-expired` (new, revision round) | Recovery request, expired | 1440×900 | B (status-page family, `Recover-declined`’s structure) | R10c (B-36) |
| `Email-recovery-alert` | Email to the old address, someone asked to change it | **720×1100** | B (email) | R11 (#2) |
| `Recover-cancel` | Cancel the recovery request, from the old address | 1440×900 | B | R12 under review |
| `Recover-owner-view` (new, revision round) | Recovery request, under review, owner’s view | 1440×900 | B (`Recover-review`’s structure) | R6b: masked new email, no Account row |
| `Recover-canceled` | Recovery request canceled by the account owner | 1440×900 | B | R13 |
| `Security-recovered` | Sign-in and security, recently recovered | **1440×1800** | F (of `Security`, re-copied from WP3.3’s Phase 4 `Security` of 18:53) | R14 on November 8 |

**Full copies.** `Recover-limited` is a copy of `Recover-details`; `Security-recovered` is WP3.1’s `Security` (unchanged by WP3.3, P-6) with the R14 edits and its own sidebar copy. Re-copy, never hand-patch, if a base changes.

---

## 11. Open questions, with defaults

| # | Question | Default | Confirm |
|---|---|---|---|
| Q-R1 | A sign-in to the account while a request is open: cancel it, or alert in settings? | Neither for now: support sees the sign-in and declines (§5.1 step 4); a settings alert for an open request is a later item | Security, product |
| Q-R2 | How long do we keep R5 answers? | Deleted 30 days after the request closes; the request record (without answers) kept as security history | Legal |
| Q-R3 | Does the old address get the requester’s browser and city? | Yes: it is the evidence the owner needs (as Google and Microsoft alerts do) | Privacy |
| Q-R4 | Time zone in emails | The account’s setting, named; else UTC | Engineering |
| Q-R5 | Does a decline say why? | One reason for every decline; support’s note stays internal, so no account detail leaks to a requester | Support |
| Q-R6 | Can support ask for more before deciding? | Yes, by email from the desk; R6 doesn’t change | Support |
| Q-R7 | Second agent for risky approvals and every undo | Yes when the account signed in after the request, and for undo | Support, security |
| Q-R8 | Someone signs up with the confirmed new address during the request | The address is reserved: sign-up gives A2’s "already has a Richie account" | Engineering |
| Q-R9 | Should Sign in (A1) link to R1 directly? | No: A1 → Send code → Verify → Can’t get the code? is two taps and keeps A1 simple; Help covers people who know their mailbox is gone | Product |
| Q-R10 | Support form for a signed-out person | Asks for a contact email (it may be the new address) and carries the topic and any token | Support |

---

## 12. Testing

### 12.1 Commands (Part A §5.4)

```
node .design/qa-rig/build.mjs .design/edges/project .design/edges/out-wp34 .design/edges/blobs
lsof -i :8814                                   # printed nothing
node .design/qa-rig/run.mjs --out .design/edges/out-wp34 --port 8814 --boards <the 18> --results .design/edges/qa/wp34
node .design/qa-rig/run.mjs --out .design/edges/out-wp34-dark --port 8814 --boards <the 18> --results .design/edges/qa/wp34/dark   # .dark on every <body>
node .design/qa-rig/run.mjs --out .design/edges/out-wp34 --port 8814 --forced-colors --results .design/edges/qa/wp34/forced \
     --boards Recover,Recover-code,Recover-declined,Recover-approved,Security-recovered,Email-recovery-alert
python3 -I .design/canvas-tools/check-icons.py .design/edges/icons <my boards>
node .design/qa-rig/drift.mjs .design/edges/project
node .design/wp3-1/measure-status.mjs .design/edges/qa/wp34/slot-strings.json 460,358,343,288
```

### 12.2 Results (2026-11-05)

| Run | Boards × viewports × schemes | Blockers | Majors | Minors |
|---|---|---|---|---|
| Light | 18 × 10 × 2 | 0 | 0 | 75 |
| Forced-dark build | 18 × 10 × 2 | 0 | 0 | 75 |
| Forced colours | 6 × 10 × 2 | 0 | 74 (all axe `color-contrast`, discarded below) | 40 |

- **check-icons:** 0 bad (24 icons; `arrow-square-out` added to `.design/edges/icons/` from `@phosphor-icons/core@2.1.1`). **drift:** one group, `nav.ra-side-nav` (`Profile-email-changed` WP3.3 and `Security-recovered`: the new address, intended, D-R9).
- **Fixed during the run:** facts groups first nested `div > div` inside `dl` (axe `definition-list`, `dlitem`: 33 majors) → rows hold `dt` + `dd` directly; `aria-labelledby` on the R14 alert `div` (axe `aria-prohibited-attr`) → removed; "Back" links 39 px wide → 48 (`.ri-rec-back`); a period wrapping alone after the masked address on `Recover-cancel` at 320 → lede reworded (CSS-REQUESTS WP3.4-2); `Recover-completed` lede 3 lines at 390 → 2.

**False positives discarded** (Part A §5.4, how-we-design §5, rig README):

| Finding | Count | Why it is noise |
|---|---|---|
| Footer Help, Privacy, Terms at 24 px on fine pointers ≥768 (identity and email footers) | 51 | By design: 44 below 768 (email: below 600) and on coarse pointers |
| Inline links under 44 / 24 ("Contact support", "Sign in with it", "passkeys", "devices", the link fallback URL) | 13 | Inline in text, WCAG 2.5.8 exempt |
| Sidebar items at 36 px, "Back to Richie" (`Security-recovered`) | 8 | Known settings minor |
| axe "needs review" contrast: `#code` (transparent OTP input), `#limit-status` (two paragraphs share one grid cell), **Link** over Mist | 3 | Known; the slot text is Ink on Paper (16.83:1) |
| Forced colours: axe `color-contrast` (author colours against the forced Canvas) | 74 | Tool artefact: the same run on WP3.1’s `Main`, `Signedout`, `Security` reports 12 / 8 / 20 of the same; the screenshots show system colours throughout |

### 12.3 Status strings (`measure-status.mjs`, lines at 460 / 358 / 343 / 288, Inter then SF)

All 29 fit one line at 358 and two at 288. The ones that needed rewriting (D-R4): "We couldn’t send your request. Check your connection." (SF 1/2/2/2) → "We couldn’t send it. Check your connection." (1/1/1/2); "…cancel your request…" and "…cancel the request…" (2 at 358) → "cancel it"; "We couldn’t look up your account…" (2 at 358) → "We couldn’t continue…"; "We couldn’t use your passkey because you’re offline." (SF 2 at 358) → "…use a passkey…". Data: `.design/edges/qa/wp34/slot-measure.json`.

### 12.4 Looked at by eye

390 light (every board), 1440 light, 320 (`Recover`, `Recover-cancel`, `Recover-details`), 1280 × 540, 390 dark in the forced-dark build, the `-dark` wrapper at 1440, forced colours at 390. The SVG mascot and the Google image render; badges sit on the shoulder in both themes; the code row fits at 320; the email reads at 390 and 768. Measured heights at 1440 (rounded up for §10): `Recover` 967, R2–R4 900, `Recover-details` 1306, `Recover-limited` 1286, status pages 900, `Email-recovery-alert` 1036 at 720, `Security-recovered` 1768.

---

## 13. Changelog (see also §14)

- **Revision 1 (2026-11-05):** pages R1–R14, every email (R11 drawn, 13 more as copy), support procedure, state matrix, identity-canvas deltas (A5, A6), CSS, engineering handoff; 18 boards; rig 0 blockers, 0 majors. Folded in the PM’s notes from WP3.2 and WP3.3: E5 links to `Recover-account`; a used cancel link shows R13; the 30-day undo promise matches E5 and S6; `Security` is final, so `Security-recovered`’s copy stands and the short-address gap is fixed by the shared rule, not a workaround.

---

## 14. Fix log (Phase 4)

Findings from `qa/p4-rig.md`, `p4-a11y.md`, `p4-copy.md`, `p4-coverage.md` owned by WP3.4, and the PM rulings that land here (Part A Part B). Every change is also written into the sections above.

| ID | Status | What changed |
|---|---|---|
| RIG-01 (blocker) | **Fixed** | Labels shortened to fit 256 px at 16 px (measured Inter / SF): `Recover-declined` "New request on November 12" (192 / 214; was 264 in SF), `Recover-limited` "Send request at 9:15 AM" (159 / 178) with sr ", tomorrow" and the time in the note. Copy-only labels checked the same way ("New request on November 13", "Continue at 9:15 AM", "Send a new code in 0:41"). The shared block-button wrap (edges.css §9) is the fallback. Text-spacing check at 320 and 1440, light and dark: no overflow and no label outside its button (§12.5) |
| RIG-06 | **Fixed (documented)** | §3.12 and D-R9: the recovered account has one passkey (the lost Pixel 8; with the MacBook passkey recovery wasn’t needed), one device (completion signs out every device), Google not linked (with Google, J7 applies), four activity rows. Its section heights therefore differ from `Security` by design; it is not a layout-shift reference for `Security`. Re-copied from WP3.3’s changed `Security` (18:53: `wrap-all` titles, quoted passkey names, `ri-set-2l`), size still 1440 × 1800 (measured 1788) |
| A11Y-04 (major) | **Fixed** | B-29: the 30-minute draft and the 30-minute new-address confirmation slide with activity (any input, debounced keep-alive at most once a minute, or request); 2 minutes before, spec-edge’s time limit warning ("Still there? This page times out in 2:00." + **Keep going**) on R2–R5. Expiry copy says "without activity" (§2.4, §3.2–3.5, §15.3) |
| A11Y-07 | **Fixed** | Short visible failure lines keep the noun for assistive tech (`it` `aria-hidden` + sr "your request" / "the request"); D-R4 |
| A11Y-09 | **Fixed** | Document titles carry the step: "Recover your account, step 1 of 3 – Richie", "Choose a new email, step 2 of 3 – Richie", "Check your new email, step 2 of 3 – Richie", "Tell us about your account, step 3 of 3 – Richie" |
| A11Y-10 | **Fixed** | R6/R7 **Cancel request** is a link (`<a data-slot="button">`) to the requester’s confirm page; R6/R7 lose their slot (nothing on them reports); failures live on the confirm |
| A11Y-11 | **Fixed** | `Security-recovered`’s notice is `<section data-slot="alert" aria-labelledby="recovered-title">`, a named region heading navigation and landmarks reach |
| COPY-07 | **Fixed** | Google row "Not linked. Works with any Google account." (spec-settings) with `ri-set-2l` |
| COPY-10 | **Fixed in substance** | AT hears the noun (A11Y-07). The visible noun isn’t used: "send your request", "cancel your request", "cancel the request" and "check that email" all measure 2 lines at 358 (SF), and §4.6 rules out a taller slot on one page. Won’t move R2 to "check that email" for the same reason |
| COPY-12 | **Fixed** | "Richie support usually replies within 2 business days." verbatim on R1, R2, R6 and email #1; support’s written sign-off of every number added as Q-R11 |
| COPY-16 | **Fixed** | Draft-expired alert: **This took too long** / "After 30 minutes without activity we clear what you typed, except your email. Start again from here." |
| COPY-18 | **Fixed** | Email footer: Richie, Help, Privacy, **Terms** (board and §4) |
| COPY-23 | **Fixed** | R2 lede "Enter the email address you can’t get anymore." |
| COPY-26 | **Fixed** | Board titles "Recovery request, approved, waiting 3 days" (+ ", mobile"); CANVAS-CHANGES WP3.4-10 |
| COPY-31 | **Won’t fix (by design)** | The R14 alert keeps "on November 8" while the activity row says "Today at 4:10 PM": the alert stays for 7 days, so it must be a date; the activity list follows AS’s relative-date rule (B-16) |
| COV-03 (major) | **Fixed** | R1 `from=link` variant: Back to the code → P6 as left, Microsoft hidden; P6 added to R1’s entry points (§3.1). Part A §3.1’s R1 row is the PM’s to sync (B-26) |
| COV-05 (major) | **Fixed** | R2’s open-request error says how long ("open for up to 7 days") and offers **Contact support** (`recovery-not-me`); support may decline the open request and let the new one through (§5.1) |
| COV-06 (major) | **Fixed** | Support on every early page: R1 note under the recovery button (drawn), R4 note (drawn), R2/R3 limit notes and closed-door field errors (copy), R5 limited (drawn) |
| COV-07 (major) | **Fixed** | R2 deleted-account answer matches A1: days 30–60 say what `Signin-deleted` says (deleted, data downloadable), no date; after day 60 "No Richie account uses this email." |
| COV-12 (major) | **Fixed** | New board **`Verify-help`** (1440 × 900): sign-in Verify in this frame with "Can’t get the code?" and "Not your email? Use a different one."; R1 Back to the code links to it (no more `#verify`) |
| COV-16 | **Fixed** | R2’s L2 slot line is E9’s string verbatim (one bucket, one string), 0:00 line "You can try again now."; my own L5 lines follow B-12 |
| COV-22 | **Fixed** | Forms wired for click-through (canvas `action` → next board, `method="get"`; the app POSTs): R2 → R3 → R4 → R5 → R6, R12 → R13; R6/R7 Cancel request → `Recover-cancel` |
| COV-23 | **Fixed** | Completion: R8 only from the stored status; "changing now" and "blocked" variants and email #15 (§15.5) |
| COV-24 | **Fixed** | Token table (§15.4) |
| COV-31 | **Fixed** | After a passkey failure R1’s note becomes "No passkey for Richie on this device? Choose your phone when your browser asks, or use another way." (WebAuthn doesn’t say whether it was canceled or found nothing) |
| COV-33 | **Fixed (measured, accepted)** | Copy-only variants that drop a row or a button, measured on the drawn layouts at 1440 / 390: R6 owner view (no Account row) moves the action 542 → 466 / 546 → 470; the requester’s confirm on `Recover-cancel` (no From row, no note) 542 → 466; R10b (no second button) moves the note 414 → 358. Each variant is its own page, so B-32 applies (pages share the mascot and `h1`, not the primary’s y) |
| COV-38 | **Fixed** | No future sample date: R2’s deleted-account line carries no date |
| B-6 | Applied | R1 failure lines "We couldn’t sign you in…" for passkey, Google, Microsoft |
| B-7 | Applied | One fine print for emails to the account’s address. Emails to the **new** address before completion keep "You’re getting this because someone entered this address in a Richie recovery request and confirmed it with a code.": that address isn’t on the account yet, so "a security change on your Richie account" would be untrue to its reader |
| B-8 | Applied | Zones on status-page facts (R6, R7, R12) and in every email (§2.5) |
| B-10 | Applied | Email board and #2, #4: **Cancel this request**; #6: **Contact support**; D-R12 superseded |
| B-12 | Applied | "This network has made 5 recovery requests today." / "This email has been used for 3 recovery requests today." (the second is a field error only: 2 lines at 358) |
| B-18 | Applied | R1’s methods return to `/account/profile#email` with Change email open; the note says so (D-R16 extends it to the passkey) |
| B-19 | Applied | §15.1 Support handoff |
| B-20 | Applied | R4 "New code sent…" stays until the first character |
| B-21 | Applied | New board **`Recover-limited-dark`** (1440 × 1300) |
| B-24 | Applied | §15.2 Help article minimum content |
| B-25 | Applied | §15.6 settings step-up screen C delta; R2 `from=settings` |
| B-27 | Applied | Countdown buttons: digits `aria-hidden`, sr ", available at {time}" (R2, R3, R4 locked) |
| B-28 | Applied | Busy text written once into the slot’s polite line on R1–R5, R12 |
| B-31 | Applied | R4: arrival focus in the input; failed submit → "Error: " title prefix, focus stays with the text selected |

**New boards (CANVAS-CHANGES WP3.4-8, -9):** `Verify-help` 1440 × 900 (row "recover", x 3510), `Recover-limited-dark` 1440 × 1300 (row "request", x 8070). **Resized:** `Recover` and `Recover-dark` 1000 → 1100 (WP3.4-6, measured 1031), which moves rows "request", "status", "owner" by +100 (WP3.4-7).

---

**COV-14 (PM-applied after WP4.6, REG-01):** fixed. R1’s Google and Microsoft buttons use spec-sso §3.1’s mechanism (form submit + `fetch`) and B-6’s line "We couldn’t sign you in…".

## 15. Phase 4 additions

### 15.1 Support handoff (B-19: the contract every "Contact support" link uses)

The support form belongs to Richie support’s help site; identity owns this contract.

| Part | Rule |
|---|---|
| URL | `{help site}/contact` with query parameters below; always `target="_blank" rel="noopener"`, accessible name suffix ", opens in a new tab"; block buttons carry the `ArrowSquareOut` icon (inline links don’t) |
| `topic` | One of the fixed list below (required) |
| `ref` | The E4 reference (`9F2C-41AB-7D03`), when the page has one |
| `request` | An opaque recovery token (status or dispute), when the page has one; the help site exchanges it server-side for the request and never shows it |
| `from` | The page ID (`R1`, `R2`, `E4`, `P3`, `S8`, …) for routing and reporting |
| Signed out | A **required** contact email ("Email we can reach you at", hint "Use an address you can get now."), a required message ("What happened?", up to 2,000 characters). Nothing from identity is prefilled into the URL (no addresses in URLs) |
| Signed in | The contact email prefilled from the account and the account attached (short-lived signed account token, not the address), the message field |
| Confirmation | A page "We got your message" with "Richie support usually replies within 2 business days." and the same sentence in a confirmation email to the contact address. Richie support never asks for an email code |

**Topics (all four areas):**

| `topic` | Label on the form | Used by |
|---|---|---|
| `cant-sign-in` | I can’t sign in | R1, R2 no-account error, footer Help, the Help article |
| `cant-get-code` | I can’t get my email code | Verify (help site links), settings step-up screen C’s list item (§15.6) |
| `recovery-request` | My recovery request | R2 wait error and the open-request error’s "Yours?" line, R3 limit, R4 note, R5 limited, R6, R7, R9, R10a, R10c, E5’s "Lost the email?" note, email #9, #14, #15, #16 |
| `recovery-not-me` | Recovery I didn’t ask for | R2 open-request error, R12 "already changed", R14 alert, email #6, E5 owner variant |
| `email-changed-not-me` | Someone changed my Richie email | S6 email (spec-settings §3.4, its **This wasn’t me** button) |
| `signin-changed-not-me` | Someone changed how I sign in | The settings security emails: passkey added, passkey removed, provider linked, provider unlinked (spec-settings §5.2, B-35) |
| `provider` | Signing in with Google or Microsoft | P3, P7, settings link and unlink errors (spec-sso, spec-settings §4.2: `topic=provider&ref=…`) |
| `error` | Something went wrong (with `ref`) | E4, P3’s reference |
| `deleted-account` | My deleted account | `Signin-deleted`, export pages (AS §4.8) |
| `other` | Something else | Footer Help elsewhere |

### 15.2 The Help article "Can’t sign in" (B-24, owned by support content)

Minimum content, in this order: (1) **Didn’t get the code?** Check spam or junk; codes expire after 5 minutes and allow 4 tries; send a new one after a minute. (2) **Other ways in:** a passkey (including one on your phone, through the browser’s prompt), Google or Microsoft if linked; link to R1 (`/signin/help`). (3) **Lost access to your email?** What a recovery request does: Richie support checks it’s you ("Richie support usually replies within 2 business days."), the old address is told and can cancel, the change happens 3 days after approval, every device is signed out, and for 7 days you confirm it’s you before sensitive changes; link **Start a recovery request** (R2). (4) "Richie support never asks for your email code." (5) **Contact support** (`topic=cant-sign-in`).

### 15.3 Time limits (B-29)

| Limit | Slides with | Warning | After |
|---|---|---|---|
| Recovery draft (R2–R5), 30 min | Any input (keep-alive at most once a minute while a field changes) or any request | spec-edge’s time limit warning, 2 minutes before: "Still there? This page times out in 2:00." + **Keep going** (renews; up to 10 times) | R2 with "This took too long" |
| New-address confirmation, 30 min | The same activity on R5 | The same warning | R3 with "Confirm your new email again" |
| Email code, 5 min | — (the authentication factor’s lifetime, essential) | — | "This code has expired." + Send a new code |

### 15.4 Tokens (COV-24)

| Token | Issued | Sent in | Opens | Reusable | Lifetime | Authorises |
|---|---|---|---|---|---|---|
| Status | Submit | Every email to the new address | `/recover/status?token=` (R6–R10c); `/recover/status/cancel?token=` (R6a, the requester’s confirm; the POST also needs antiforgery; once the request is closed it shows the closed status page) | Yes | Until 30 days after the request closes (completed, declined, canceled or expired) | Viewing this request with both addresses; canceling as the requester |
| Cancel | Submit | #2, #4 to the old address | `/recover/cancel?token=` (R12 and its variants; POST cancels); `/recover/cancel/status?token=` (R6b, the owner’s view, masked). After a decline, an expiry or a requester cancel both show R13 "Nothing changed" | Yes | Same | Viewing with the new address masked; canceling as the owner |
| Dispute | Completion | #6; R12 "already changed" | The support form’s `request` | Yes | 30 days after completion | Opening a `recovery-not-me` case on this request |
| Draft | R2 | A `HttpOnly`, `SameSite=Lax` cookie | R2–R5 | — | 30 min sliding | The draft |

Recovery tokens are never "used up"; they expire. Wrong, expired or tampered tokens give E5 (410), identical for every cause.

### 15.5 Completion delayed or blocked (COV-23)

| Case | Page (`Recover-review` layout, clock badge) | Emails |
|---|---|---|
| `changes_at` passed, job not done yet | `h1` **Your email is changing now**; lede "This usually takes a minute. This page updates when it’s done." Reloads every 30 s for up to 10 minutes, then the blocked case | — |
| Re-check fails (account gone for good, address taken, support flag) | `h1` **We need to check something**; lede "We couldn’t finish changing your email. Richie support usually replies within 2 business days." Facts unchanged; **Contact support** (`recovery-request`) | #15 to the new address: subject "We couldn’t finish your Richie recovery", body "Hi, we couldn’t finish changing your account’s email, so nothing has changed yet. Richie support will look into it and email you here. Richie support usually replies within 2 business days.", button **See your request**. Nothing to the old address (nothing changed) |

### 15.6 Settings handoff: step-up screen C (B-25)

Settings canvas boards `Stepup-cant-get-code`, `Stepup-cant-get-code-mobile` and the screen C ghosts in `Stepup*`:

| Now | Delta |
|---|---|
| Paragraph "Can’t get email at **…** any more? Contact Richie support. We’ll check it’s you and help you change your email." | "Can’t get email at **jamie.rivera@northwind.coffee** anymore? Request account recovery. Richie support checks it’s you, then changes your email." |
| Footer **Back**, **Contact support** (new tab) | **Back** (outline), **Request account recovery** (default) → `/recover?from=settings` in the same tab: step-up closes and the action is canceled; R2 opens with the account’s email prefilled and "Back to Sign-in and security" |
| Policy row (AS §4.4) | The same safeguards as signed out: old-address alert, 72-hour wait, every device signed out at completion |

### 15.7 Results (Phase 4)

| Run | Boards | Blockers | Majors | Minors |
|---|---|---|---|---|
| Light | 20 (18 + `Verify-help`, `Recover-limited-dark`) × 10 viewports × 2 schemes | 0 | 0 | 89 |
| `BOARD_THEME=dark` build | same | 0 | 0 | 89 |
| Text spacing (1.4.12) at 320 and 1440, light and dark | 20 | no overflow; no label outside its button | | |

Minors are the known ones (§12.2: footer links at 24 on fine pointers, inline links, sidebar items, three axe "needs review"). The text-spacing script also prints two non-findings: the visually hidden ", opens in a new tab" suffix inside `Recover-declined`’s **Contact support** (a clipped 1 px `ra-sr` span the range includes) and the hidden skip link on `Security-recovered` (shown only on focus). Icons: 0 bad. Drift: the one intended sidebar group (D-R9). New slot strings measured one line at 358 and two at 288 (§12.3 method): "We couldn’t sign you in because you’re offline." / "…Check your connection." / "…Try again in a moment.", "This network has made 5 recovery requests today.", "You can try again now.", "Still there? This page times out in 2:00."

### 15.8 Open question added

| # | Question | Default | Confirm |
|---|---|---|---|
| Q-R11 | Written sign-off on every number in recovery copy: 2 business days (reply), 3 days (wait), 7 days (decline wait, recently recovered), 30 days (undo, link lifetimes), 30 minutes (draft) | Ship the numbers above; any change is a text swap | Support (COPY-12) |

- **Revision 2 (2026-11-05, Phase 4):** §14 fix log and §15; boards `Verify-help` and `Recover-limited-dark` added; `Recover`, `Recover-dark` 1100; `Security-recovered` re-copied from WP3.3’s `Security`; rig 0 blockers, 0 majors.

---

## Revision round (Phase 5)

After the grade (`evaluation.md`) and rulings B-34 to B-43. Every change is written into the sections above; superseded sentences were deleted, not annotated.

1. **B-36, expired request:** R10c in §3.9 (h1 "We couldn’t finish your request in time", lede, exclamation badge, **Start a new request** with no wait, **Contact support** `recovery-request`, states, status-token behaviour, IA row); emails #16 (new) and #17 (old) in §4.1; §2.1, §2.3 row 6f, §2.4, §5.1, §5.2, §6, §9.2 (`expired`, `expiresAt`, expiry job), §10, §15.1, §15.4. New board `Recover-expired`.
2. **R6a:** drawn as `Recover-review-cancel` with its own copy table, states and rationale (§3.6); **Cancel request** on `Recover-review` and `Recover-approved` (and `-mobile` through its import) links to it; §6, §7.4, §10.
3. **R6b:** drawn as `Recover-owner-view` in the owner’s words ("We’re reviewing this request", **Cancel this request**), masked new email, no Account row, with its approved and closed variants (§3.6); **Keep the request** on `Recover-cancel` links to it; D-R10, §3.10, §7.4, §10.
4. **B-34:** R3’s L1 slot is "You’ve asked for 5 codes in an hour. Try again in 41:12."; R2’s L2 slot is "This network has made 30 attempts in an hour." and the old "This network tried to sign in 30 times in an hour." is gone. With its countdown the L2 line measures 2 lines at 358 (Inter and SF), so the slot carries the line alone and the countdown stays on the commit: flagged for the PM, no wording invented.
5. **B-35, B-36:** `signin-changed-not-me` in the §15.1 topic list; `recovery-request` gains R10c, #16 and the new "Yours?" line; B-36 in §2.4 (open request: at most 7 days), §2.3 and §6 (states), §15.4 (tokens).
6. **B-38:** R2’s and R3’s expiry alerts and R14 focus the alert container, labelled by the title and described by the description (§3.2, §3.5, §3.12, §6, §9.1); `Security-recovered`’s alert `section` has `tabindex="-1"` and `aria-describedby`.
7. **B-39:** R4’s lede "…to confirm your new email." and its grammar line (§3.4, `Recover-code`); `Verify-help`’s lede "…to sign in." and the §7.1 delta (alternatives below the commit).
8. **Time limit warning:** §3.2, §3.3, §3.5 and §6 point at spec-edge’s `Signin-timeout-warning`.
9. No change in this file (E2 is spec-edge’s).
10. **R2’s open-request error** adds "Yours? Use the link in your latest status email, or contact support to get it again." (§3.2; no board draws that error).
11. **R5:** `maxlength` removed from `Recover-details` and `Recover-limited`; lengths are checked on submit with the "Too long" errors (§3.5).
12. **Stale text deleted:** §2.2 step-up (B-18), §7.3 screen C (B-25), every "This wasn’t me" on the cancel path (D-R12, §2.1, §2.3, §2.4, §4.3, §5.1, §7.4); §4.3 now states the email CTA once: **Cancel this request**, a pill (22 px radius in MJML).
13. No change in this file.
14. No drawer or action sheet in recovery, so no DOM-order row to add.

**Boards:** new `Recover-expired`, `Recover-review-cancel`, `Recover-owner-view` (1440 × 900 each, CANVAS-CHANGES REV-1 to REV-3); changed `Recover-review`, `Recover-approved`, `Recover-cancel`, `Recover-code`, `Verify-help`, `Recover-details`, `Recover-limited`, `Security-recovered`.

**Tests (revision round, 2026-11-05).** Rig on the 52 changed, new and base boards (`Main`, `Security` and every copy), port 8831, 10 viewports: light build (`.design/edges/qa/rev/light`) **0 blockers, 0 majors, 396 minors**; `BOARD_THEME=dark` build with `--schemes dark` (`.design/edges/qa/rev/dark`) **0 / 0 / 396**. Every minor is a known class: 158 targets on inert pages behind overlays, 105 footer links at 24 px on fine pointers, 80 sidebar items at 36 px, 30 axe "needs review" contrast items (scrims, transparent code inputs, **Link** on Mist), 23 inline links. Text spacing (1.4.12, 320 and 1440, both builds): only the known false positives (the skip link; the sr-only ", opens in a new tab" in **Contact support**). Block labels at 320: 90 block buttons, widest 226 px (≤ 256), none taller than 44. Drift: the intended sidebar copies only. Icons: 0 bad. Every `href`, form `action` and `dc-import` target exists.
