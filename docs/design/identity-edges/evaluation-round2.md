# Richie identity edges: independent evaluation, round 2

Re-graded 2026-11-05 by an independent reviewer who wasn't involved in the design or the revision. It covers the 84 boards in `.design/edges/project/` (the canvas https://claude.ai/artifact/6Xe7bnSdGHE3xvVR5LeMge, version `1793900431-dcb1`) and the five specs in this folder. The baseline is the round-1 grade (`evaluation.md`, `evaluation.json`): the same 23 units, the same six shippable criteria and the same scale. **8** means a team could build it now with minor follow-ups, **9 or higher** is exemplary, and a unit is **No-ship** below 8. **Ship** is 8 or above with no must-fix, and **Ship with fixes** is 8 or above with must-fixes listed. Every point below 10 traces to an open issue in the unit's table or to a cross-cutting issue it cites.

## Summary

| Unit | Name | Round 1 | Round 2 | Verdict | Confidence |
|---|---|---|---|---|---|
| E1 | Sign-in timed out (Timeout, -mobile, -dark; Signin-timeout-warning) | 8.6 | **8.8** | Ship | High |
| E2 | Session expired (Session-expired, -mobile; E3 alert as copy) | 8.0 | **8.7** | Ship | High |
| E4 | Error with reference (Error, -mobile, -dark) | 8.6 | 8.6 (carried) | Ship | High |
| E5 | Link no longer works (Error-link) | 8.4 | **8.7** | Ship | High |
| E6 | Page not found (Error-notfound) | 8.8 | 8.8 (carried) | Ship | High |
| E7/E8 | Offline (Signin-offline, -dark; Offline) | 8.5 | **8.7** | Ship | High |
| E9/E10 | Too many attempts (Signin-limited, -dark; Limited, -dark) | 8.0 | **8.7** | Ship | High |
| E11 | Sign-out prompt (Signedout-prompt) | 8.7 | 8.7 (carried) | Ship | High |
| E13 | Signed out (Signedout, -mobile, -dark, Signedout-client) | 8.8 | 8.8 (carried) | Ship | High |
| E14 | Signed out everywhere (Signedout-everywhere) | 8.6 | 8.6 (carried) | Ship | High |
| S-A | Passkey management (Security*, -passkey-*, -rename*, -remove-failed, -load-error*) | 8.6 | **8.8** | Ship | High |
| S-B | Email settings (Profile, Profile-email-*) | 8.5 | **8.8** | Ship | High |
| S-C | Google and Microsoft in settings (Security section, -linked, -link-error, -unlink) | 8.3 | **8.6** | Ship | Medium |
| R-A | Can't get the code (Recover, -mobile, -dark, Verify-help) | 8.6 | **8.7** | Ship | High |
| R-B | Recovery request steps (Recover-account, -email, -code, -details*, -limited*) | 8.1 | **8.7** | Ship | Medium-high |
| R-C | Request status pages (Recover-review, -review-cancel, -approved*, -completed, -declined, -expired) | 7.9 | **8.6** | Ship | High |
| R-D | Old-address alert and cancel (Email-recovery-alert, Recover-cancel, -owner-view, -canceled) | 8.3 | **8.7** | Ship | High |
| R-E | Recently recovered (Security-recovered) | 8.6 | 8.6 | Ship | High |
| P-A | Sign in with Google and Microsoft (Main*, Signin-redirecting, Signin-provider-*) | 8.2 | **8.7** | Ship | High |
| P-B | Welcome back with a provider (Signin-remembered-provider) | 8.3 | **8.6** | Ship | Medium-high |
| P-C | Create account with a provider (Signup*, Signup-finish*) | 8.4 | 8.4 (carried) | Ship | Medium-high |
| P-D | Provider dead ends (Provider-no-account, -no-email, -not-linked) | 8.6 | 8.6 (carried) | Ship | High |
| P-E | Link a provider at sign-in (Provider-link*, Provider-link-code) | 8.4 | **8.8** | Ship | High |
| **Set** | **Overall** | **8.1** | **8.6** | **Ship** | High |

All 23 units Ship. Nothing is below 8 and no must-fix remains. The mean unit score is 8.68 (round 1: 8.43). R-C, the round-1 No-ship, rises from 7.9 to 8.6.

**What the revision fixed.** All 4 round-1 must-fixes are fixed. Of 30 should-fixes, 26 are fixed and 4 are not; two of those 4 are the brand sign-off, which B-42 reclassifies as a release gate. None of the 27 nice-to-haves is fixed; one is partly fixed. Of the 11 cross-cutting issues, 7 are fixed, 2 are partly fixed and 2 are open: the brand release gate and the handoff size.

**What still matters, in order**

1. **should-fix (new):** Part A didn't absorb this round's own rulings in six places, against B-37's rule that rulings reach the tables engineers read. The places: the S12 support topic, the R15 email count, the open-request lifetime, the "or" spacing and wireframes, the always-present alert paragraph, and the board list.
2. **should-fix (partly fixed CC-8):** B-43's divider rule is `form + .ri-or`. R1's commit is a `div`, so a failure line on `Recover`, `Recover-mobile` and `Recover-dark` still sits flush on "or".
3. **should-fix (carried):** two states are still copy-only even though they introduce new shapes. They are S-C's busy row action ("Taking you to Microsoft…", "Link in 41:12") and E4's other-routes variant.
4. **release gate (not a design defect):** brand and legal sign-off for the custom Google button and "Sign up with Microsoft" (B-42).
5. **nice-to-have:** canvas wiring is still missing in four places. Three boards' content runs past their canvas height (`Signin-provider-error` and its dark wrapper by 6 px since B-43, `Security-recovered` by 8 px). There are also a few polish items.

Status tables use **fixed**, **partly fixed** and **not fixed**. "Criterion n" refers to the product owner's list in order: 1 real states, 2 final copy, 3 widths and modes, 4 WCAG 2.2 AA, 5 visual consistency, 6 buildable without a design question.

## The No-ship unit

### R-C Request status pages (Recover-review, -review-cancel, -approved*, -completed, -declined, -expired)

**Round 1 7.9 No-ship → round 2 8.6 · Ship · confidence high**

