# Richie identity edges: independent evaluation

Evaluated 2026-11-05 by an independent reviewer who wasn't involved in the work. It covers the 80 boards in `.design/edges/project/` (the same content as the canvas https://claude.ai/artifact/6Xe7bnSdGHE3xvVR5LeMge) and the five specs in this folder (`spec.md` Parts A and B, `spec-edge.md`, `spec-settings.md`, `spec-recovery.md`, `spec-sso.md`). It grades against the product owner's six shippable criteria, using a production ship bar for an identity product: **8** means a team could build it now with only minor follow-ups, and **9 or higher** is exemplary. Verdicts: **Ship** is 8 or above with no must-fix, **Ship with fixes** is 8 or above with must-fixes listed, and **No-ship** is below 8. Every point below 10 traces to an issue in that unit's table or to a cross-cutting issue it cites.

## Summary

| Unit | Name | Score | Verdict | Confidence |
|---|---|---|---|---|
| E1 | Sign-in timed out (Timeout, -mobile, -dark) | 8.6 | Ship | High |
| E2 | Session expired (Session-expired, -mobile; E3 alert as copy) | 8.0 | Ship | Medium-high |
| E4 | Error with reference (Error, -mobile, -dark) | 8.6 | Ship | High |
| E5 | Link no longer works (Error-link) | 8.4 | Ship | High |
| E6 | Page not found (Error-notfound) | 8.8 | Ship | High |
| E7/E8 | Offline (Signin-offline, -dark; Offline) | 8.5 | Ship | High |
| E9/E10 | Too many attempts (Signin-limited, -dark; Limited, -dark) | 8.0 | Ship with fixes | High |
| E11 | Sign-out prompt (Signedout-prompt) | 8.7 | Ship | High |
| E13 | Signed out (Signedout, -mobile, -dark, Signedout-client) | 8.8 | Ship | High |
| E14 | Signed out everywhere (Signedout-everywhere) | 8.6 | Ship | High |
| S-A | Passkey management (Security, -mobile, -dark, -passkey-added, -passkey-limit, -rename*, -remove-failed, -load-error*) | 8.6 | Ship | High |
| S-B | Email settings (Profile, Profile-email-*) | 8.5 | Ship | Medium-high |
| S-C | Google and Microsoft in settings (Security section, -linked, -link-error, -unlink) | 8.3 | Ship | Medium |
| R-A | Can't get the code (Recover, -mobile, -dark, Verify-help) | 8.6 | Ship | High |
| R-B | Recovery request steps (Recover-account, -email, -code, -details*, -limited*) | 8.1 | Ship with fixes | Medium-high |
| R-C | Request status pages (Recover-review, -approved*, -completed, -declined) | 7.9 | No-ship | Medium-high |
| R-D | Old-address alert and cancel (Email-recovery-alert, Recover-cancel, Recover-canceled) | 8.3 | Ship | Medium-high |
| R-E | Recently recovered (Security-recovered) | 8.6 | Ship | High |
| P-A | Sign in with Google and Microsoft (Main*, Signin-redirecting, Signin-provider-*) | 8.2 | Ship with fixes | Medium-high |
| P-B | Welcome back with a provider (Signin-remembered-provider) | 8.3 | Ship | Medium-high |
| P-C | Create account with a provider (Signup*, Signup-finish*) | 8.4 | Ship | Medium-high |
| P-D | Provider dead ends (Provider-no-account, -no-email, -not-linked) | 8.6 | Ship | High |
| P-E | Link a provider at sign-in (Provider-link*, Provider-link-code) | 8.4 | Ship | Medium-high |
| **Set** | **Overall** | **8.1** | **Ship with fixes** | Medium-high |

19 units Ship, 3 Ship with fixes, 1 No-ship. Mean unit score 8.43. Issues logged in the units: 4 must-fix, 30 should-fix and 27 nice-to-have. Several repeat across units because they share one cause; the cross-cutting list collapses them.

**The five issues that matter most**

1. **must-fix**: The precedence rule is circular and the last rulings never reached the domain specs. Part A says it wins over domain specs; Part B and Part A §3.1 say a domain spec's departure wins; spec-settings and spec-sso say Part A wins. B-34, B-35 and B-36 exist only in Part B, so two L1 lines and three L2/L6 lines are in play, and a drawn board (`Signin-limited`) contradicts the binding ruling.
2. **must-fix**: B-36's expired recovery request is a new end state with no page, actions, badge or emails.
3. **should-fix**: First-paint page alerts focus only the title paragraph. The description, which says what to do next, and P3's reference are not announced.
4. **should-fix**: The WCAG 2.2.1 time-limit warning adds an inline Keep going control inside the status slot on every timed page, and it's never drawn.
5. **should-fix**: Code-entry screens disagree on where the alternatives sit and on Tab order, and P6 drops its purpose from the head.

Unit tables use these columns: **Severity** (must-fix, should-fix, nice-to-have), **Issue**, **Evidence** (board, element, measurement or spec section), **Principle** (WCAG SC, heuristic or shippable criterion) and **Change**. "Criterion n" refers to the product owner's list in order: 1 real states, 2 final copy, 3 widths and modes, 4 WCAG 2.2 AA, 5 visual consistency, 6 buildable without a design question.

## Edge pages

### E1 Sign-in timed out (Timeout, -mobile, -dark)

**Score 8.6 · Ship · confidence high**

- States: the code sign-in variant is drawn at 1440, 390 and dark. The sign-up, passkey (E1a), provider (E1b) and no-email variants have final copy on the same layout, which is a fair use of copy-only.
- Copy says what timed out, after how long (20 minutes, 'without activity' because the clock slides) and what was kept. The lede wraps after the @ with the long-email markup. No red and no blame.
- Frame: mascot 689,64 and `h1` 166 at 1440; 164,32 and 134 at 390; no overflow at 320. Clock badge is `aria-hidden` and the `h1` takes focus with no ring. Rig: 0 blockers and 0 majors in light and forced-dark builds.
- Buildability: the trigger, the 303, `InitiateLoginUri ?? ClientUri` and `login_hint` are all specified. Points come off because the warning that leads here has no board, and the page never names the destination when Start again leaves Richie.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| should-fix | The B-29 time-limit warning ("Still there? This page times out in 2:00." + an inline **Keep going** button) is a new element in the slot with its own CSS, but no board draws it. | spec-edge §3.6; `edges.css` `.ri-edge-slot-action`; Part A §5.3 limits copy-only states to swaps "with no new element" | Criterion 1 (real states), criterion 6; WCAG 2.2.1 | Draw it once (e.g. on `Verify-help` or a copy of `Main`), with Keep going focused, at 1440 and 320. |
| nice-to-have | For a third-party client, Start again goes to that client's login start, but the page doesn't say the person is leaving Richie. | spec-edge E1 behaviour (`Client.InitiateLoginUri` ?? `ClientUri`) | Match between system and world (Nielsen 2) | When the client isn't Richie's own, label the action "Start again in Tallybird", or add the client name to the note. |

### E2 Session expired (Session-expired, -mobile; E3 alert as copy)

**Score 8.0 · Ship · confidence medium-high**

- Behaviour is unusually complete: alertdialog with `aria-modal`, labelled and described, title focused, Tab cycling between two exits, Escape and outside clicks ignored with a stated reason (D3), a 30-minute `sessionStorage` draft, a visibility check, the per-page draft table and the E3 alert on return.
- Copy answers the one worry ("Your changes on this page aren't saved yet. Sign in again within 30 minutes and we'll put them back.") with a real number and an exit that isn't sign-in.
- The board doesn't show the state it describes. The Profile behind shows the saved "Jamie" / "Rivera" and no dirty Cancel/Save row, so nothing on screen is unsaved.
- Mobile: the drawer draws a grab handle on a drawer that can't be dismissed, and its Tab order runs bottom-up. E3's alert announces only its title. No dark board, though the forced-dark build renders correctly.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| should-fix | The backdrop contradicts the dialog: the copy says changes aren't saved, but the Profile behind shows its saved values and no dirty action row. | `Session-expired` at 1440 and 390: First name "Jamie", Last name "Rivera", no Cancel/Save; spec-edge E2 draft table says the dirty state shows Cancel and Save | Criterion 1 (real state drawn); visibility of system status | Draw edited name values and the dirty action row behind the scrim, as on AS `Profile-name-error`. |
| should-fix | A grab handle on a drawer that can't be dismissed suggests swipe-to-dismiss. | `Session-expired-mobile` top handle; spec-edge §8.1 `dismissible={false}` | Affordance and signifiers; criterion 6 | Remove the handle for non-dismissible drawers and say so in the spec. |
| should-fix | On the mobile drawer boards the DOM, and therefore Tab order, runs bottom-up: the secondary at y 832 comes before the commit at y 780. B-30 fixes this only as handoff text. | `Session-expired-mobile`: Go to Richie (y 832) is reached before Sign in again (y 780) (Tab probe at 390); spec Part B B-30 | WCAG 2.4.3 Focus Order; criterion 6 | Draw the mobile DOM order on the `-mobile` boards (commit first, `flex-direction: column`) so copied markup is correct, or mark the board as non-normative for order. |
| should-fix | Page alerts move focus to the alert title paragraph only. The description, which carries the next step, and the reference are not announced. | E3 "You were signed out" (spec-edge E3, layout `Signin-provider-canceled`): "We'll put back what you typed" is not announced; `<p data-slot=alert-title tabindex=-1>` with no `aria-describedby`; alert has no role by design (Part A §4.7, §4.10) | WCAG 4.1.3, 1.3.1; criterion 4 (screen-reader-sensible errors) | Give the title `aria-describedby` pointing at the description (and the reference row on P3), or focus the alert container labelled by its title and described by its description. |
| nice-to-have | There is no dark board for the dialog; only the forced-dark build covers it. | Part A §5.3 dark list; forced-dark render checked | Criterion 3 | Optional: add `Session-expired-dark`, since the scrim and drawer surfaces are new in dark. |

### E4 Error with reference (Error, -mobile, -dark)

**Score 8.6 · Ship · confidence high**

- Drawn in light, mobile and dark. The exclamation badge, the lede that says "not with your account" and the single filled exit follow the grammar. The reference is grouped 4-4-4 in tabular figures, not monospace.
- Copy is labelled "Copy reference" (the name starts with the visible label). Feedback goes to a polite slot under the button. The support link says it opens a new tab, and the reference travels with it.
- Route variants (third-party Tallybird, sign-out, other identity routes with Try again first) and clipboard fallbacks are final copy. The busiest variant, "We couldn't open this page" with two buttons, a reference and a note, is copy-only.
- If Richie's own client is the misconfigured one, Go to Richie loops back here. The spec admits it, and the only escape is a small inline link.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| should-fix | The other-routes variant (two stacked buttons, reference line, slot and note) is the layout recovery users will meet, and no board draws that combination. | spec-edge E4 "Actions, other routes (copy)"; §5 state matrix ("combination measured") | Criterion 1 | Draw `Error-route` (Try again busy optional) at 1440 and 320. |
| nice-to-have | When Richie's own client fails, the filled Go to Richie loops back to this page. | spec-edge E4 behaviour ("the person lands here again") | Help users recover from errors (Nielsen 9) | When the failing client is Richie's own, make Contact support the primary for that variant. |

### E5 Link no longer works (Error-link)

**Score 8.4 · Ship · confidence high**

- One page and one 410 for expired, used and tampered tokens. The lede names both causes without confirming either, and the recovery variant leads with that flow's own next step. The note says status emails keep coming.
- The owner-cancel and generic variants have final copy. The `h1` balances to 2 lines at 390, and nothing overflows at 320.
- Journey gap: a requester whose status link died while their request is still open presses Start a recovery request and lands on R2's "already has a recovery request… Not yours? Contact support". That error treats them as the suspect and gives no way back to their status.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| should-fix | For an honest requester with an open request, the primary leads to an error that treats them as a possible intruder and offers no route to their status. | `Error-link` primary → `Recover-account`; spec-recovery §3.2 "Open request" row | Avoid dead ends (Part A §1.7 principle 2); error recovery | Add a requester line to R2's open-request error ("Yours? Use the link in your latest status email, or contact support to get it again."), or add "Lost the email? Contact support." to E5's note. |
| nice-to-have | E5 has no 390 or dark board; only the harness covers it. | Part A §5.3; B-22 | Criterion 3 | Optional mobile board; the two-line h1 at 390 is the one layout change. |

### E6 Page not found (Error-notfound)

**Score 8.8 · Ship · confidence high**

- Plain cause, two likely destinations and no badge, because nothing the person cares about failed. The noun headline is a documented exception: the document title is fixed, and it's what people search for.
- Frame, focus (`h1`), contrast and widths are all clean in the rig and by eye at 390, 1440 and 320.
- The only weakness: on the identity host most 404 visitors are signed out, so Go to Richie and Sign in both land on Sign in. That's two buttons for one destination.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| nice-to-have | For a signed-out visitor both buttons end on Sign in. | spec-edge E6 behaviour (Go to Richie → app root, "which signs in if needed") | Aesthetic and minimalist design (Nielsen 8) | Keep Go to Richie only, or make the second action Help ("Can't sign in?") when no session exists. |

### E7/E8 Offline (Signin-offline, -dark; Offline)

**Score 8.5 · Ship · confidence high**

- E7 is a full copy of Main: email kept, focus on Send code, a red line with an inline icon in the `role=alert` paragraph, and every other method still enabled. The idle "You're offline. We'll keep what you typed." (Slate, polite) and back-online lines are specified and measured to one line at 358.
- E8 has the wifi-slash badge and a lede that warns the page will reload by itself. The busy state makes the automatic retry visible.
- The filled slot line sits flush on "or" at both widths. E8's busy, still-offline and load-failed states are copy-only, and E8 has no 390 board, though offline is a phone-first situation.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| should-fix | A filled status line sits flush on the "or" divider: 8 px below the commit, 0 px above "or", so the error reads as the divider's caption. | `Signin-offline`, `Signin-offline-dark`: slot text box 418-438 and `.ri-or` 438-458 at 1440; 386-406 and 406-426 at 390 (measured) | Gestalt proximity; criterion 5 (visual consistency) | Keep the 20/40 px reservation and add 8-12 px between `.ri-commit` and `.ri-or` so the line groups with the button it reports on. |
| nice-to-have | Offline has no mobile or dark board. | B-22; Part A §5.3 | Criterion 3 | Add `Offline-mobile`; it costs one wrapper. |

### E9/E10 Too many attempts (Signin-limited, -dark; Limited, -dark)

**Score 8.0 · Ship with fixes · confidence high**