- **B-36 is now a designed end state, not a phrase.** `Recover-expired` (R10c) is drawn on the status-page frame. The mascot sits at 689,64 / 164,32 and the `h1` at 166 / 134, on two balanced lines at 1440 and 390. The page has the exclamation badge and a lede that gives the reason without blaming either side ("Requests close after 7 days without a decision, so we closed yours. Your email hasn't changed."). **Start a new request** is the filled primary: there's no wait after an expiry, so it isn't R9's dated button. **Contact support** comes second, with the accessible name "Contact support, opens in a new tab" (CDP probe). The page renders cleanly in the forced-dark build and under forced colours.
- **The surrounding system is specified end to end:**
  - the lifecycle row (spec-recovery §2.3, 6f) and the open-request lifetime (§2.4);
  - emails #16 to the new address and #17 to the old (§4.1);
  - status and cancel tokens on all four routes (§3.9 "Status tokens after an expiry", §15.4);
  - the close date in the support queue (§5.1 step 1, §5.2);
  - the data model (`expiresAt`, status `expired`) and an idempotent expiry job that releases the reserved address (§9.2).

  R6, R6a, R6b and R12 each say what they show once a request has expired. Part A §3.1 and §4.9 carry the R10c row and badge.
- **R6a is drawn** (`Recover-review-cancel`). It uses the requester's words ("Cancel your request?", "Your email won't change. You can start a new request at any time.") and shows only the New email and Requested facts. **Cancel request** is a POST form with its own slot, and **Keep the request** goes back to R6. `Recover-review` and `Recover-approved` (and so `-approved-mobile`) now link to it. Tab order: Cancel request, then Keep the request.
- **Remaining debits are small.** R9's dated outline button is unchanged. R6a's second button sits 40 px under the commit, as on R12. Two email subjects for opposite outcomes read almost alike. Part A's R15 row still counts 15 emails (CC-N1).

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| must-fix | B-36's expired request had no page, actions, badge or email specification | **fixed** | `Recover-expired` board; spec-recovery §2.3 (6f), §2.4, §3.6 state row, §3.9 R10c and the token paragraph, §3.10 "already closed", §4.1 #16–#17, §5.1–§5.2, §9.2 data model and expiry job, §15.4; Part A §3.1 R10c, §4.9 |
| should-fix | The requester's Cancel request opened the owner's confirm; R6a was copy-only | **fixed** | `Recover-review-cancel` drawn; `Recover-review` and `Recover-approved` → `Recover-review-cancel.dc.html`; Keep the request → `Recover-review.dc.html` (grep) |
| nice-to-have | R9's unavailable "New request on November 12" looks like a broken outline button | **not fixed** | `Recover-declined` unchanged; spec-recovery §3.9 R9 table |

**New issues**

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| nice-to-have | On R6a, **Keep the request** sits 40 px below **Cancel request** because the reserved slot lies between them. It's the same pattern as R12 (R-D's open nice-to-have), now on a second page. | `Recover-review-cancel` at 1440: commit 466–510, slot 518–538, Keep the request from 550 | Proximity | Accept for slot stability and say so once in Part A §4.1, or move the slot under the second button on two-button confirms. |
| nice-to-have | Emails #15 and #16 have near-identical subjects for opposite outcomes. #15 is "We couldn't finish your Richie recovery" (the change is blocked and support is on it). #16 is "We couldn't finish your Richie recovery request in time" (the request is closed; start again). | spec-recovery §15.5 (#15), §4.1 (#16) | Recognition; match between system and world | Give #15 the R7a page's words: "We need to check something before changing your Richie email". |

## The Ship-with-fixes units

### E9/E10 Too many attempts (Signin-limited, -dark; Limited, -dark)

**Round 1 8.0 Ship with fixes → round 2 8.7 · Ship · confidence high**

- The drawn slot line is B-34's: "You've asked for 5 codes in an hour. Try again in 41:12." The digits are `aria-hidden` and the screen-reader text is the fixed "at 4:53 PM". The countdown button's accessible name is "Send code, available at 4:53 PM", described by "You've asked for 5 codes in an hour. Try again at 4:53 PM." (CDP; Chromium inserts its usual space before the sr span).
- One rate-limit sentence now exists everywhere. spec-edge E9, §3.3 and §7, Part A §3.5 (L1, L2 and the wording note), §4.3 and §4.4 carry B-34. B-44 settles where the countdown sits on the per-network line: L2's sentence fits the slot alone, so its countdown stays on the commit. D12 is gone from spec-edge §10.1, and no old variant survives in normative text (grep).
- "or" is now 12 px below the slot: slot 418–438 and "or" 450–470 at 1440, 386–406 and 418 at 390, 386–426 and 438 at 320. The method buttons move by the same amount on every Sign in state, so states still don't move against each other.
- What keeps it under 9: L2, waits over an hour and 0:00 are still copy-only on this layout (accepted), plus the Part A spacing lines that lag B-43 (CC-N1) and the handoff size (CC-11).

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| must-fix | Rate-limit line contradicted B-34, and the board omitted the time | **fixed** | `Signin-limited`, `-dark` slot text; spec-edge E9 table, §7; Part A §3.5, §4.3, §4.4; B-44 |
| should-fix | A filled status line sat flush on the "or" divider | **fixed** | Measured 12 px gap at 1440, 390 and 320 (B-43; `edges.css` line 523) |

No new issues.

### R-B Recovery request steps (Recover-account, -email, -code, -details*, -limited*)

**Round 1 8.1 Ship with fixes → round 2 8.7 · Ship · confidence medium-high**