- The countdown buttons are done well. The digits are `aria-hidden` and a fixed "available at" suffix gives a stable name (Chromium: "Send code , available at 4:53 PM"). The button is described by the reason, stays focusable, and starts from the server's time. Passkey, Google and Microsoft stay enabled.
- Limited states the wait in whole minutes and in m:ss, and its note answers "but I only tried once". Both pages are drawn in dark.
- Must-fix: the drawn slot line "This email has received 5 codes in an hour." contradicts ruling B-34 ("You've asked for 5 codes in an hour.", followed by the countdown or clock time). Part A §3.5 and §4.4 and spec-edge D12 still carry the old line.
- Should-fix: the same flush slot-to-"or" spacing as E7.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| must-fix | Rate-limit line contradicts binding ruling B-34. The board says "This email has received 5 codes in an hour." and omits the time that B-34 says follows it. | `Signin-limited`, `Signin-limited-dark` `#slot-status`; spec-edge E9 table, §7, D12; Part A §3.5 L1, §4.3, §4.4; Part B B-34 | Criterion 2 (final copy), criterion 6 (buildable without asking) | Use B-34's line everywhere it applies ("You've asked for 5 codes in an hour." per address; "This network has made 30 attempts in an hour." per network, followed by the countdown or clock time) and delete the variants from the domain specs and Part A. |
| should-fix | A filled status line sits flush on the "or" divider: 8 px below the commit, 0 px above "or", so the error reads as the divider's caption. | `Signin-limited`, `Signin-limited-dark`: slot text box 418-438 and `.ri-or` 438-458 at 1440; 386-406 and 406-426 at 390 (measured) | Gestalt proximity; criterion 5 (visual consistency) | Keep the 20/40 px reservation and add 8-12 px between `.ri-commit` and `.ri-or` so the line groups with the button it reports on. |

### E11 Sign-out prompt (Signedout-prompt)

**Score 8.7 · Ship · confidence high**

- The page asks a question with the account chip (sr "Signed in as…"). The fill goes on the expected answer, Sign out of every device is outline, and Stay signed in is plain and set apart by the slot. The departure from "Cancel" is argued and right.
- Busy (E12: all three buttons `aria-disabled`, gerund in the polite slot) and three failure lines are final copy. The prompt condition, the antiforgery form with two submits, and `RemoveSessionsAsync` are all specified.
- Clean at 390, 1440, 320 and in forced-dark. Small debits: the unusual three-button busy state is undrawn, and the page has no mobile or dark board.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| nice-to-have | E12 disables three controls at once, an uncommon pattern, and it is copy-only. | spec-edge E12; layout reference AS `Security-passkey-waiting` | Criterion 1 | Draw the busy state once so the disabled look of the two non-pressed buttons is fixed. |
| nice-to-have | No mobile or dark board for the prompt; the account chip and three-button stack are covered only by the harness. | B-22; Part A §5.3 | Criterion 3 | Optional `-mobile` wrapper (the chip is the element most likely to wrap). |

### E13 Signed out (Signedout, -mobile, -dark, Signedout-client)

**Score 8.8 · Ship · confidence high**

- The ok badge, a lede scoped to this device, Sign in again, and a lost-device note pointing at the exact place to act. Back to Tallybird is drawn and measured: the note moves 56 px and the head stays fixed.
- Front-channel iframe (`aria-hidden`, `tabindex=-1`), no automatic redirect, and the delta to the settings sign-out are specified. Drawn in light, mobile and dark.
- Canvas only: the settings sidebar's Sign out isn't wired to this board, so journey J12 can't be walked. The note link jumps straight to `Security`, while the spec routes it through Sign in.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| nice-to-have | The canvas doesn't wire the sidebar Sign out (a `<button>` with no target) to `Signedout`, and the note goes straight to `Security`. | `Account-sidebar.dc.html` Sign out; `Signedout` note href `Security.dc.html`; Part A §3.1 E13 exits | Criterion 6 (journeys walkable) | Link the sidebar Sign out to `Signedout` and point the note at `Main`. |

### E14 Signed out everywhere (Signedout-everywhere)

**Score 8.6 · Ship · confidence high**

- The headline confirms the scope, the lede says what still works (passkeys and settings), and the note names the one thing not covered (connected apps) with the place to fix it.
- The two-line `h1` at 390 and 320 renders cleanly. The Tallybird variant is final copy on `Signedout-client`'s layout.
- Debits: no mobile or dark board. QE-3 settles on no email for a security-relevant action, which leaves this the only account-wide security change in the area without a notification.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| nice-to-have | "Sign out of every device" sends no email, unlike every other account-wide security change. | spec-edge QE-3 | Consistency of security notifications (Part A §4.3 "We always send these") | Decide explicitly with security. If yes, reuse the security email template ("You signed out of every device…"). |
| nice-to-have | No mobile or dark board. | B-22 | Criterion 3 | Optional wrapper boards. |

## Account settings

### S-A Passkey management (Security, -mobile, -dark, -passkey-added, -passkey-limit, -rename*, -remove-failed, -load-error*)

**Score 8.6 · Ship · confidence high**

- Every asked state is drawn. Added: new row first, polite status, no toast. Limit: 10 rows with two 40-character names, button hidden, quiet note, live region kept. Rename: default, plus the too-long error in a drawer. Remove: the failure sits inside the confirm. Load error: light and dark, with `role=alert` on the text only.
- Copy states the number ("Use 40 characters or fewer.", "You have 10 passkeys, the limit."), and row buttons quote the name ("Rename the “Pixel 8” passkey"). Visually identical to the shipped Security board.
- Rig clean; every Tab stop shows focus. Under 1.4.12 text spacing only "Remove passkey" goes tight (145 px of content in a 140 px button at 1440).
- Debits: the failure line breaks the quoted name ("the “Pixel / 8” passkey"). The too-long error doesn't give the count. The spec contradicts itself on wrapping. The base board's slot has no alert paragraph, though failures use `role=alert`.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| should-fix | The spec says two different things about passkey names: §2.2 "wrap freely (`.wrap-all`, no clamp)" and §11 "…2 at 390 and 320 under `wrap-2`". | spec-settings §2.2 vs §11 "Passkey rows at the limit" | Criterion 6 | Delete the §11 sentence or restate it as wrap-all. |
| should-fix | Base `Security` draws only `p[role=status]` in the passkeys and providers foots, while action failures there use `role=alert`. A live region has to exist before its message to be announced reliably. | `Security.dc.html` `.ra-section-foot`; spec-settings §1.4 ("base boards draw only the polite one") | WCAG 4.1.3 | Draw both paragraphs on the base and state that both render at page load. |
| nice-to-have | The balanced failure line splits the quoted passkey name across lines. | `Security-remove-failed` at 1440 and 390: "the “Pixel / 8” passkey" | Readability | Keep a quoted name of 20 characters or fewer together (`white-space: nowrap` on the name span), and let longer names wrap. |
| nice-to-have | The too-long error doesn't tell the person how far over they are, and the field shows a truncated value. | `Security-rename-error-mobile`: 45-character name, "Use 40 characters or fewer." | WCAG 3.3.3 Error Suggestion | Add the count ("Use 40 characters or fewer. This name has 45."). |

### S-B Email settings (Profile, Profile-email-*)

**Score 8.5 · Ship · confidence medium-high**

- Step 1's taken error is drawn on desktop and in the drawer. It keeps the typed value and the dialog's single height (it wraps to 2 lines inside the reserved space at 1440). Step 2's "New code sent. Use the code in the newest email." is drawn with focus on the code. The done state updates the row, the sidebar and the status.
- The S6 old-address email is fully written: masked address, absolute time with zone, "This wasn't me" to support with a 30-day token. The L1 countdown's width is reserved and measured.
- Debits: the per-account address-check limit (10 an hour) has copy only in an open-question default, not in the state table. The mobile drawers' Tab order runs bottom-up. Spec §8 says the longest field-slot string fits one line at 400, but the taken error wraps to two at 1440.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| should-fix | The address-check limit (10 an hour, "You've tried 10 addresses in an hour.") is a real state whose copy lives only in the open-questions table. | spec-settings §10 Q-S4 vs §3.1 state table | Criterion 1, criterion 6 | Add the row to §3.1: commit label, slot line, the 0:00 line and the layout reference. |
| should-fix | On the mobile drawer boards the DOM, and therefore Tab order, runs bottom-up: the secondary at y 832 comes before the commit at y 780. B-30 fixes this only as handoff text. | `Profile-email-code-mobile`: input, Use a different email, Cancel (y 832), then Change email (y 780) (Tab probe at 390); spec Part B B-30 | WCAG 2.4.3 Focus Order; criterion 6 | Draw the mobile DOM order on the `-mobile` boards (commit first, `flex-direction: column`) so copied markup is correct, or mark the board as non-normative for order. |
| nice-to-have | The measurement claim doesn't match the render. §8 says the longest field-slot string is 1 line at 400, but "That email already belongs to another Richie account. Use a different one." wraps to 2 lines in the 1440 dialog. | `Profile-email-taken` at 1440; spec-settings §8 `.ri-set-field-slot` | Criterion 6 (trustworthy measurements) | Correct the note. The height still holds because the dialog keeps step 2's height. |

### S-C Google and Microsoft in settings (Security section, -linked, -link-error, -unlink)

**Score 8.3 · Ship · confidence medium**

- The section sits between Passkeys and devices. The brand tiles are right: the G on a white or black tile and the four squares unaltered. One row per provider; controls named "Link Microsoft" and "Unlink Google". The linked and linked-elsewhere returns are drawn, with focus on the section `h2` and the outcome in the polite paragraph (A11Y-15 reasoning).
- The unlink confirm says what stops and what remains, and step-up gating for link and unlink is specified down to `auth_time` and the callback rules.
- Debits: seven outcomes (busy, canceled, organization approval, timed out, provider error, signed out while away, L9 countdown) are copy-only on one board, including the 204 px busy row button. The Google scheme registration contradicts spec-sso. The provider-error support topic is unstated.
- The not-linked row's reserved second line shows as an empty band at 1440.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| should-fix | The two specs give different Google registrations, and each cites the other as its source. | spec-settings §9.2 "Schemes": `AddOpenIdConnect("google", …)`; spec-sso §13.2: `AddGoogle("google", …)` with `ClaimActions.MapJsonKey("email_verified", "verified_email")` | Criterion 6 | Pick one handler and fix the claim mapping and `CallbackPath` in both specs. |
| should-fix | The busy row action ("Taking you to Microsoft…", 204 px) and "Link in 41:12" are new row-action shapes that are only measured, never drawn. At 390 the row action wraps under the text. | spec-settings §1.2, §4.1 (COV-26 measurements); no board | Criterion 1 | Draw `Security-linking` at 390 and 1440. |
| should-fix | The provider-error note's support link has no topic. spec-recovery §15.1 lists `provider` for settings link errors, while Part A §4.5 routes references through `error`. | spec-settings §4.2 "Provider error" row; spec-recovery §15.1; Part A §4.5 | Criterion 6 | State `topic=provider&ref=…` (or `error`) in §4.2. |
| should-fix | Base `Security` foot lacks the `role=alert` paragraph used by "failure before the redirect". | `Security.dc.html` `#providers .ra-section-foot` | WCAG 4.1.3 | As S-A: draw both paragraphs. |
| nice-to-have | Reserving a second description line leaves a visible empty band in not-linked rows at 1440. | `Security` Microsoft row; `Security-recovered` Google row | Aesthetic integrity | Fill the reservation with content: "Not linked." on line 1 and "Works with a work, school or personal account." on line 2, so heights match without blank space. |

## Account recovery

### R-A Can't get the code (Recover, -mobile, -dark, Verify-help)

**Score 8.6 · Ship · confidence high**

- R1 is Sign in's method stack without the form, in the same order with the same labels, so every visitor sees every way in and nothing is revealed. The likely fix (Back to the code) is the filled primary. Recovery sits last, under a hairline and an `h2`, with the reply time stated. Drawn in light, mobile and dark; `Verify-help` shows the entry point.
- Variants (from=link hides Microsoft, from=passkey retitles and offers a code, no sign-in in progress) and passkey and provider states are final copy. Support links carry topics and the new-tab suffix.
- Debits are spec hygiene. §2.2 step 4 still says step-up "by passkey or provider-backed session", against B-18 and spec-settings Q-S1. §7.3 still says screen C keeps Contact support, against §15.6 and B-25. And `Verify-help` places "Can't get the code?" differently from `Provider-link-code`.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| should-fix | Normative text contradicts rulings: step-up methods (§2.2 vs B-18) and settings screen C (§7.3 vs §15.6, B-25). | spec-recovery §2.2 step 4; §7.3 last bullet; Part B B-18, B-25 | Criterion 6 | Edit both sentences to the rulings. |
| should-fix | Code-entry screens in one product use two grammars: `Verify-help` puts "Can't get the code?" below the commit as a switch line (Tab: input, commit, link); `Provider-link-code` puts "Use a passkey instead · Can't get the code?" between the meta and the commit (Tab: input, two alternatives, commit). | `Verify-help` vs `Provider-link-code`; Tab probes at 1440 | Consistency and standards (Nielsen 4); WCAG 3.2.3 Consistent Navigation | Pick one placement for code alternatives across Verify, P6, A3's code step and R4 (the AS step-up pattern is fine) and state it once in Part A §4. |

### R-B Recovery request steps (Recover-account, -email, -code, -details*, -limited*)

**Score 8.1 · Ship with fixes · confidence medium-high**