- R2's L2 state uses the canonical line, "This network has made 30 attempts in an hour.", with the countdown on **Continue in 41:12**. The reason is stated in spec-recovery §3.2 and B-44. R3's L1 slot line is B-34's.
- R5 has no `maxlength` (0 occurrences on `Recover-details`). §3.5 says lengths are checked on submit with the "Use 100 characters or fewer." errors, and that pastes are never cut. This is the same call spec-settings makes for passkey names.
- The time-limit warning is drawn once (`Signin-timeout-warning`), and R2–R5 point to it (spec-recovery §3.2, §3.3, §3.5, §15.3) with their own "You have 30 more minutes." line.
- The two expiry alerts ("This took too long", "Confirm your new email again") now follow B-38: focus goes to the alert container, labelled and described. They're still copy-only on the drawn alert pattern.

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| must-fix | R2's per-network line contradicted B-34 | **fixed** | spec-recovery §3.2 L2 row; grep finds the old "tried to sign in 30 times" only in the fix log |
| should-fix | `maxlength` on R5 contradicted the too-long errors and cut pastes silently | **fixed** | `Recover-details.dc.html` has no `maxlength`; spec-recovery §3.5 |
| should-fix | The B-29 time-limit warning was never drawn | **fixed** | `Signin-timeout-warning` (see E1); spec-recovery §15.3 |
| nice-to-have | Visible "Send request at 9:15 AM" omits "tomorrow" | **not fixed** | `Recover-limited` still draws "Send request at 9:15 AM" with sr ", tomorrow" |
| nice-to-have | The first-paint expiry alerts are copy-only | **not fixed** | spec-recovery §3.2, §3.5 (copy; focus now per B-38) |

No new issues. The shared polish note on the warning's focus ring is logged under E1.

### P-A Sign in with Google and Microsoft (Main*, Signin-redirecting, Signin-provider-*)

**Round 1 8.2 Ship with fixes → round 2 8.7 · Ship · confidence high**

- The L6 alert's description is now B-34's line plus the wait: "This network has made 30 attempts in an hour. Try again in 42 minutes, or sign in with a passkey." (spec-sso §3.5). The whole minutes round up from the buttons' 41:12, as E10 does.
- P2 and P3 implement B-38 on the board. The container has `role="group"`, `tabindex="-1"`, `aria-labelledby="alert-title"` and `aria-describedby="alert-desc"` (plus `alert-ref` on P3). Chromium computes the P3 group as name "We couldn't sign you in with Microsoft" with description "The sign-in didn't finish. Try again in a moment, or use your email or a passkey. Reference 9F2C-41AB-7D03". The next step and the reference are now read with the title.
- The Google registration conflict is gone: spec-settings §9.2 and spec-sso §13.2 carry the same `AddOpenIdConnect` text and say why not `AddGoogle`. Brand sign-off is a release gate (B-42). I note it and don't deduct for design.
- **New:** B-43's 12 px pushed P3's content to 1006 px on a 1000 px board, so the footer is clipped by 6 px on the canvas in light and dark, and spec-sso §1 still lists 994.

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| must-fix | The L6 alert line contradicted B-34 | **fixed** | spec-sso §3.5 description row |
| should-fix | Page alerts focused the title only; the description and reference weren't announced | **fixed** | `Signin-provider-canceled`, `Signin-provider-error(-dark)` markup; CDP name and description above; spec-sso §3.2, §3.3, §8.4, §13.1 |
| should-fix | Brand and legal sign-off still open for the custom Google button and "Sign up with Microsoft" | **not fixed** (reclassified as a release gate by B-42; no design deduction) | Part B B-42; spec-sso OQ-6 |
| nice-to-have | The L6 state (alert + three countdown buttons) isn't drawn | **not fixed** | spec-sso §3.5 (copy) |

**New issues**

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| nice-to-have | P3's content (1006 px) now runs past its 1000 px board, so the footer is cut on the canvas, and spec-sso §1 still says 994. | `Signin-provider-error.fixed`, `-dark.fixed`: scrollHeight 1006 at 1440 (probe); spec-sso §1 table | Canvas fidelity; criterion 6 (trustworthy measurements) | Set both boards to 1100 (or 1040) and update §1's measured height. |

## Units the round-1 issues touched

### E1 Sign-in timed out (Timeout, -mobile, -dark; Signin-timeout-warning)

**Round 1 8.6 → round 2 8.8 · Ship · confidence high**

- The B-29 warning is drawn on `Signin-timeout-warning`, a full copy of `Main`. The slot reads "Still there? This page times out in 2:00." with an inline **Keep going** (`.is-focused`). The slot keeps its height: 20 / 20 / 40 px at 1440 / 390 / 320, where it runs to two balanced lines. "or" stays at 450 / 418 / 438, exactly as on `Main`, so nothing below moves.
- Tab order is Email, Send code, **Keep going**, the methods (rig keyboard pass), as spec-edge §3.6 says. The accessible text swaps the ticking m:ss for a fixed clock time, so the region never re-announces.
- **New polish:** when focused, the 2 px ring and its 5 px halo sit on the period before "Keep going". At 1440 the line reads "2:00.[Keep going]" with no visible gap.

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| should-fix | The B-29 warning (with an inline Keep going) was never drawn | **fixed** | `Signin-timeout-warning`; spec-edge §2, §3.6 (copy, markup, layout measured); Part A §4.7, §5.3 |
| nice-to-have | For a third-party client, Start again doesn't say the person is leaving Richie | **not fixed** | spec-edge E1 unchanged |