- Three steps, each with "Step n of 3" and its own `h1` and document title. R2's note sets the whole wait up front (2 business days, then 3 days). R3 says the new address gets the updates. R4 reuses the shipped code entry. R5 asks five plain questions, two required, with hints linked by `aria-describedby` and a line saying support never asks for codes. The limited state is drawn in light and dark.
- Must-fix: R2's per-network line "This network tried to sign in 30 times in an hour." contradicts B-34, even though the row claims to be "E9's L2 line verbatim".
- Should-fix: R5 fields carry `maxlength` (100/200/1,000), so the specified "Use 100 characters or fewer." errors can never fire and pastes are cut silently, which spec-settings rejects for rename. The time-limit warning and the two first-paint expiry alerts are copy-only.
- Frame: the `h1` drops 24 px for the step line on every step page (190 vs 166 at 1440), consistently.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| must-fix | Rate-limit line contradicts binding ruling B-34. R2 uses "This network tried to sign in 30 times in an hour." | spec-recovery §3.2 L2 row (COV-16 "E9's L2 line verbatim, today 'This network tried to sign in 30 times in an hour.'"); Part B B-34 | Criterion 2 (final copy), criterion 6 (buildable without asking) | Use B-34's line everywhere it applies ("You've asked for 5 codes in an hour." per address; "This network has made 30 attempts in an hour." per network, followed by the countdown or clock time) and delete the variants from the domain specs and Part A. |
| should-fix | `maxlength` on R5 contradicts the spec's too-long errors, and it truncates pastes silently. | `Recover-details.dc.html` inputs `maxlength=100/200/100/100`, textarea `1000`; spec-recovery §3.5 "Too long" row; spec-settings §2.1 (no `maxlength`: "a pasted long name would be cut silently") | WCAG 3.3.1 Error Identification; consistency | Remove `maxlength` and validate on submit with the specified errors, as rename does. |
| should-fix | The B-29 time-limit warning ("Still there? This page times out in 2:00." + an inline **Keep going** button) is a new element in the slot with its own CSS, but no board draws it. | spec-edge §3.6; `edges.css` `.ri-edge-slot-action`; Part A §5.3 limits copy-only states to swaps "with no new element" | Criterion 1 (real states), criterion 6; WCAG 2.2.1 | Draw it once (e.g. on `Verify-help` or a copy of `Main`), with Keep going focused, at 1440 and 320. |
| nice-to-have | Visible "Send request at 9:15 AM" omits "tomorrow"; only the sr suffix and the note below carry it. | `Recover-limited` at 390 and 1440 | Information parity | If it fits at 288 px, show "Send request tomorrow at 9:15 AM"; otherwise keep the note directly under the button. |
| nice-to-have | The first-paint expiry alerts ("This took too long" on R2, "Confirm your new email again" on R3) are copy-only. | spec-recovery §3.2, §3.5 | Criterion 1 | Optional board for one of them. |

### R-C Request status pages (Recover-review, -approved*, -completed, -declined)

**Score 7.9 · No-ship · confidence medium-high**

- The drawn pages are strong. Badges follow meaning (clock for waiting, check for done, exclamation for declined). The date is the `h1` and the facts are a real `dl` with zones. Cancel request is outline so a nervous reader doesn't take it as the next step. R8 has one action and the new email in bold. R9's primary is the only thing that can change the outcome. The approved state is drawn at 390.
- Variant copy for R6a, R6b, R7a ('changing now' and 'blocked'), R10a and R10b is final.
- Below the bar for two reasons. Ruling B-36 creates a new end state, a request closed as expired after 7 days with no decision, that has no status page, badge, actions or email copy beyond one phrase. And the canvas sends the requester's Cancel request (R6, R7) to `Recover-cancel`, which carries the owner's copy ("If it wasn't you, cancel it."); the requester's own confirm (R6a) is undrawn.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| must-fix | Ruling B-36 (request expires after 7 days undecided; decline-style notice; new request allowed at once) has no page or email specification. | Part B B-36; spec-recovery §3.6–§3.9 and §4.1 have no expired row; R2 already says "open for up to 7 days" | Criterion 1 (real states), criterion 2 | Add R-expired: `h1`, lede, badge, actions (Start a new request, no wait; Contact support), the status-token behaviour and emails to the new and the old address. |
| should-fix | On the canvas, the requester's Cancel request opens the owner's confirm page and copy. R6a is copy-only. | `Recover-review`, `Recover-approved` → `Recover-cancel.dc.html`; spec-recovery §3.6 ("it carries the owner's copy") | Criterion 6 (journeys); match between system and world | Draw `Recover-review-cancel` (R6a: "Cancel your request?", Keep the request) and relink. |
| nice-to-have | R9's unavailable "New request on November 12" looks like a broken outline button. | `Recover-declined` at 390 | Signifiers | Consider a sentence ("You can start a new request on November 12.") until the day, then the button. |

### R-D Old-address alert and cancel (Email-recovery-alert, Recover-cancel, Recover-canceled)

**Score 8.3 · Ship · confidence medium-high**

- The email states the event plainly in subject and title, masks the new address, puts "If this wasn't you, cancel the request." before the button and the reassurance after it, and lists the evidence (time with zone, browser, city). It has fine print and a link fallback, and renders well at 390 and dark.
- R12 makes the protective action the fill. "Not sure? Cancel it. Nothing about your account changes." removes the fear. R13 confirms in the owner's terms and states the 7-day stop.
- Debits: the spec's drawing-to-email table still names the button "This wasn't me" with radius 10, while the board (correctly, per B-10) draws "Cancel this request" as a pill. The same stale label sits in §2.1, §5.1 and §7.4. On the canvas, Keep the request opens the requester's view with the full new address, which D-R10 forbids for the owner (R6b is undrawn).

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| should-fix | Stale normative text contradicts B-10 and the board. | spec-recovery §4.3 table row ("This wasn't me (default lg, Signal fill… radius 10…)"); §2.1 map; §5.1 step 7; §7.4 row; board `Email-recovery-alert` draws "Cancel this request" as a pill | Criterion 2, criterion 6 | Update the four places; state pill or radius 10 for the email CTA once. |
| should-fix | The canvas shows the owner the requester's view, with the new address in full, after Keep the request. | `Recover-cancel` Keep the request → `Recover-review.dc.html`; spec-recovery D-R10, §3.6 owner view | Part A §1.7 principle 5 (reveal no more) | Draw R6b (masked New email, no Account row) and relink. |
| nice-to-have | R12's 40 px reserved slot between Cancel the request and Keep the request detaches the second button. | `Recover-cancel` at 1440: primary ends 586, Keep the request starts 626 | Proximity | Accept, or put Keep above the slot as a plain link. |

### R-E Recently recovered (Security-recovered)

**Score 8.6 · Ship · confidence high**

- A full copy of Security with the data a recovered account would really have: one passkey (the lost phone), Google not linked, one device and the recovery activity rows. The info alert is a named `section` with in-page links to passkeys and devices and a support route.
- The copy speaks to both possible readers without accusing either. The 7-day step-up note and the undone state are final copy. Visually identical to shipped settings at 1440 and 390.
- Small debits: content measures 1,808 px on an 1,800 px board, so the bottom is clipped on the canvas. The alert doesn't link Add a passkey, the one step R8 and email #5 push hardest.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| nice-to-have | Board height is 8 px short of its content. | `Security-recovered` fixed render 1808 vs canvas 1440x1800 | Canvas fidelity | Set the board to 1900. |
| nice-to-have | The alert asks the person to check passkeys but not to add one, though that's the best protection against a second recovery. | `Security-recovered` alert; spec-recovery R8 note, email #5 | Recognition over recall | Add "Add a passkey" as a link to `#passkeys` in the alert description. |

## Single sign-on

### P-A Sign in with Google and Microsoft (Main*, Signin-redirecting, Signin-provider-*)

**Score 8.2 · Ship with fixes · confidence medium-high**

- Order is email first, then "or", then passkey, Google and Microsoft as equal outline block buttons with one 20 px mark box. The official G is an image and the four squares are unaltered. The busy state swaps the mark for the spinner and moves nothing (y identical to `Main`), and it's announced once through the slot. P2 and P3 alerts appear only at first paint, and P3 carries a reference with Copy/Copied and the organization-approval variant.
- Light, dark (P3) and forced colours render correctly. The `fetch`/`location.assign` challenge, bfcache reset and callback decision table are a build-ready spec.
- Must-fix: the L6 alert copy "This network has made 30 failed sign-in attempts in an hour." contradicts B-34.
- Should-fix: the alerts announce only their titles; brand sign-off is still pending; the Google registration conflict carries over from S-C.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| must-fix | Rate-limit line contradicts binding ruling B-34. The L6 alert uses "This network has made 30 failed sign-in attempts in an hour." | spec-sso §3.5 L6 alert description; Part B B-34 | Criterion 2 (final copy), criterion 6 (buildable without asking) | Use B-34's line everywhere it applies ("You've asked for 5 codes in an hour." per address; "This network has made 30 attempts in an hour." per network, followed by the countdown or clock time) and delete the variants from the domain specs and Part A. |
| should-fix | Page alerts move focus to the alert title paragraph only. The description, which carries the next step, and the reference are not announced. | `Signin-provider-canceled`, `Signin-provider-error`: "The sign-in didn't finish. Try again in a moment, or use your email or a passkey." is not announced; `<p data-slot=alert-title tabindex=-1>` with no `aria-describedby`; alert has no role by design (Part A §4.7, §4.10) | WCAG 4.1.3, 1.3.1; criterion 4 (screen-reader-sensible errors) | Give the title `aria-describedby` pointing at the description (and the reference row on P3), or focus the alert container labelled by its title and described by its description. |
| should-fix | Brand and legal sign-off is still open for the custom Google button (our font, 16 px, our edge colour) and for "Sign up with Microsoft", which is not a Microsoft-listed label. | Part A §8 Q3, Q4, P-1, P-2; spec-sso OQ-6 | Criterion 2 (final copy); third-party branding terms | Get the Google and Microsoft brand checks before build freeze; keep the fallback label ("Sign in with Microsoft" on A2) ready as a text swap. |
| nice-to-have | The L6 state (alert plus three countdown method buttons) is a combination of drawn parts but isn't drawn itself. | spec-sso §3.5 | Criterion 1 | Optional board; it's the busiest version of Sign in. |

### P-B Welcome back with a provider (Signin-remembered-provider)

**Score 8.3 · Ship · confidence medium-high**

- Mirrors identity's passkey welcome-back. The chip has Not you? drawn as a real POST form with a full accessible name. The lede says which method was used last, and Sign in another way keeps the account remembered. The rule for no filled button is argued (Google's ground rule).
- At 390, on a page whose job is "is this you?", the chip truncates the sample address to "jamie.riv… @northwind.coffee" (with a stray gap before the @). At 320 it wraps instead, so the chip behaves two ways.
- Tab reaches Not you? before Sign in with Google. That follows the visual order, but the only action comes second. No dark or mobile board.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| should-fix | The account chip truncates the person's own address at the most common phone width. | `Signin-remembered-provider` at 390: "jamie.riv… @northwind.coffee"; at 320 it wraps after the @ | Recognition; WCAG 1.4.10 spirit (no loss of information) | Let the chip wrap after the @ at every width (as it does at 320), or move Not you? under the chip. |
| nice-to-have | No dark or 390 board for a page people mostly see on their own phone. | B-22 | Criterion 3 | Add a `-mobile` wrapper. |

### P-C Create account with a provider (Signup*, Signup-finish*)

**Score 8.4 · Ship · confidence medium-high**

- Create account is Sign in's twin. Names sit side by side from a 400 px column and stack below it; the shared hint is described on both fields; there's a legal line with real links; providers are labelled "Sign up with…". `Signup-finish` prefills Priya's names, shows the provider email as a fact chip, and says in the lede what we take.
- Error markup is specified precisely: each error is its own paragraph, the hint stays referenced, there are no links in errors, and the switch line carries the email. All widths are clean, with 1,058 px of scroll at 390.
- Debits: field errors, limits, the relabelled second action and the A3 code step are copy-only. The legal line sits about 40 px below Create account behind the reserved slot, so it reads detached at 320. The brand label is pending (see P-A).

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| should-fix | Brand and legal sign-off is still open for the custom Google button (our font, 16 px, our edge colour) and for "Sign up with Microsoft", which is not a Microsoft-listed label. | Part A §8 Q3, Q4, P-1, P-2; spec-sso OQ-6 | Criterion 2 (final copy); third-party branding terms | Get the Google and Microsoft brand checks before build freeze; keep the fallback label ("Sign in with Microsoft" on A2) ready as a text swap. |
| nice-to-have | The A3 code step for an unconfirmed Microsoft email changes the commit to Create account and the alternative to Use a different email, on P6's layout, and it's copy-only. | spec-sso §6.3 | Criterion 1 | Optional board; it's a distinct branch with its own exits. |
| nice-to-have | The legal line floats far from the commit it qualifies. | `Signup` at 320: Create account ends at 726, the legal line starts about 60 px lower | Proximity | Accept for slot stability, or move the legal line above the commit. |

### P-D Provider dead ends (Provider-no-account, -no-email, -not-linked)

**Score 8.6 · Ship · confidence high**