**New issues**

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| nice-to-have | The focus ring on the inline **Keep going** overlaps the period before it, so the focused line reads cramped. | `Signin-timeout-warning` at 1440 (3× crop): button at x 812, preceding text ends at 811.5 including one space; `outline: 2px` + `box-shadow: 0 0 0 5px` | Gestalt (legibility of the focused state); WCAG 2.4.13 is AAA, so this is polish only | Give `.ri-slot .ri-edge-slot-action` `margin-inline-start: 4px` (the slot height doesn't change). |

### E2 Session expired (Session-expired, -mobile; E3 alert as copy)

**Round 1 8.0 → round 2 8.7 · Ship · confidence high**

- The backdrop now shows the state the dialog talks about. `Session-expired` overlays a full copy of the dirty Profile: First name "Jay", the **Cancel**/**Save** row shown, and the messenger preview saying "Jay".
- The drawer has no grab handle. The board marks it `data-dismissible="false"`, and spec-edge E2 "Layout" and §8.1 say why.
- E3 implements B-38 (spec-edge E3, §3.4, §8.1): the alert container is focused, labelled by "You were signed out" and described by the description, so "We'll put back what you typed" is read.
- Drawer Tab order is resolved with the alternative round 1 offered. B-41 makes the wrapper's visual order normative and its DOM order the shared desktop DOM. spec-edge §2 and §5 say "DOM order: commit first below 768 (B-30)", and §8.1 tells the build to render **Sign in again** first. The board still Tabs Go to Richie, then Sign in again (rig keyboard pass at 390), and nothing on the board itself says that order isn't normative (CC-7).

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| should-fix | The backdrop contradicted the dialog (saved values, no dirty row) | **fixed** | `Session-expired` at 1440 and 390 (screenshots); spec-edge §2 board row, E2 "Page behind (drawn)" |
| should-fix | A grab handle on a drawer that can't be dismissed | **fixed** | `Session-expired-mobile` at 390: no handle; `data-dismissible="false"`; spec-edge E2 Layout, §8.1 |
| should-fix | Bottom-up Tab order on the mobile drawer board | **fixed** (by the stated alternative: B-41 marks the board's DOM order non-normative in its spec rows; the board's own Tab order is unchanged) | spec-edge §2, §5 E2 row, §8.1; Part B B-41; rig keyboard pass at 390 |
| should-fix | E3's alert focused only its title | **fixed** | spec-edge E3, §3.4, §8.1; layout `Signin-provider-canceled` (see P-A) |
| nice-to-have | No dark board for the dialog | **not fixed** | Forced-dark build only (rendered correctly) |

No new issues.

### E5 Link no longer works (Error-link)

**Round 1 8.4 → round 2 8.7 · Ship · confidence high**

- Both suggested routes are in place. E5's note ends "Lost the email? Contact support." (topic `recovery-request`; AX name "Contact support, opens in a new tab"). R2's open-request error adds "Yours? Use the link in your latest status email, or contact support to get it again." ahead of "Not yours? …" (spec-recovery §3.2). An honest requester whose status link died is no longer treated as the suspect.
- The note stays two lines at 1440 and at 390. The page is otherwise unchanged and clean.

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| should-fix | An honest requester's primary led to an error that treated them as an intruder | **fixed** | `Error-link` note; spec-edge E5 table; spec-recovery §3.2 "Open request" row; §15.1 topic list |
| nice-to-have | No 390 or dark board | **not fixed** | Harness only (B-22) |

No new issues.

### E7/E8 Offline (Signin-offline, -dark; Offline)

**Round 1 8.5 → round 2 8.7 · Ship · confidence high**

- The offline failure line now groups with **Send code**: slot 418–438 and "or" 450–470 at 1440, 386–406 and 418 at 390, with the same y on `Main`, `Signin-limited`, `Signin-redirecting` and `Signin-timeout-warning`.
- E8 is unchanged and still has no mobile board.

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| should-fix | A filled status line sat flush on "or" | **fixed** | Probe at 1440 / 390 / 320; `edges.css` `form + .ri-or { margin-top: 12px }` (B-43) |
| nice-to-have | Offline has no mobile or dark board | **not fixed** | No `Offline-mobile` |

No new issues. The same defect survives on R1; it's logged under R-A.

### R-A Can't get the code (Recover, -mobile, -dark, Verify-help)

**Round 1 8.6 → round 2 8.7 · Ship · confidence high**

- The two stale normative sentences are gone. spec-recovery §2.2 step 4 now says step-up is a passkey or an email code, and that a provider-only person signs in with the provider again (B-18). §7.3 says screen C offers **Request account recovery** (B-25).
- One code-screen grammar (B-39) is written once in Part A §4.7 and drawn on three boards:
  - `Verify-help` Tabs code, Verify code, Can't get the code?, Use a different one;
  - `Provider-link-code` Tabs code, Link Microsoft, Cancel, Use a passkey instead, Can't get the code?;
  - `Recover-code` Tabs code, Continue, the note's contact support, Use a different email.

  Each lede names its purpose and runs two lines at 390.
- **New:** B-43's fix doesn't reach R1. The rule is `form + .ri-or`, and R1's commit is a `div.ri-commit` (it holds the **Back to the code** link). So R1's slot still sits flush on "or". With "The request timed out or you canceled it." injected (a specified R1 state), the red line reads as the divider's caption at 1440 and 390.

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| should-fix | Normative text contradicted rulings (step-up methods; settings screen C) | **fixed** | spec-recovery §2.2 step 4, §7.3; grep |
| should-fix | Code-entry screens used two grammars and two Tab orders | **fixed** | Part A §4.7 code-screen grammar; rig keyboard passes on the three boards |

**New issues**

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| should-fix | R1's status slot still sits flush on the "or" divider, so a failure line (passkey canceled, passkey failure, provider offline) captions the divider instead of the button. | `Recover`, `-mobile`, `-dark`: slot 342–362 and "or" 362–382 at 1440; 310–330 and 330–350 at 390 (probe; injected alert line, screenshot). `edges.css` line 523 matches only `form + .ri-or` | Gestalt proximity; criterion 5; B-43 | Widen the rule to `:is(form, .ri-commit) + .ri-or { margin-top: 12px }` and update Part A §4.1 (see CC-N1). |

### R-D Old-address alert and cancel (Email-recovery-alert, Recover-cancel, Recover-owner-view, Recover-canceled)

**Round 1 8.3 → round 2 8.7 · Ship · confidence high**

- The cancel path now has one button name everywhere ("Cancel this request", B-10). spec-recovery §4.3 states the email CTA once (a pill, `border-radius` 22 px, VML `roundrect`), and §2.1, §2.3, §5.1 and §7.4 no longer say "This wasn't me" (grep: it survives only on S6, as B-10 intends).
- R6b is drawn (`Recover-owner-view`), in the owner's words: "We're reviewing this request". It shows the masked New email (sr "j, hidden, at rivera.studio"), no Account row, and **Cancel this request** back to R12. R12's **Keep the request** now opens it, so the old mailbox learns nothing beyond what the email already showed.

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| should-fix | Stale normative text contradicted B-10 and the board | **fixed** | spec-recovery §2.1, §4.3, §5.1, §7.4; grep |
| should-fix | After Keep the request the canvas showed the owner the requester's view with the full new address | **fixed** | `Recover-cancel` → `Recover-owner-view.dc.html`; `Recover-owner-view` markup; spec-recovery §3.6 R6b, D-R10 |
| nice-to-have | R12's 40 px reserved slot detaches Keep the request | **not fixed** | `Recover-cancel` at 1440: primary 542–586, slot 594–614, Keep from 626 (now repeated on R6a, see R-C) |

No new issues.

### R-E Recently recovered (Security-recovered)

**Round 1 8.6 → round 2 8.6 · Ship · confidence high**

- R14 gained B-38. The alert `section` takes focus with `aria-labelledby` and `aria-describedby`. Chromium names it "Your email was changed by account recovery on November 8" and reads the checklist as its description.
- Neither round-1 nice-to-have was taken. The content still measures 1808 px on the 1800 px board, and the alert still asks to check passkeys without linking **Add a passkey**. The score stays.

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| nice-to-have | Board height is 8 px short of its content | **not fixed** | `Security-recovered.fixed`: scrollHeight 1808, canvas 1440 × 1800 (probe) |
| nice-to-have | The alert doesn't link Add a passkey | **not fixed** | `Security-recovered` alert (screenshot); spec-recovery §3.12 |

No new issues.

### S-A Passkey management (Security, -mobile, -dark, -passkey-added, -passkey-limit, -rename*, -remove-failed, -load-error*)

**Round 1 8.6 → round 2 8.8 · Ship · confidence high**

- The wrap contradiction is gone. spec-settings §2.2 and the §11 "Resolved" bullet both say passkey names wrap freely (`.wrap-all`, no clamp); 40 characters take one line at 1440 and two at 390 and 320.
- `Security-rename-error-mobile` draws **Save** first in its footer DOM (`.ri-set-commit-first`, B-41). Tab: Name, Save, Cancel.
- The base boards' missing `role=alert` paragraph is closed by ruling B-40. Alert lines may be inserted when they occur; only `role=status` must exist at load. I accept the reason: `role="alert"` is announced on insertion by the major screen readers, and it's the one live role that doesn't need a pre-existing region. spec-settings §1.4 says the same.
- The two nice-to-haves are unchanged. The quoted name still breaks as "the “Pixel / 8” passkey" at 390, and the too-long error doesn't give the count.

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| should-fix | The spec said both wrap freely and wrap-2 | **fixed** | spec-settings §2.2, §11; grep finds `wrap-2` only in the historical Part A §7 brief and §13's changelog-style note |
| should-fix | Base `Security` drew only `p[role=status]` while failures use `role=alert` | **fixed** (closed by ruling B-40, reason accepted) | Part B B-40; spec-settings §1.4 |
| nice-to-have | The failure line splits the quoted passkey name | **not fixed** | `Security-remove-failed` at 390 (screenshot) |
| nice-to-have | The too-long error doesn't give the count | **not fixed** | spec-settings §2.1 "Use 40 characters or fewer." |

No new issues.

### S-B Email settings (Profile, Profile-email-*)

**Round 1 8.5 → round 2 8.8 · Ship · confidence high**

- The 10-addresses-an-hour check is a real row in spec-settings §3.1: the 11th **Send code** is refused, the commit counts down, and the slot reads "You've tried 10 addresses in an hour. Try again in 41:12." Editing the address doesn't clear it, the 0:00 line is given, and the layout reference is `Profile-email-taken`.
- `Profile-email-code-mobile` draws **Change email** first in its footer DOM. Tab: code, Use a different email, Change email, Cancel. `Profile-email-taken-mobile` is a wrapper, so it keeps the shared DOM (field, Cancel, Send code) under B-41, and its spec row says so.
- The measurement note is half corrected. §12.4 now names the two-line exception ("That email already belongs…"), but §8's table row for `.ri-set-field-slot` still says "longest string 1 line at 400".

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| should-fix | The address-check limit's copy lived only in an open question | **fixed** | spec-settings §3.1 "Too many address checks" row; §6.3; §10 Q-S4 |
| should-fix | Bottom-up Tab order on the mobile drawer boards | **fixed** | `Profile-email-code-mobile` keyboard pass at 390; wrapper documented per B-41 (§7 rows) |
| nice-to-have | The §8 measurement claim didn't match the render | **partly fixed** | §12.4 corrected; §8 `.ri-set-field-slot` row still "longest string 1 line at 400" |

**New issues**

| Severity | Issue | Evidence | Principle | Change |
|---|---|---|---|---|
| nice-to-have | B-39 lists S5 step 2 among the code screens whose lede names the purpose, but its description is still "Enter the code we sent to jamie@rivera.studio." The dialog title "Check your new email" carries the purpose, so the impact is small. | `Profile-email-code-mobile` at 390 (screenshot); spec-settings §3.2 table; Part B B-39; Part A §4.7 | Consistency (B-39); WCAG 2.4.6 | Either use "…to confirm your new email." as R4 does, or state in spec-settings §3.2 that the dialog title carries the purpose. |

### S-C Google and Microsoft in settings (Security section, -linked, -link-error, -unlink)

**Round 1 8.3 → round 2 8.6 · Ship · confidence medium**

- The specs agree on one registration. spec-settings §9.2 and spec-sso §13.2 have identical text: `AddOpenIdConnect` for both providers, the claim mapping, and `CallbackPath` `/signin-google` and `/signin-microsoft`. Both explain why not `AddGoogle` or `AddMicrosoftAccount`.
- The provider-error note's support link is `topic=provider&ref=9F2C-41AB-7D03&from=S8` (§4.2), and `provider` is in spec-recovery §15.1's fixed list. The base-slot alert paragraph is closed by B-40, as for S-A.
- Still undrawn: the busy row action ("Taking you to Microsoft…", 204 px) and **Link in 41:12** are new row-action shapes. They're measured but only copy, and at 390 the row action already wraps under the text. The empty reserved line in the not-linked Microsoft row is unchanged.

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| should-fix | The two specs gave different Google registrations | **fixed** | spec-settings §9.2 = spec-sso §13.2 (same text) |
| should-fix | The busy row action and "Link in 41:12" are measured, never drawn | **not fixed** | spec-settings §1.2, §4.1, §6.3 ("copy"); no `Security-linking` board |
| should-fix | The provider-error support link had no topic | **fixed** | spec-settings §4.2; spec-recovery §15.1 `provider` |
| should-fix | Base `Security` foot lacked the `role=alert` paragraph | **fixed** (closed by ruling B-40, reason accepted) | Part B B-40 |
| nice-to-have | The reserved second line leaves an empty band in not-linked rows at 1440 | **not fixed** | `Security` `#providers` at 1440 (screenshot) |

No new issues.

### P-B Welcome back with a provider (Signin-remembered-provider)

**Round 1 8.3 → round 2 8.6 · Ship · confidence medium-high**

- The chip no longer truncates the person's own address. At 390 it wraps after the @ ("jamie.rivera@" / "northwind.coffee"), the stray gap is gone, and spec-sso §4 states the rule for every width (P-B; `edges.css` §9). It's one line at 1440.
- The P2, P3 and L6 alerts that can appear on A4 follow B-38.
- Unchanged: Tab reaches **Not you?** before **Sign in with Google**. That follows the visual order, so it's accepted, but the page's only action still comes second. There's still no dark or 390 board for a page mostly seen on phones.

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| should-fix | The chip truncated the person's address at 390 | **fixed** | `Signin-remembered-provider` at 390 (screenshot); spec-sso §4 chip row |
| nice-to-have | No dark or 390 board | **not fixed** | Harness only (B-22) |

No new issues.

### P-E Link a provider at sign-in (Provider-link*, Provider-link-code)

**Round 1 8.4 → round 2 8.8 · Ship · confidence high**

- P6 says what the code is for before the button. The lede is "Enter the code we sent to jamie.rivera@northwind.coffee to link Microsoft." (two lines at 390, wrapping after the @), and the commit is **Link Microsoft**.
- P6 follows the shared code-screen grammar. **Cancel** is stacked under the commit and the alternatives are one switch line below it. Tab: code, Link Microsoft, Cancel, Use a passkey instead, Can't get the code?. Nothing sits between the code and the commit.

| Severity | Round-1 issue | Status | Evidence |
|---|---|---|---|
| should-fix | P6 didn't say what the code is for until the commit | **fixed** | `Provider-link-code` lede; spec-sso §7.3 |
| should-fix | Code-entry screens used two grammars | **fixed** | Part A §4.7 (B-39); rig keyboard pass at 390 |

No new issues.

## Spot-checks for regressions

For each unit below I checked the rig results in both builds, the frame probe (mascot and `h1` at 1440 and 390), the Tab order where it applies, and the board-to-spec deltas that touch the unit. None regressed. Each keeps its round-1 score.

| Unit | Round 1 | Round 2 | What I checked | Round-1 issues still open |
|---|---|---|---|---|
| E4 | 8.6 | 8.6 | `Error`, `-mobile`, `-dark`: 0 blockers, 0 majors; mascot 689,64 / 164,32, `h1` 166 / 134; the reference slot at 398–418 (1440) | should-fix: the other-routes variant (two buttons, reference, slot, note) is still copy-only, **not fixed**. nice-to-have: Go to Richie loops when Richie's own client fails, **not fixed** |
| E6 | 8.8 | 8.8 | `Error-notfound`: clean; frame unchanged | nice-to-have: both buttons end on Sign in for a signed-out visitor, **not fixed** |
| E11 | 8.7 | 8.7 | `Signedout-prompt`: clean; slot 442–462 (1440), 434–454 (390); frame unchanged | nice-to-have: E12 busy copy-only, **not fixed**; no mobile or dark board, **not fixed** |
| E13 | 8.8 | 8.8 | `Signedout`, `-mobile`, `-dark`, `-client`: clean; frame unchanged | nice-to-have: sidebar Sign out still a `<button>` with no target, and the note still → `Security.dc.html`, **not fixed** |
| E14 | 8.6 | 8.6 | `Signedout-everywhere`: clean; `h1` 2 lines at 390 | nice-to-have: no email for "Sign out of every device" (QE-3), **not fixed**; no mobile or dark board, **not fixed** |
| P-C | 8.4 | 8.4 | `Signup`, `-mobile`, `Signup-finish`, `-mobile`: clean; B-43 doesn't stack on A2 (the legal-line rule keeps "or" 24 below the legal line at 1440, 390, 320); A3's code step now has the B-39 lede in spec-sso §6.3 | should-fix: brand sign-off, **not fixed** (now a release gate under B-42; I'd stop deducting for it, but kept the round-1 score as the spot-check rule asks). nice-to-have: the A3 code step is copy-only, **not fixed**; the legal line floats about 40 px below the commit, **not fixed** |
| P-D | 8.6 | 8.6 | `Provider-no-account`, `-no-email`, `-not-linked`: clean; `h1` 2 lines at 1440 and 390, frame unchanged | nice-to-have: P4's primary looks like a provider trip, **not fixed**; no mobile or dark boards, **not fixed** |

## Overall set

**Round 1 8.1 Ship with fixes → round 2 8.6 · Ship · confidence high**

The revision did what round 1 asked, and more. Buildability, the dimension that held the set at 8.1, is now mostly sound:

- one precedence order (B-37) is stated word for word in all five headers;
- B-34 to B-36 and the round's new rulings (B-37 to B-44) are written into the domain page tables;
- the eight stale sentences round 1 listed are gone;
- the one drawn board that contradicted a ruling is corrected.

The worst state gap, B-36's expired request, is now one of the best-specified states in the area: page, badge, actions, emails, tokens, support queue and job. The accessibility debts are paid. Page alerts focus a labelled and described container. The time-limit warning is drawn and measured. One code-screen grammar fixes Tab order on Verify, R4 and P6. The 390-only drawers put the commit first in the DOM.

Across 84 boards × 10 viewports × light and forced-dark, the rig still finds 0 blockers and 0 majors. The 619 minors per build are the same three known classes as round 1:

- 557 targets under 44 px;
- 23 inline links under 24 px;
- 39 axe "needs review" contrast items.

The frame hasn't moved: mascot 689,64 and `h1` 166 at 1440; 164,32 and 134 at 390; 129,32 and 134 at 320. Every new board sits on it.

What keeps the set at 8.6 rather than 8.8:

- **Part A lags its own round in six places** (CC-N1). Part A is lowest in precedence, so these don't block a build, but they break B-37's own rule.
- **B-43's selector misses R1** (CC-8, R-A).
- **Two should-fix states are still copy-only** although they introduce new shapes: S-C's busy row and E4's other routes.
- **The handoff grew** from 4,424 to 4,639 lines with no per-page sheet (CC-11).
- **Canvas wiring and two board heights** are small debts (CC-9, CC-N2).

None of these is a must-fix, and none needs a design question to resolve.

### Cross-cutting issues: round-1 status

| # | Severity (round 1) | Issue | Status | Evidence |
|---|---|---|---|---|
| 1 | must-fix | Circular precedence; B-34–B-36 never reached the domain specs; a drawn board contradicted B-34 | **fixed** | B-37 text in spec.md line 7 and Part B intro, spec-edge line 5, spec-settings line 5, spec-recovery line 5, spec-sso line 3; one L1 and one L2/L6 line in every domain spec; `Signin-limited` corrected. Residue in Part A logged as CC-N1 |
| 2 | must-fix | B-36's expired request had no design | **fixed** | See R-C |
| 3 | should-fix | First-paint alerts focused the title only | **fixed** | B-38; P2, P3, R14 boards (CDP names and descriptions); E3, L6 and the recovery expiry alerts in their specs |
| 4 | should-fix | The WCAG 2.2.1 warning was never drawn | **fixed** | `Signin-timeout-warning`; spec-edge §3.6; Part A §4.7, §5.3 |
| 5 | should-fix | Code screens disagreed on alternatives and Tab order; P6 lacked its purpose | **fixed** | B-39; Part A §4.7; `Verify-help`, `Recover-code`, `Provider-link-code` keyboard passes |
| 6 | should-fix | At least eight stale or contradictory normative sentences | **fixed** | All eight cleared (grep; R-A, R-B, R-D, S-A, S-C sections). New stale lines are listed in CC-N1 |
| 7 | should-fix | Mobile drawer boards Tab bottom-up | **fixed** | 390-only boards (`Security-rename-error-mobile`, `Profile-email-code-mobile`) draw the commit first in the DOM. Wrapper boards (`Session-expired-mobile`, `Profile-email-taken-mobile`) are declared non-normative for DOM order in their spec rows (B-41), the alternative round 1 offered. Residue: the wrappers themselves carry no marker |
| 8 | should-fix | A filled status line touched "or" | **partly fixed** | Fixed on all nine Sign in-family boards (12 px, measured). Missed on `Recover`, `-mobile`, `-dark`: the rule is `form + .ri-or` and R1's commit is a `div` (see R-A) |
| 9 | should-fix | Canvas links misrepresented journeys | **partly fixed** | The two misleading links are fixed (R6a and R6b drawn and relinked). Still unwired: the sidebar Sign out (`<button>`), the `Signedout` note (→ `Security`, spec says → Sign in), and the Security Rename, Remove and Unlink triggers (no overlay targets). R6a's form posts to `#`, so J23 stops before R10b (copy-only). These are walkability only: now nice-to-have |
| 10 | should-fix | Brand sign-off open | **not fixed** (reclassified by B-42 as a brand and legal release gate; noted, no design deduction) | Part B B-42; spec-sso OQ-6; Part A §8 Q3–Q4 |
| 11 | nice-to-have | Handoff about 4,400 lines in five layered files; normative content under "Phase 4 additions" | **not fixed** | Now 4,639 lines (spec.md 1,198; spec-edge 895; spec-settings 725; spec-recovery 1,047; spec-sso 774); spec-recovery §15 still holds the support contract; no per-page sheet |

### New cross-cutting issues

| # | Severity | Issue | Evidence | Principle | Change | Units |
|---|---|---|---|---|---|---|
| N1 | should-fix | Part A didn't absorb this round's rulings in six places, against B-37's own rule that rulings reach the tables engineers read. Precedence settles each one, but an engineer reading Part A's grammar would build the wrong divider spacing. | (1) §3.1 S12 row: the topic is "not in spec-recovery §15.1's fixed topic list; not settled", although B-35 settled it. (2) §3.1 R15 row: "15 in all", without #16 and #17. (3) §3.5 limits-and-lifetimes "Request open time": "spec-recovery §2.4 has no lifetime row", which it now has. (4) §4.1 spacing "slot → 'or' 0" and both wireframes with "or" at 438 / 406, against B-43's 12 px and the measured 450 / 418. (5) §4.6 "an always-present … `role=alert`" against B-40. (6) §5.1 and §5.2 say 80 boards and omit the four new boards (also spec-edge §12.4 and spec-sso §15 layout tables at 438 / 406; spec-settings §8 field-slot row) | Criterion 6 | One sync pass over Part A §3.1, §3.5, §4.1, §4.6 and §5.2 against B-35 to B-44; delete rather than annotate | All; most visibly E7/E8, E9/E10, R-A, R-C |
| N2 | nice-to-have | Three boards' content runs past their canvas size, so the bottom is cut on the canvas. On P3 only the footer's bottom padding is lost; the links stay visible. | `Signin-provider-error`, `-dark`: 1006 on 1000 (new since B-43); `Security-recovered`: 1808 on 1800 (round 1, unchanged). Every other desktop board fits; the 390 wrappers scroll by design (probe of all 84 against `canvas.json`) | Canvas fidelity | Grow the three boards (to 1100 and 1900) and update spec-sso §1 | P-A, R-E |

### Remaining issues, prioritized

1. **should-fix (CC-N1):** sync Part A to B-35 to B-44 (six places).
2. **should-fix (CC-8, R-A):** widen B-43's rule to `:is(form, .ri-commit) + .ri-or` so R1's failure lines group with **Back to the code**.
3. **should-fix (S-C):** draw `Security-linking` (the busy row and **Link in 41:12**) at 1440 and 390.
4. **should-fix (E4):** draw the other-routes variant (two buttons, reference, slot, note) at 1440 and 320.
5. **release gate (CC-10, B-42):** brand and legal sign-off for the custom Google button and "Sign up with Microsoft" before build freeze. The fallback label is ready as a text swap.
6. **nice-to-have (CC-9, CC-N2):** wire the sidebar Sign out → `Signedout`, the `Signedout` note → `Main`, and the Security triggers → their overlays. Fix the three board heights.
7. **nice-to-have (polish):**
   - **Keep going**'s focus ring on the preceding period (E1);
   - the quoted "Pixel / 8" (S-A) and the empty not-linked band (S-C);
   - the detached second button on two-button confirms (R12, R6a);
   - the #15 and #16 email subjects (R-C);
   - S5 step 2's lede without its purpose (S-B);
   - the unchanged round-1 nice-to-haves listed in each unit.
8. **nice-to-have (CC-11):** after the Part A sync, generate one per-page sheet (route, title, copy, states, focus, live regions, limits).

### By criterion

| Criterion | Assessment |
|---|---|
| 1. Real states | Met. The round-1 gaps are closed: B-36's expired request, the time-limit warning, E2's unsaved backdrop, R6a and R6b. Copy-only states remain where they're text swaps on drawn layouts. Two new-shape states are still copy-only: S-C's busy row and E4's other routes. |
| 2. Final copy | Met. One rate-limit sentence per limit everywhere; the cancel path uses one button name; the new pages read plainly and give real numbers. Brand labels are a release gate (B-42), not a copy gap. |
| 3. 360 to desktop, light and dark | Met. No horizontal overflow on any board at 320–1920 or at 200 % in either build. The new boards render correctly in the forced-dark build and under forced colours. Text spacing (1.4.12) is clean on the 14 new or changed boards I probed at 320 and 1440. |
| 4. WCAG 2.2 AA | Met. Rig: 0 critical or serious axe findings in both builds. The alert containers are labelled and described (verified in Chromium's accessibility tree). Countdown names are stable. **Keep going** is the next Tab stop after the commit. The code screens and the 390-only drawers have a sensible focus order. B-40's reason for inserting `role=alert` lines is sound. The wrapper drawers' bottom-up board order is declared non-normative (B-41). |
| 5. Visual consistency | Strong. One frame on every identity board, and settings states are faithful copies of the shipped boards. Remaining spacing debts: R1's divider and the detached second button on two-button confirms. |
| 6. Buildable without a design question | Mostly met. One precedence order, rulings in the page tables, B-36 fully specified. Held back by Part A's lag (CC-N1), the handoff size (CC-11) and the two undrawn should-fix states. |

## Method and limits

**What I did**

- **Read** Part A and Part B of `spec.md` and sections 1–13 (or 1–16) of each domain spec. I also read spec-recovery §15, which B-19 makes normative, and spec-settings §0. I skipped every "Fix log", "Changelog", "Revision round (Phase 5)" and "Revisions" section, as instructed, and treated Part A §7 (marked historical) as background. I applied the precedence B-37 sets: Part B rulings, then domain specs, then Part A.
- **Grepped** the five specs for every stale string round 1 named ("This wasn't me", radius 10, "has received 5 codes", "tried to sign in 30 times", "failed sign-in attempts", `wrap-2`, `AddGoogle`/`AddOpenIdConnect`) and for the Part A lines in CC-N1.
- **Built** all 84 boards with the rig's `build.mjs`, light and `BOARD_THEME=dark` (0 warnings each), into the session scratchpad (`regrade/`). I then ran `run.mjs` on both builds: 10 viewports, two schemes, axe, overflow, clipping, targets and keyboard passes. Both runs: 0 blockers, 0 majors, 619 minors. I also ran a `--forced-colors` pass on the four new boards and checked their screenshots. Its 42 "majors" are the known axe artefact of authored colours measured against forced black; light forced runs and the renders are correct.
- **Ran my own Playwright probes:**
  - mascot, `h1`, commit, slot and "or" positions on 29 boards at 1440 and 390 (15 of them also at 320);
  - content height against `canvas.json` for all 84 boards;
  - the time-limit warning's geometry and focus ring (3× crop);
  - an injected failure line in R1's slot;
  - Chromium accessible names and descriptions through CDP for the P2, P3 and R14 alert containers, the countdown button, **Keep going**, R10c's support button, R6a's commit, the E2 dialog and E5's note link;
  - WCAG 1.4.12 text spacing at 320 and 1440 on 14 new or changed boards.
- **Took screenshots** of 24 boards at the widths that matter for each (the four new boards at 1440, 390 and 320, and in the dark build at 1440 and 390), plus element crops where a detail mattered. The rig's own screenshots cover every board at 10 viewports. I looked at the renders cited in this report; for the other boards I relied on the rig and the probes.
- **Checked canvas links** by grep across the board files.
- I did not open anything under `docs/design/identity-edges/qa/`, `wbs.md`, `inventory.md`, `docs/design/how-we-design.md`, `.design/edges/qa/`, `CANVAS-CHANGES.md` or `CSS-REQUESTS.md`. I edited no design, spec or stylesheet, and committed and published nothing.

**Limits**

- The boards are static. Runtime behaviour (live-region timing, the warning's countdown and renewals, focus moves, bfcache, the service worker, the expiry job) is judged from markup and spec.
- I tested Chromium only. On this Mac the system stack resolves to SF Pro, so the renders use SF, not Inter. I didn't test Safari, Firefox or real devices. Accessible names and descriptions come from Chromium's accessibility tree, not from VoiceOver, NVDA or JAWS. Forced colours were emulated.
- I used the local board mirror, stated to match canvas version `1793900431-dcb1`, rather than the live canvas.
- I took B-40's and B-41's resolutions on their stated reasons. For B-40 I agree on technical grounds. For B-41 I note that the wrapper boards still Tab bottom-up on the canvas.
- The spot-checked units weren't re-graded from scratch. They keep their round-1 scores because I found no regression; their open round-1 issues are listed.
- Scores are one reviewer's judgement on round 1's scale. A unit moves from its round-1 score by the weight of each issue fixed or added (roughly 0.4–0.5 for a must-fix, 0.15–0.25 for a should-fix, 0.05 for a nice-to-have), and the rest of the distance to 10 traces to its open issues and the cross-cutting issues it cites (most often CC-N1 and CC-11).