- P4 serves its three readers in order of likelihood: create with it, switch account, or sign in another way, plus a note for the work-email case. P7 explains in one sentence, always offers the email way, and has a create switch. P8 says "you're signed in" first, then the reason and both exits.
- Badges follow meaning: none on P4 (a question), exclamation on P7 and P8. Reveals are no more than Sign in. Each `h1` takes focus. No overflow at 320 (P4's `h1` runs 3 lines there).
- Debits: P4 has no filled control, and its "Sign up with Google" looks like a fresh trip to Google but is a link to A3. No mobile or dark boards.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| nice-to-have | P4's primary is drawn as a provider method button but doesn't go to the provider, and nothing on the page is filled. | `Provider-no-account` → `Signup-finish.dc.html`; spec-sso departure 14 | Signifiers | Accept (argued), or label it "Continue with this Google account" to say there is no second trip. |
| nice-to-have | No mobile or dark boards for the three pages. | B-22 | Criterion 3 | Optional. |

### P-E Link a provider at sign-in (Provider-link*, Provider-link-code)

**Score 8.4 · Ship · confidence medium-high**

- Nothing is ever linked on email alone: P5 proves the Richie account with a code or a passkey. The chip shows only provider-supplied initials, and the lede says why we ask and what happens next. P6's commit names what the code completes ("Link Microsoft"). P5 is drawn at 390. The passkey button shows whether or not a passkey exists, so it reveals nothing.
- P6 loses context. Its `h1` and lede ("Check your email", "Enter the code we sent to…") don't mention Microsoft or linking, so someone back from their mail app sees no purpose until the button.
- P6's alternatives sit between the input and the commit, so Tab from the code reaches "Use a passkey instead" before Link Microsoft. `Verify-help` and `Recover-code` go straight from input to commit.

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| should-fix | P6 doesn't say what the code is for until the commit. | `Provider-link-code` `h1` "Check your email", lede "Enter the code we sent to jamie.rivera@northwind.coffee." | Visibility of system status; WCAG 2.4.6 | Lede: "Enter the code we sent to … to link Microsoft." (check it stays 2 lines at 390). |
| should-fix | Code-entry screens in one product use two grammars: `Verify-help` puts "Can't get the code?" below the commit as a switch line (Tab: input, commit, link); `Provider-link-code` puts "Use a passkey instead · Can't get the code?" between the meta and the commit (Tab: input, two alternatives, commit). | `Provider-link-code` vs `Verify-help` and `Recover-code`; Tab probes at 1440 | Consistency and standards (Nielsen 4); WCAG 3.2.3 Consistent Navigation | Pick one placement for code alternatives across Verify, P6, A3's code step and R4 (the AS step-up pattern is fine) and state it once in Part A §4. |

## Overall set

**Score 8.1 · Ship with fixes · confidence medium-high**

A coherent, restrained and unusually well-specified set. 57 identity-frame boards share one measured frame (mascot 689,64 and h1 166 at 1440; 164,32 and 134 at 390), and the shiba-with-badge is a distinctive, accessible state signal. Across 80 boards × 10 viewports, in light and forced-dark builds, the rig finds 0 blockers and 0 majors, and every one of 2,528 Tab stops shows focus. Journeys are walked end to end with real numbers and few dead ends. Buildability is what holds the set at 8.1. The precedence rule between Part A, Part B and the domain specs is circular, three late rulings (B-34 to B-36) never reached the page tables, one drawn board contradicts a binding copy ruling, and one ruling (B-36) created a state nobody designed. Fix the two must-fixes and the reconciliation pass, and the set is a clear 8.5.

### Consistency across the four areas

- **One frame, measured.** All 57 identity-frame boards (edge, recovery and SSO) put the mascot at 689,64 and the `h1` at 166 at 1440 (190 under a step line), at 164,32 and 134 at 390, and at 129,32 and 134 at 320. Moving between Sign in, an edge page, a recovery step and a provider outcome changes only the body, so it feels like the same place reporting back. Settings states are copies of the shipped Security and Profile boards; the only drift is the intended sidebar copy on recovered and changed-email boards.
- **Shared mechanics.** One failure phrase per surface (B-6), badges by meaning (check, clock, wifi-slash, exclamation, none for questions), the same countdown-button markup (B-27) in sign-in, settings and recovery, one status-slot rule, and one support handoff contract with fixed topics.
- **Where it breaks.** Rate-limit sentences differ across areas (cross-cutting 1). Code screens use two grammars (5). The alert-focus flaw is consistent everywhere, so it is shared rather than isolated (3).
- **Against the older identity boards and suiss.** The new pages swap the identity canvas's 52 px outlined fields, ≥240 px pills and viewport-height type (34/40/48) for settings' 44 px filled field with a 1 px edge, full-width 10 px-radius block buttons and a fixed 30/36 title. That's coherent: one control shape at every width, the same tokens as settings, and each choice argued with measurements in Part A §2 rows 3–7. It departs from suiss rule 2 (fields with no border) for WCAG 1.4.11, and from suiss's hard-coded `role=alert` on alerts in favour of focus. Both departures are documented; the second needs the fix in cross-cutting 3.

### Visual identity

A restrained, Apple-like system, and the shiba carrying a 30 px state badge is the one bold idea. It reads at a glance, it's `aria-hidden` so the `h1` always says the outcome in words, and it holds up in dark (Ink glyph on the Leaf disc) and under forced colours (badges and marks keep their shapes). There are no illustrations, no red pages and no decoration, and the provider marks are used as the brands require. At 1440 the edge pages are sparse: most content sits in the top 400 px of a 900 px board. That's consistent with Sign in and it's the right call for these moments. The visible polish debts are small and shared: the slot line touching "or" (8), the empty reserved line in not-linked provider rows (S-C), and a passkey name split across lines in a failure message (S-A).

### Journeys and dead ends

I walked the spec's journeys J1–J23 against the boards and the canvas links. Every page has a way out, recovery offers support at every closed door, the owner who misses the alert is caught by the completed email's support route, and P8 closes the link-conflict case. Remaining gaps:

- A requester with an open request whose status link died loops from E5 into R2's "Not yours?" error (E5).
- The expired-request end state from B-36 has no design (R-C).
- On the canvas, the requester's Cancel request and the owner's Keep the request land on each other's pages, and the owner's lands on a view that shows the full new address (cross-cutting 9).
- The settings sidebar Sign out isn't wired, so J12 can't be walked on the canvas (E13).

### Buildability

This is the set's strongest and weakest dimension at once. The specs give an engineer far more than usual: IdentityServer settings (`LoginUrl`, `ErrorUrl`, `LogoutUrl`, `ShowSignoutPrompt`, `RemoveSessionsAsync`), a callback decision table, `auth_time` step-up rules, `@repo/ui` component mappings, CSS recipes with their suiss mappings and contrast ratios, slot strings measured in two fonts, and a limits-and-lifetimes table. But the material runs to 4,424 lines in five files, layered as PM rulings over domain departures over a foundation, and the precedence rule between them is circular (1). Three late rulings never reached the page tables, so the same state has up to four strings. At least eight normative sentences contradict a ruling or each other (6), and normative content sits under "Phase 4 additions" and revision notes (11). An engineer can build from this, but not without asking questions, which is exactly what criterion 6 rules out. One reconciliation pass fixes most of it.

### By criterion

| Criterion | Assessment |
|---|---|
| 1. Real states | Mostly met. Every brief item has a default board and its failure, limit and success states are drawn or written as final copy on a drawn layout. Gaps: B-36's expired request (must-fix), the undrawn time-limit warning, the E2 backdrop that doesn't show unsaved changes, and several new-shape states that are copy-only (E4 other routes, the S-C busy row). |
| 2. Final copy | High quality: plain, specific, real numbers, no blame, no banned words found on any board. Held back by the B-34 conflicts (drawn E9 line, R2 L2, L6 alert) and by the pending brand labels. |
| 3. 360 to desktop, light and dark | Met. No horizontal overflow on any board at 320, 375, 390, 768, 1024, 1280, 1440, 1920, 1280×540 or 200 % zoom in either build. Dark is drawn for each new element and every board renders correctly in the forced-dark build. The mobile and dark boards that are missing are covered by the harness. |
| 4. WCAG 2.2 AA | Largely met. Rig: 0 axe critical or serious findings in light and dark, and every one of 2,528 Tab stops has a visible indicator. Labels, `aria-describedby` hints, quoted accessible names and countdown names are done well, and text spacing (1.4.12) causes no clipping except one tight button. Open: alert focus that omits the description (4.1.3), bottom-up Tab order on the mobile drawer boards (2.4.3), and base settings slots missing their alert region. |
| 5. Visual consistency | Strong across all four areas and with shipped settings. The departures from the old identity boards are coherent and justified. Small spacing debts. |
| 6. Buildable without a design question | Partly met: the main reason the set is 8.1 and not 8.6 (cross-cutting 1, 6, 11). |

## Cross-cutting issues, prioritized

| # | Severity | Issue | Evidence | Principle | Change | Units |
|---|---|---|---|---|---|---|
| 1 | must-fix | The precedence rule is circular and the last rulings never reached the domain specs. Part A says it wins over domain specs; Part B and Part A §3.1 say a domain spec's departure wins; spec-settings and spec-sso say Part A wins. B-34, B-35 and B-36 exist only in Part B, so two L1 lines and three L2/L6 lines are in play, and a drawn board (`Signin-limited`) contradicts the binding ruling. | spec.md line 7 vs Part B intro and §3.1 note; spec-settings line 5; spec-sso line 3; spec.md §3.5 wording note; B-34, B-35, B-36; `Signin-limited` slot line | Criterion 6 (build without asking); criterion 2 | State one order (Part B rulings, then domain specs, then Part A). Then reconcile: write every ruling into the page tables engineers read, and fix the drawn E9 line. | All, most visibly E9/E10, R-B, P-A |
| 2 | must-fix | B-36's expired recovery request is a new end state with no page, actions, badge or emails. | spec.md B-36; spec-recovery §3.6–§3.9, §4.1, §15.4 | Criterion 1 (real states) | Specify R-expired copy and both emails; add the status-token behaviour for it. | R-C |
| 3 | should-fix | First-paint page alerts focus only the title paragraph. The description, which says what to do next, and P3's reference are not announced. | `Signin-provider-canceled`, `Signin-provider-error(-dark)`; E3, L6, R2 and R3 expiry alerts on the same pattern (Part A §4.7, §4.10) | WCAG 4.1.3, 1.3.1; criterion 4 | Add `aria-describedby` from the title to the description (and reference), or focus the alert container labelled and described. | P-A, E2 (E3), R-B, P-C, P-B |
| 4 | should-fix | The WCAG 2.2.1 time-limit warning adds an inline Keep going control inside the status slot on every timed page, and it's never drawn. | spec-edge §3.6; `.ri-edge-slot-action`; Part A §5.3 copy-only rule | Criterion 1; WCAG 2.2.1 | Draw it once at 1440 and 320 with focus on Keep going; reference that board from every timed page. | E1, R-B, P-C, P-E and every timed page |
| 5 | should-fix | Code-entry screens disagree on where the alternatives sit and on Tab order, and P6 drops its purpose from the head. | `Verify-help` (input, commit, link) vs `Provider-link-code` (input, alternatives, commit) vs `Recover-code` (input, commit, note) | Consistency and standards; WCAG 3.2.3 | One code-screen grammar in Part A §4, applied to Verify, P6, A3's code step and R4; name the purpose in P6's lede. | R-A, P-E, P-C |
| 6 | should-fix | Normative spec text is stale or contradictory in at least eight places, so engineers will have to ask. | spec-recovery §2.1/§2.3/§5.1/§7.4 and §4.3 ("This wasn't me", radius 10) vs B-10; §2.2 step-up via provider vs B-18; §7.3 screen C vs §15.6; spec-settings §9.2 `AddOpenIdConnect` vs spec-sso §13.2 `AddGoogle`; spec-settings §11 wrap-2 vs §2.2 wrap-all; `Recover-details` `maxlength` vs §3.5 too-long errors; §15.1 topic list lacks B-35's `signin-changed-not-me`; settings provider-error topic unstated | Criterion 6 | One reconciliation pass with a checklist; delete superseded sentences rather than annotating them. | R-A, R-B, R-D, S-A, S-C, P-A |
| 7 | should-fix | Mobile drawer and action-sheet boards have bottom-up Tab order. The fix exists only as handoff text (B-30), so anyone copying the board markup ships a focus-order failure. | Tab probes at 390: `Session-expired-mobile`, `Security-rename-error-mobile`, `Profile-email-code-mobile` (y 832 before y 780) | WCAG 2.4.3 | Draw mobile DOM order on `-mobile` boards, or label the board's order non-normative. | E2, S-A, S-B |
| 8 | should-fix | A filled status line touches the "or" divider (0 px), so the message groups with the divider instead of with the button. | Measured: slot 418–438, "or" 438–458 at 1440; 386–406 / 406–426 at 390 | Gestalt proximity; criterion 5 | Add 8–12 px above `.ri-or` when it follows `.ri-commit`; it's one shared rule and nothing else moves. | E7/E8, E9/E10 (and every Sign in-family state with a slot message) |
| 9 | should-fix | Canvas links misrepresent journeys. The requester's Cancel request lands on the owner's page. The owner's Keep the request lands on the requester's view, with the full address. The sidebar Sign out is unwired, and settings triggers don't open their overlay boards. | `Recover-review`/`Recover-approved` → `Recover-cancel`; `Recover-cancel` → `Recover-review`; `Account-sidebar` Sign out `<button>`; `Security` Rename/Unlink have no targets | Criterion 6 (links stand in for navigation) | Draw R6a and R6b and relink; wire Sign out to `Signedout`; link Rename, Remove and Unlink to their overlays. | R-C, R-D, E13, S-A, S-C |
| 10 | should-fix | Google and Microsoft brand sign-off is still open for the custom buttons and the sign-up label. | Part A §8 Q3/Q4, P-1/P-2; spec-sso OQ-6 | Criterion 2; third-party brand terms | Close before build freeze; keep the fallback labels as text swaps. | P-A, P-C, S-C |
| 11 | nice-to-have | The handoff is about 4,400 lines across five files, layered as rulings that override departures that override the foundation. Normative content lives under "Phase 4 additions" (spec-recovery §15) and revision notes. | spec.md 1,175 lines; spec-edge 867; spec-settings 698; spec-recovery 934; spec-sso 750 | Criterion 6 | After reconciliation, produce one per-page sheet (route, title, copy, states, focus, live regions, limits) generated from the specs. | All |

## Method and limits

**What I did**

- Built every board as plain HTML with the QA rig's `build.mjs`, once light and once with `BOARD_THEME=dark`. I also built the reference identity and shipped settings boards for comparison. Outputs and scripts are in the session scratchpad (`grade/`).
- Looked at every board myself, through contact sheets and full-size images, at 390 and 1440 in light and in the forced-dark build. I also checked 16 boards at 768 and 320 and four under emulated forced colours, and compared against the reference Sign in, Create account, Verify, passkey welcome-back, Security and change-email boards.
- Ran the rig's `run.mjs` on all 80 boards in both builds: 10 viewports, two schemes, axe, overflow, clipping and overlap, target size, and keyboard passes. Both runs: 0 blockers, 0 majors, 601 minors. The minors are 539 targets under 44 px (24 px footer links on fine pointers, 36 px sidebar items, 32–36 px settings row buttons, all above the 24 px AA minimum), 23 exempt inline links, and 39 axe "needs review" contrast items on overlay scrims and hidden code inputs.
- Ran my own Playwright probes:
  - accessibility-tree snapshots and Tab order on 13 boards, including all three mobile drawers;
  - mascot, `h1` and primary positions on all 57 identity-frame boards at 1440, 390 and 320;
  - content height against canvas board size for all 80 boards;
  - WCAG 1.4.12 text spacing at 320 and 1440 on every board;
  - status-slot spacing;
  - broken-link and incoming-link checks across board files.
- Read Part A and Part B in full, and sections 1–11 of each domain spec. I also read spec-recovery §15, because Part B ruling B-19 makes its support-handoff contract normative: I treated it as content, not history, and note its placement as an issue. I skipped every fix log, changelog and revision note as instructed. From the suiss guide I read the ten rules, the accessibility section and the gotchas.
- I did not open anything under `docs/design/identity-edges/qa/`, `wbs.md`, `inventory.md`, `docs/design/how-we-design.md` or `.design/edges/qa/`, and I edited no design, spec or stylesheet.

**Limits**

- The boards are static. Runtime behaviour (live-region timing, countdowns, focus moves, bfcache resets, the service worker, auto-reload) is judged from markup and spec, not observed.
- I tested in Chromium only, with Inter only. I didn't test SF Pro, Safari, Firefox or real mobile devices. The accessibility tree comes from Playwright, not from VoiceOver, NVDA or JAWS, and forced colours were emulated.
- The reference identity boards rendered without their mascot (that blob is missing locally), so the identity comparison covers layout, controls and type only.
- I used the local board mirror, stated to match the canvas, rather than the live canvas. I didn't check foreignObject or blob rendering on the canvas itself.
- I consulted the account-settings spec through citations rather than reviewing it in full. Emails are judged from the one drawn email board and the spec tables.
- Scores are one reviewer's judgement on a 10-point scale calibrated to the ship bar above. Confidence is lower where a unit's states are mostly copy-only.
