# Richie identity edges: settings gaps (Part C, WP3.3)

WP3.3, Richie Design Program, identity edges workstream. **Revision 2, 2026-11-05** (Phase 4 fix round: every WP3.3 finding and the PM rulings B-6 to B-33 that land here; the fix log is §14; the Phase 5 revision round is written into the sections and listed at the end). For the PM who assembles the canvas, the testers, the evaluator and engineering.

**What this file covers.** The states of passkey management and change email that the shipped settings canvas (8.7/10 Ship) left as copy or didn’t cover, and every state of the new **Google and Microsoft** section that WP3.1 added to Sign-in and security. Part A ([`spec.md`](spec.md)) is the foundation; departures are listed in §11 with reasons. **Precedence (one order for all five files, ruling B-37):** Part B rulings in `spec.md`, then the domain specs’ page tables and recorded departures, then Part A (Revision 2 synced it to both). A board that disagrees with its spec is a bug in the board. Everything already decided in the settings spec ([`../account-settings/spec.md`](../account-settings/spec.md), Revision 5, cited as **AS §**) applies unchanged and isn’t restated: the shared grammar (AS §4.0), the step-up screens and code rules (AS §4.4), the board architecture (AS §6.1) and the CSS (AS §7). This file gives the deltas and the new states, with final copy.

**Summary.** Sixteen boards fill the gaps (fifteen, plus `Security-load-error-dark` from ruling B-21). Passkeys get a rename dialog (default and too-long), the added state, the 10-passkey limit with two names at the 40-character limit, a list that didn’t load, and a removal that failed inside its confirm. Change email gets the "already belongs to another Richie account" error (desktop and drawer), the "New code sent" drawer and the done state. Google and Microsoft get the section at 390 and in dark, the return from Microsoft when linking worked and when that Microsoft account already belongs to another Richie account, and the unlink confirm. Every other state is copy on a layout already drawn (§6). `Profile` and `Account-sidebar` are unchanged. **`Security` changed in the Phase 4 fix round** (allowed by the PM for RIG-05): the Microsoft row that isn’t linked reserves two description lines, passkey names and activity titles wrap instead of clamping, and the passkey buttons’ names quote the passkey (§14). Every full copy was regenerated from it.

**Sample data** is Part A §4.12 and AS §10.1: Jamie Rivera, `jamie.rivera@northwind.coffee`, today Thursday, November 5, 2026, signed in with a passkey at 9:14 AM on Chrome on macOS in Portland, Oregon. Google `jamie.rivera@gmail.com` linked October 2; Microsoft (Jamie’s work account, `jamie.rivera@northwind.coffee`) not linked on `Security`, linked at 9:31 AM on `Security-linked`. New email `jamie@rivera.studio` (AS’s target). The address that already has an account: `hello@rivera.studio` (a shared studio inbox someone else already uses for Richie).

---

## Contents

0. [What this file amends in the settings spec](#0-what-this-file-amends-in-the-settings-spec)
1. [Rules every state here follows](#1-rules-every-state-here-follows)
2. [Passkeys (S1–S4)](#2-passkeys-s1s4)
3. [Change email (S5–S6)](#3-change-email-s5s6)
4. [Google and Microsoft (S7–S10)](#4-google-and-microsoft-s7s10)
5. [Activity entries and security emails](#5-activity-entries-and-security-emails)
6. [State matrix](#6-state-matrix)
7. [Boards](#7-boards)
8. [CSS (`edges.css`, WP3.3 section)](#8-css-edgescss-wp33-section)
9. [Handoff mapping](#9-handoff-mapping)
10. [Open questions, with defaults](#10-open-questions-with-defaults)
11. [Departures, and the questions Part A asked me to resolve](#11-departures-and-the-questions-part-a-asked-me-to-resolve)
12. [Test results](#12-test-results)
13. [For the other WPs and the PM](#13-for-the-other-wps-and-the-pm)
14. [Fix log (Phase 4)](#14-fix-log-phase-4)

---

## 0. What this file amends in the settings spec

When AS is next revised, these are the changes, by section. Each row points at the section of this file with the final text.

| AS section | Change | Here |
|---|---|---|
| §4.0 Dialogs | A form dialog with a field reserves the field’s message slot (1 line in a 400 px dialog body, 2 in drawers), so an error never changes its height (rename, change email step 1). A confirm’s action failure is one centred status line inside the confirm’s header, icon inline before the first word; on phones it stays in the action sheet’s top piece | §1.4, §2.1, §2.4 |
| §4.0 Failure phrases | New rows (ruling B-6: the button’s verb): "link Microsoft" / "link Google" (Link, before the redirect and after the return, section status slot); "unlink Google" (inside the confirm, or inside step-up when it ran) | §1.3 |
| §4.0 Busy labels | "Removing…" (remove passkey), "Unlinking…" (unlink), "Taking you to Microsoft…" (Link), "Trying again…" (load error). Each is also written once into the surface’s polite paragraph, sr-only (B-28) | §1.2 |
| §4.0 Screen-reader rules | Names in sr suffixes are quoted: "Rename the “Pixel 8” passkey" (A11Y-13). Countdown buttons keep the digits `aria-hidden` and carry a fixed clock time: "Link Microsoft, available at 10:42 AM" (B-27). Return-from-provider outcomes, errors included, are written into the polite paragraph (A11Y-15) | §1.4 |
| §4.0 Dates | Relative dates stop at "yesterday" (B-16); every email time names the zone, "9:31 AM Pacific Time" (B-8) | §5.2 |
| §4.0 Row (AS §7.2 row 8) | Passkey names and activity titles wrap freely (`.wrap-all`); the 2-line clamp cut the third line under WCAG 1.4.12 text spacing at 320 (RIG-03). Device names keep `.wrap-2` | §2.2 |
| §4.2 Change email | Step 1: the field slot, the send limit, the busy state. Step 2: the locked and expired descriptions; "New code sent. Use the code in the newest email." (Part A §2.1) stays until the first character is typed (AS §4.4 said 5 s); a confirm failure keeps the code; the race where the address is taken between the steps. Done: focus, the activity entry, `signalCurrentUserDetails()` fields | §3 |
| §4.2 Done, "a security email goes to the old address" | Subject, body and button written; the button is **This wasn’t me** | §3.4 |
| §4.3 Lede and sections | WP3.1’s lede and the Google and Microsoft section (Part A §5.2); this file adds the section’s states | §4 |
| §4.3 Add-a-passkey states | Default name and provider line rules (security keys: the model as the name, "Security key" as the provider line); the 10th passkey (focus to the new row’s **Rename**); the limit as a quiet line on load; passkey prompt failure copy is Part A §2.1’s "The request timed out or you canceled it." | §2.2 |
| §4.3 Rename | Text selected on open; trimmed; unchanged name closes without saving; focus back to the row’s **Rename**; activity entry; no email | §2.1 |
| §4.3 Remove | The description lists every way in that remains, linked providers included ("…your other passkey, Google or an email code."); busy "Removing…"; the toast description depends on the provider; call `signalAllAcceptedCredentials()`, not `signalUnknownCredential()` | §2.4 |
| §4.3 Load error | Title "We couldn’t load your passkeys" (COPY-13); `role="alert"` on the title and description only (A11Y-14); offline and server variants, Try again behaviour, focus, the other lists’ titles; **Add a passkey** hidden until the list loads | §2.3 |
| §4.3 Google and Microsoft rows | A row that isn’t linked reserves two description lines, so linking doesn’t move the sections below (RIG-05) | §4.1 |
| §4.4 When it runs | Required also for **link** and **unlink** a provider (Part A §8 Q20); descriptions for both; action failures after a passkey step-up and after a code step-up | §4.4 |
| §4.3 Recent security activity | Entries for add, rename, remove, link, unlink, provider sign-in and email change | §5.1 |
| §5 State matrix | Sign-in and security and Profile cells that were "copy" now name a board (§6) | §6 |
| §7.2 row 15 Emails | Short local parts (5 characters or fewer) drew a gap before the @: `ri-set-email-short` (CSS-REQUESTS WP3.3-1) | §8 |
| §7.2 row 17 Alert | The `.ra-alert-action` the spec names isn’t in `account.css`; `.ri-set-alert-action` places it | §8 |
| §8 Handoff | WebAuthn Signal API calls; external login linking and unlinking in IdentityServer | §9 |
| §9 Q4 | Saying "already belongs to another Richie account" stays; add a per-account rate limit on those checks (default in §10 Q-S4) | §3.1 |

---

## 1. Rules every state here follows

### 1.1 Step-up order (AS §4.4, Part A §8 Q20)

| Action | Own confirmation first | Step-up (if the last sign-in or step-up was more than 10 minutes ago, or always in the 7 days after a recovery) | Then |
|---|---|---|---|
| Rename a passkey | none | **not required** | save |
| Add a passkey | none | required | WebAuthn create |
| Remove a passkey | confirm `Remove the “Pixel 8” passkey?` | required: the confirm closes, step-up opens (AS §4.0 hand-off) | remove |
| Change email | none | required, inside the change-email dialog, which keeps its height (AS §4.0 "Form dialogs … stay one dialog") | step 1 |
| **Link** Google or Microsoft | none | **required** (Q20): step-up opens from the row’s **Link** | busy "Taking you to Microsoft…", then the redirect |
| **Unlink** | confirm `Unlink your Google account?` | **required**: the confirm closes, step-up opens | unlink |

Canceling step-up cancels the action, changes nothing and returns focus to the button that started it (the row’s **Remove**, **Link** or **Unlink**). The server checks `auth_time` (or the step-up grant) on every gated endpoint; the dialog is only a convenience.

### 1.2 Busy

AS §4.0 unchanged: the button keeps its variant, shows the spinner (`aria-hidden`, no role) and a gerund, and is `aria-disabled`, never `disabled`. In a confirm or dialog, Cancel is `aria-disabled` while the action runs. Busy labels are reserved in the button with `.ra-steps` ghosts so nothing jumps: "Saving…", "Removing…", "Unlinking…", "Sending code…", "Checking code…". The row button **Link** becomes "Taking you to Microsoft…" (measured: the Microsoft row keeps its height at 1440, 1024, 768, 390 and 320 with the 204 px busy button; nothing moves).

**Announced once (B-28).** The busy text is also written once into the surface’s existing polite paragraph, sr-only when the slot shows nothing visible: the section slot for **Link** ("Taking you to Microsoft…") and **Add a passkey**; the dialog’s field slot for **Save** and **Send code**; a `p.ra-sr[role=status]` inside each confirm for "Removing…" and "Unlinking…" (the page behind a modal is inert, so its regions don’t speak). No second live region, and the spinner keeps no role.

**Countdowns (B-27).** A countdown button keeps its digits `aria-hidden` and carries a fixed sr suffix with the clock time: visible "Link in 41:12", accessible name "Link Microsoft, available at 10:42 AM"; visible "Send code in 41:12", name "Send code, available at 4:53 PM". At 0:00 the polite slot says its line once ("You can link an account now.", "You can send a code now.").

### 1.3 Failures: the three patterns (AS §4.0) and where they show

| Action | {phrase} | Where |
|---|---|---|
| Rename | rename this passkey | Inside the rename dialog, in the field’s reserved slot (`role="alert"`, focus stays on **Save**) |
| Add (server rejects the credential) | save your new passkey | Passkeys status slot |
| Remove | remove the “Pixel 8” passkey | Inside the confirm (board `Security-remove-failed`), or inside step-up when it ran |
| Change email, step 1 | send a code | The field’s reserved slot (`role="alert"`) |
| Change email, step 2 | change your email | Code meta, line 1 |
| Link, before the redirect | link Microsoft / link Google (B-6; "reach" is retired) | Google and Microsoft status slot |
| Link, after the return (provider error) | link Microsoft | Google and Microsoft status slot (polite paragraph, error look, §1.4), with the reference in a note (§4.2) |
| Unlink | unlink Google | Inside the confirm, or inside step-up when it ran |
| Load a list | (alert title "We couldn’t load your …") | A destructive `Alert` in place of the group (§2.3) |

"We couldn’t {phrase} because you’re offline." / "We couldn’t {phrase}. Check your connection." / "We couldn’t {phrase}. Try again in a moment." Every slot string fits one line at a 480 px column (measured, §12.4).

### 1.4 Status, alerts, toasts and focus

- **Inline status, not toasts**, whenever the thing acted on is still on screen (AS §4.0 toast rule): renamed, added, linked, unlinked (the row stays, now "Not linked"), email changed. **Toast** only for a removed passkey (the row leaves).
- **Slots** are the AS section slots: one grid cell with `p.ra-status[role=status]` and `p.ra-status.is-error[role=alert]`. The full copies draw both paragraphs (as `Security-passkey-failed` does); the base boards draw only the polite one.
- **Return from a provider** is a fresh page load, not a live change. The app scrolls to `#providers`, moves focus to the section `h2` "Google and Microsoft" (`tabindex="-1"`, no ring), then writes the outcome into the slot’s **polite** paragraph about 150 ms later, so a screen reader reads the heading and then the outcome once. **Errors too** go into the polite paragraph, drawn with the error look (`p.ra-status.is-error[role=status]`): an assertive alert 150 ms after a focus move cuts off the heading in NVDA and JAWS (A11Y-15). `role="alert"` stays for failures while focus is on a pressed button. The outcome comes from a one-time server flash (not a URL parameter), so a reload shows the plain page.
- **Focus after each action** is listed with each state below and collected in §6.2.

### 1.5 Why the reserved space stays empty

The rename dialog leaves 20 px under the field (40 in drawers), and the passkeys and providers foots keep their slots, because that is where "Use 40 characters or fewer.", "We couldn’t rename this passkey. Check your connection." or "Microsoft linked. You can now sign in with it." go. Collapsing them would move the footer buttons or the next section the moment a message appears (AS §1.5 rule 8).

---

## 2. Passkeys (S1–S4)

Route `/account/security#passkeys`. Document title "Sign-in and security – Richie" ("Error: Sign-in and security – Richie" after a failed rename submit). Reveals nothing to anyone else; limit 10 passkeys per account.

### 2.1 Rename (S1): `Security-rename`, `Security-rename-error-mobile`

Dialog (AS §4.0 Dialogs; a floating drawer below 768), opened by a row’s **Rename** (`aria-haspopup="dialog"`, sr "Rename the “Pixel 8” passkey": the name is quoted so a name with a comma reads as one thing, A11Y-13).

| Element | Copy and rule |
|---|---|
| Title | Rename passkey |
| Description | Pick a name that helps you recognize it, like the device you use it on. |
| Field | Label **Name**. Prefilled with the current name. `type="text"`, `autocomplete="off"`, `autocapitalize="sentences"`, `spellcheck="false"`, `enterkeyhint="done"`, no `maxlength` (a pasted long name would be cut silently) |
| Reserved slot under the field | 1 line (20) in a 400 px body, 2 lines (40) in drawers (`.ri-set-field-slot`). Holds the field error (no role), the polite paragraph (busy text, sr-only, B-28) and the action failure (`role="alert"`) |
| Footer | **Cancel** (outline), **Save** (default, `type="submit"`); busy "Saving…", Cancel `aria-disabled`. Below 768 the app renders **Save** first in the DOM (B-30). `Security-rename` keeps the desktop DOM; `Security-rename-error-mobile`, drawn only at 390, draws **Save** first in its DOM (`.ri-set-commit-first`, B-41) |
| Initial focus | The field, **with its text selected**, so typing replaces the name. The board draws the focus ring; a static board can’t draw a selection |
| Empty (after trimming) | "Enter a name for this passkey." Field `aria-invalid`, error linked by `aria-describedby`, no role, focus to the field, document title gets "Error: " |
| Over 40 characters (counted as user-perceived characters, after trimming) | "Use 40 characters or fewer." (board `Security-rename-error-mobile`, a 45-character name at 390) |
| Server failure | "We couldn’t rename this passkey. Check your connection." (offline and server variants, §1.3). Focus stays on **Save**, both buttons enabled |
| Unchanged name | Save closes the dialog without a request and without a status |
| Escape, Cancel | Close, nothing saved, focus back to the row’s **Rename** |
| Done | The dialog closes; the row title updates; focus returns to that row’s **Rename**; the passkeys slot reads "Passkey renamed." (polite, success icon); activity entry (§5.1). No security email: a name is a label, not a way in |
| Duplicate names | Allowed (a label; §10 Q-S5) |

**Rationale.** Renaming is low-stakes, so it gets no step-up and no email, and the dialog is the smallest one in the system: one field, the two buttons. The text is selected on open because people rename to replace, not to edit. The 40-character rule is said with its number. The slot is reserved because a dialog that grows when an error appears moves **Save** away from the thumb that just pressed it.

### 2.2 Add a passkey: outcomes (S2): `Security-passkey-added`, `Security-passkey-limit`

The flow and the waiting and canceled states are AS §4.3 (boards on the settings canvas). Deltas and the gaps:

| State | Button | Passkeys status slot | Focus |
|---|---|---|---|
| **Added** (board) | **Add a passkey** | "Passkey added. Rename it so you can tell your passkeys apart." (polite, success icon) | Stays on **Add a passkey** |
| Added, and that was the **10th** | Removed | "Passkey added. That’s 10, the limit." (polite; COPY-24) | Moves to the new row’s **Rename** (the button that was pressed is gone; the status invites a rename) |
| **At the limit** on load (board) | Hidden | The slot stays (empty polite and alert paragraphs, so "Passkey renamed." still has a live region to land in, A11Y-02); under it a note (`.ra-note`): "You have 10 passkeys, the limit. Remove one to add another." | — |
| Removing one at the limit | Returns | The quiet line clears; the toast and focus are §2.4’s | Passkeys `h2` |
| Already registered (`InvalidStateError`) | **Add a passkey** | (alert) "You already have a passkey on this device. Use it to sign in." | Stays on the button |
| Browser can’t create passkeys | Hidden | Quiet: "This browser can’t create passkeys. Try the latest Chrome or Safari." | — |
| Server rejects the new credential | **Add a passkey** | (alert) "We couldn’t save your new passkey. Check your connection." and variants | Stays on the button |
| Prompt canceled or timed out | **Add a passkey** | (alert) "The request timed out or you canceled it." (Part A §2.1; was "The passkey prompt closed. Try again." on `Security-passkey-failed`) | Stays on the button |

**The new row** (first in the list):

| Part | Rule | Board |
|---|---|---|
| Name (title) | The authenticator’s name from its AAGUID: a synced provider’s name ("iCloud Keychain", "Google Password Manager") or a security key’s model ("YubiKey 5 Series with NFC"); "Passkey" when unknown | "YubiKey 5 Series with NFC" |
| Line 1 (where it lives) | The provider’s name; "Security key" for roaming hardware keys; "Passkey" when unknown | "Security key" |
| Line 2 | "Added just now." (then "Added today.", "Added yesterday.", then the date: relative dates stop at yesterday, B-16) and, once used, " Last used …" | "Added just now." |

A synced passkey’s default name repeats its provider line ("iCloud Keychain" / "iCloud Keychain") until it is renamed; that is why the status asks for a rename.

**At the limit** (`Security-passkey-limit`, 10 rows): two names use all 40 characters ("Roastery office PC, under the front desk", "Spare YubiKey, kept in the roastery safe"). **Passkey names and activity titles wrap freely** (`.wrap-all`, no clamp; amends AS row 8, which clamped them at 2 lines): names are capped at 40 characters, so a row never grows past 3 title lines, and the clamp cut "Added the passkey “YubiKey 5 Series with NFC”" to two lines under WCAG 1.4.12 text spacing at 320 (RIG-03). Measured without spacing: 1 line at 1440, 2 at 390 and 320. The full name is also in the **Rename** / **Remove** names.

**Rationale.** The added state shows the result where the person is looking (the new row first, the line under the button) instead of a toast, because the row is still on screen. The limit hides the button rather than leaving it unavailable: the unavailable style means "wait", and a countdown would be wrong here because waiting doesn’t help; the quiet line says what does. At the 10th add the pressed button disappears, so focus goes to the most useful next control, the new row’s **Rename**.

### 2.3 The list didn’t load (S3): `Security-load-error`

A suiss `Alert` (destructive, `WarningCircle`) in place of the passkeys group. It appears after the page shell, when the fetch fails, so it is announced: **`role="alert"` sits on a wrapper around the title and description only** (`.ri-set-alert-text`), with **Try again** outside it, so the button isn’t read as part of the message or repeated on a second failure (A11Y-14).

| Element | Network failure (board) | Offline | Server error |
|---|---|---|---|
| Title | We couldn’t load your passkeys (COPY-13: the failure-line pattern) | same | same |
| Description | Check your connection. | You’re offline. We’ll try again when you’re back online. | Try again in a moment. |
| Action (outline `sm`, under the description) | **Try again** | **Try again** (also retried on the `online` event) | **Try again** |

- **Try again**: the button goes busy ("Trying again…", spinner, `aria-disabled`) and keeps focus. On success the alert is replaced by the rows and focus moves to the Passkeys `h2` (`tabindex="-1"`), because the button is gone. On another failure the button returns and the description is set again (the alert announces it).
- The foot keeps its reserved slot and **hides Add a passkey** until the list loads, as while loading (AS §4.3): the limit and step-up screen A both depend on the list.
- Other lists use the same pattern: "We couldn’t load your linked accounts" (Google and Microsoft), "We couldn’t load your devices" (Where you’re signed in), "We couldn’t load your security activity".
- Dark: `Security-load-error-dark` (B-21).

**Rationale.** The failure sits exactly where the list would be, so it is clear which part of the page is missing; the rest of the page still works. Red is only the icon (suiss restraint, Part A §2 row 18).

### 2.4 Remove (S4): `Security-remove-failed`

The confirm, sheet and step-up hand-off are AS §4.3 (`Security-remove-passkey`, `-mobile` on the settings canvas).

| Element | Copy |
|---|---|
| Title | Remove the “Pixel 8” passkey? |
| Description, other ways in remain | You won’t be able to sign in with it anymore. You can still use {ways}. Where {ways} lists what remains, in this order: "your other passkey" / "your other passkeys", each linked provider, "an email code"; joined with commas and a final "or" (no Oxford comma). Board: "You can still use your other passkey, Google or an email code." |
| Description, the last passkey | It’s your only passkey. After you remove it, you’ll sign in with {ways}. With Google linked: "…with Google or an email code."; with nothing linked: "…with an email code." |
| Actions | **Cancel** (outline, initial focus), **Remove passkey** (destructive); busy "Removing…" (also written once into the confirm’s sr-only polite paragraph, B-28), Cancel `aria-disabled`. Row buttons are named "Remove the “Pixel 8” passkey" (quoted, A11Y-13) |
| Failure inside the confirm (board) | A centred line under the description, `WarningCircle` inline before the first word, Alarm, `role="alert"`: "We couldn’t remove the “Pixel 8” passkey. Check your connection." (offline: "…because you’re offline."; server: "…Try again in a moment."). Both buttons enabled; focus stays on **Remove passkey** |
| Failure after a code step-up | In step-up screen B: the commit shows "Removing…" after the code is accepted; on failure, meta line 1 reads the failure line (`role="alert"`), the slots keep the code without the invalid style, and the commit becomes **Try again**, which retries the removal without a new code (step-up stays valid for 10 minutes) |
| Failure after a passkey step-up | Screen A keeps its layout: the failure line sits above the buttons (as screen A’s own failed state), and **Use a passkey** becomes **Try again**, which retries the removal |
| Success | The confirm or step-up closes; the row collapses; toast "Passkey removed" with a description by provider: synced provider "You can also delete it from {provider}." (for example "…from Google Password Manager."); security key: no description; focus to the Passkeys `h2`; activity entry; security email (§5.2) |
| Signal API | Call `PublicKeyCredential.signalAllAcceptedCredentials({ rpId, userId, allAcceptedCredentialIds })` with the **remaining** credential IDs, where supported. Never pass an empty list by mistake: it hides every passkey for Richie. (AS said `signalUnknownCredential()`; that call is for a sign-in that used a credential the server doesn’t have, and belongs on identity sign-in) |

**Rationale.** A confirm isn’t a one-height dialog (AS §4.0 lists which are), so the failure line is inserted, and it goes inside the header: on phones the header is the action sheet’s top piece, so the line sits directly above **Remove passkey** and the sheet grows upward, which means nothing under the thumb moves. On desktop the confirm grows by the line and stays centred. The description now names Google and Microsoft too, because "an email code" alone undersold what still works and could push someone to keep a passkey they meant to remove.

---

## 3. Change email (S5–S6)

Route: dialog on `/account/profile#email`. Document title "Profile – Richie" ("Error: Profile – Richie" after a failed submit). **Reveals** whether the new address has a Richie account (AS §9 Q4, accepted: signed in, stepped up, rate-limited). **Limits:** L1 for the new address (1 code per 60 s, 5 per hour); 10 address checks per account per hour (§3.1, Q-S4).

### 3.1 Step 1: the new address: `Profile-email-taken`, `Profile-email-taken-mobile`

Layout and copy as AS §4.2 (`Profile-email-change`), plus the reserved field slot (`.ri-set-field-slot` under the field; step 1 still stays inside step 2’s height at every width).

| State | What shows | Focus |
|---|---|---|
| Empty | "Enter your new email address." | Field (no role on the error; `aria-describedby`) |
| Not an address | "Enter a full email address, like name@example.com." | Field |
| Same as now | "That’s already your email. Enter a different one." | Field |
| **Belongs to another account** (board) | "That email already belongs to another Richie account. Use a different one." Field `aria-invalid`, keeps the typed value | Field |
| Sending | **Send code** reads "Sending code…" (spinner), Cancel `aria-disabled`; the text is also written once into the slot’s polite paragraph, sr-only (B-28) | Stays on the button |
| Send failed | (alert, same slot) "We couldn’t send a code because you’re offline." / "We couldn’t send a code. Check your connection." / "We couldn’t send a code. Try again in a moment." | Stays on **Send code** |
| Too many codes to that address (L1) | The commit becomes unavailable **Send code in 41:12** (`aria-disabled`, tabular; digits `aria-hidden`, name "Send code, available at 4:53 PM", B-27; the countdown starts at the server’s `Retry-After`, B-15). Its width is reserved with the other labels in the button (a ghost "Send code in 41:12"; measured, COV-26: 188 px at 1440, so Cancel doesn’t move; full width in drawers; the label fits inside its button at 1440, 390 and 320, with and without 1.4.12 text spacing). Slot (polite, once): "You’ve asked for 5 codes in an hour. Try again in 41:12." (B-34; the digits `aria-hidden` with sr "at 4:53 PM", B-27; 1 line at 400, 2 at 342 and 272, measured, §8’s field-slot rule). At 0:00: **Send code** and "You can send a code now." (Part A E9) | Stays on the button |
| Too many address checks (10 per account per hour, Q-S4) | The 11th **Send code** in an hour is refused before any lookup or send. The commit becomes unavailable **Send code in 41:12** (`aria-disabled`, tabular; digits `aria-hidden`, name "Send code, available at 4:53 PM", B-27; from the server’s `Retry-After`, B-15; the same reserved width as L1, so Cancel doesn’t move). Slot (polite, once): "You’ve tried 10 addresses in an hour. Try again in 41:12." (the L1 pattern; 1 line at 400, 2 at 342 and 272, measured). Editing the address doesn’t clear it: the limit is per account. At 0:00: **Send code** and "You can send a code now." Layout: `Profile-email-taken` (field slot and commit, as L1) | Stays on the button |

The error clears when the field changes. A long address wraps after the @ in the step 2 description (AS long-email rule).

**Rationale.** Saying the address is taken is the only answer that isn’t a dead end: a person who can’t tell why no code arrives would retry forever (AS §9 Q4). The sentence says what to do and keeps what they typed, so they can fix one character.

### 3.2 Step 2: the code: `Profile-email-code-mobile`

AS §4.2 step 2 and the AS §4.4 code rules. The code-screen grammar is Part A §4.7 (B-39); as a settings dialog it keeps the AS layout: **Use a different email** inside the body under the code meta, above the dialog footer, because the footer holds Cancel and the commit. Drawer DOM order: commit first below 768 (B-30); `Profile-email-code-mobile`, drawn only at 390, draws **Change email** first in its DOM (B-41). States:

| State | Description | Meta line 1 | Meta line 2 | Commit | Focus |
|---|---|---|---|---|---|
| Default (AS board) | Enter the code we sent to **jamie@rivera.studio**. | Codes expire after 5 minutes. | Send a new code in 1:00 | Change email | Code input |
| **New code sent** (board, 390) | same | "New code sent. Use the code in the newest email." Stays until the first character is typed, then "Codes expire after 5 minutes." | Send a new code in 1:00 (restarted) | Change email | **Code input** (the plain **Send a new code** that was pressed turns back into the countdown text); slots cleared. Polite sr status: "New code sent. Use the code in the newest email." |
| Wrong code | same | (alert) That code isn’t right. You have 3 tries left. (then 2, then "1 try left") | countdown | Change email | Input stays, text selected; document title "Error: Profile – Richie" (B-31) |
| Locked (4th wrong try) | You’ll need a new code. We’ll send it to **jamie@rivera.studio**. | That was the last try for this code. (focus target, `tabindex="-1"`, no role, no ring) | hidden | **Send a new code in 0:41** (unavailable) → **Send a new code** | The error |
| Expired | as locked | This code has expired. | hidden | **Send a new code** | The error |
| Checking | same | — | — | "Checking code…" (sr "Checking your code…") | Commit |
| Send failed | same | (alert) We couldn’t send a code. Check your connection. (variants) | plain **Send a new code** (enabled: nothing was sent) | Change email | Stays on **Send a new code** |
| Change failed (code right, server failed) | same | (alert) We couldn’t change your email. Check your connection. (variants) | countdown | Change email (enabled) | Commit. The slots keep the code, not invalid: the server keeps the code valid when the change fails (one transaction) |
| Address taken between the steps | — | — | — | — | Back to step 1 with the field filled and its error "That email already belongs to another Richie account. Use a different one." |
| **Use a different email** | Step 1 with the field filled; the code is discarded; the L1 timer for that address keeps running | | | | The field |

**Rationale.** "New code sent. Use the code in the newest email." (Part A §2.1) says which of two emails to use, and it stays until typing starts because the person reads it, switches to their mail app and comes back; a 5 s message is gone by then (now the rule on every code screen, B-20). Focus goes back to the code input because the button that was pressed turns into countdown text.

### 3.3 Done: `Profile-email-changed`

| Element | What shows |
|---|---|
| Dialog | Closes |
| Email row | jamie@rivera.studio (wraps after the @, never truncated) |
| Sidebar header and Security’s Email codes row | The new address (the board carries its own sidebar copy, as AS’s `Profile-name-saved` does) |
| Email status slot | "Email changed. We’ve let your old address know." (polite, success icon) |
| Focus | Back on **Change email** (the trigger; drawn on the board) |
| Activity | "Changed your email" (§5.1) |
| Signal API | `PublicKeyCredential.signalCurrentUserDetails({ rpId, userId, name: "jamie@rivera.studio", displayName: "Jamie Rivera" })` where supported, so password managers show the new address on Richie passkeys |
| Sessions | Other devices stay signed in (the person proved the new address and stepped up) |
| Old address afterwards | Sign in with it says it has no Richie account, as for any unknown address (nothing more revealed, Part A §1.7 principle 5) |

### 3.4 Security email to the old address (S6)

Layout: the email board `Email-recovery-alert` (WP3.4). Sent at once, to the **old** address only; it can’t be turned off (Preferences: "Account and security: Always on").

| Part | Copy |
|---|---|
| From | Richie |
| Subject | Your Richie email was changed |
| Preview text | We’ll send codes and security alerts to j•••@rivera.studio from now on. |
| Title | Your Richie email was changed |
| Body | Hi Jamie, the email on your Richie account was changed to **j•••@rivera.studio** on November 5 at 9:40 AM Pacific Time, from Chrome on macOS in Portland, Oregon. From now on we’ll send email codes and security alerts there, so this is the last email this address gets about your account. |
| Body 2 | If you made this change, there’s nothing else to do. If you didn’t, tell us now and Richie support will help you get your account back. |
| Button | **This wasn’t me** → the support form (new browser tab from the mail client) through the support handoff contract in spec-recovery (B-19): `topic=email-changed-not-me` ("Someone changed my Richie email", spec-recovery §15.1), a required reachable contact email, and a signed token naming the account and the change; works for 30 days. "This wasn’t me" stays only here (B-10) |
| Fine print | You’re getting this because it’s a security change on your Richie account. We always send these. |

The new address is masked as recovery does it (Part A §4.8: first character, three bullets, the whole domain; sr "j, hidden, at rivera.studio"), so a forwarded or shared inbox doesn’t reveal where the account went. No email goes to the new address: it just received the code, and the settings status confirms the change (§10 Q-S6).

**Rationale (departure from Part A S6, §11).** Part A gave this email a **Sign in** button. After the change the old address can’t sign in to this account any more, and the one reader who needs a button is the owner whose email was changed without their consent, who may have no other way in. **This wasn’t me** is that way in, the same promise the recovery "completed" email makes (Part A J10, §8 Q13).

---

## 4. Google and Microsoft (S7–S10)

Route `/account/security#providers`. Section copy is Part A §5.2 (WP3.1). Callback `/account/security/link/callback`. **Reveals:** whether a provider account is linked to some other Richie account, to someone who just proved they own that provider account (Part A S8, accepted). Never which account. **Limit:** L9, 5 linking attempts per account per hour.

### 4.1 The rows

| State | Title | Description | Action |
|---|---|---|---|
| Google, not linked | Google | Not linked. Works with any Google account. (reserves two lines, below) | **Link** (outline `sm`, sr "Link Google") |
| Microsoft, not linked (WP3.1) | Microsoft | Not linked. Works with a work, school or personal account. (reserves two lines) | **Link** (sr "Link Microsoft") |
| Linked (Google on `Security`) | Google | jamie.rivera@gmail.com / Linked October 2. Last used October 20. | **Unlink** (sr "Unlink Google", `aria-haspopup="dialog"`) |
| **Just linked** (Microsoft, `Security-linked`) | Microsoft | jamie.rivera@northwind.coffee / Linked just now. | **Unlink** (sr "Unlink Microsoft") |
| Linked, never used to sign in | Microsoft | {email} / Linked October 2. | **Unlink** |
| Provider gave no email | Microsoft | Linked October 2. (no email line) | **Unlink** |
| Linking (busy) | as not linked | as not linked | Spinner + "Taking you to Microsoft…", `aria-disabled` (row height unchanged, §1.2) |
| Link limit reached (L9) | as not linked | as not linked | Unavailable **Link in 41:12** (tabular, `min-width` of that label; digits `aria-hidden`, name "Link Microsoft, available at 10:42 AM", B-27). Measured (COV-26): 110 px, the label stays inside the button at 1440, 390 and 320, the row keeps its height, and with 1.4.12 text spacing it is 133 px and still fits (the row grows 2 px at 1440 under spacing only) |

**Rows keep one height across link states (RIG-05).** A row that isn’t linked reserves the two description lines a linked row has (`.ri-set-2l`, `min-height: 40px` on its description): at 1440 the single line "Not linked. Works with …" sits over 20 px of reserved space, and from 390 down it already wraps to two. Measured on the rig build: "Where you’re signed in" and "Recent security activity" start at the same y on `Security` and `Security-linked` at 1440, 390 and 320 (they moved 20 px at 1440 before). The section’s status slot was already reserved (20 / 40 / 60 by width, AS §4.0); the move came from the Microsoft row growing from one description line to two.

The email line is the provider account’s address as of its last sign-in or the link (it can change at the provider; the link is by provider subject and doesn’t, Part A §8 Q22). One account per provider: to switch Google accounts, unlink, then link the other.

### 4.2 Linking (S7, S8, S10)

1. **Link** → step-up when due (§4.4).
2. Step-up succeeds (or wasn’t needed) → the row’s **Link** goes busy, "Taking you to Microsoft…", focus stays on it.
3. The browser goes to Microsoft (or Google) with the account picker (`prompt=select_account`), so the person chooses which account to link.
4. The person returns to `/account/security#providers`; the outcome shows in the section slot (focus rule §1.4):

| Outcome | Slot (one line at 480) | Also | Board |
|---|---|---|---|
| **Linked** | (polite, success icon) Microsoft linked. You can now sign in with it. | Row linked; activity entry "Linked Microsoft"; security email | `Security-linked` |
| **That provider account is linked to another Richie account** | (polite, error look, §1.4) That Microsoft account is linked to another Richie account. | Row stays "Not linked" with **Link**. Note under the slot (`.ra-note`): "To use a different Microsoft account, select Link again. To link this one here, sign in with it to reach the other Richie account and unlink it there first." (COPY-25) | `Security-link-error` |
| Canceled at the provider (`error=access_denied`) | (polite, no icon) You canceled linking Microsoft. Nothing changed. | — | copy (layout `Security-link-error`) |
| Organization approval needed (Microsoft work account; `AADSTS65001` or `90094`) | (polite, error look) Your organization hasn’t approved Richie. | Note: "Ask your IT admin to allow Richie, or link a personal Microsoft account." (B-9, COV-18: the sign-in surfaces end "…or use your email or a passkey.") | copy |
| Linking took too long (the link request is older than 20 minutes, the sign-in request lifetime P-4; a stale or replayed `state`) | (polite, no icon) Linking Microsoft timed out. Nothing changed. | Select **Link** to start again; nothing typed is lost (B-1’s reasoning: a timeout, not a broken link) | copy |
| Provider error, timeout at the provider or an invalid response | (polite, error look) We couldn’t link Microsoft. Try again in a moment. | Note: "If it keeps happening, contact support with reference 9F2C-41AB-7D03." ("contact support" opens the support form in a new tab with `topic=provider&ref=9F2C-41AB-7D03&from=S8`, spec-recovery §15.1, B-19; Part A §4.5 reference format) | copy |
| Signed out while away (the Richie session ended) | — | The callback discards the result and sends the person to Sign in with `returnUrl=/account/security#providers`, `login_hint` and **`signin_reason=session_ended`**, so Sign in shows WP3.2’s E3 alert (COV-17). Back on Security the slot reads (polite, error look) "Microsoft wasn’t linked because you were signed out." | copy |
| Failure before the redirect | (alert, focus is on **Link**) We couldn’t link Microsoft because you’re offline. / …Check your connection. / …Try again in a moment. (B-6) | The button returns to **Link**, focus stays on it | copy |
| Limit (L9, the 6th attempt in an hour) | (polite, once) You’ve tried to link an account 5 times in an hour. | **Link in 41:12** (§4.1, B-27); at 0:00 **Link** and "You can link an account now." | copy |

The return outcomes appear at first paint of a freshly loaded page, so the two-line note under the error doesn’t shift anything the person was about to tap (the same reasoning as Part A §4.10 for alerts on loaded pages).

**What linking reveals, and its limits.** A Richie person who completes the provider’s sign-in learns that the provider account is, or isn’t, linked to a Richie account: they could learn the same by signing in with it. We never show which Richie account. A matching email never links anything (links are by provider subject, Part A §8 Q6, Q21): from settings any Google or Microsoft account can be linked, whatever its address. Five attempts per hour per account stop someone with a stolen session from probing many provider accounts.

**Rationale.** The outcome lands in the section the person left, at the same spot the base drew empty. "Linked" is the only success with an extra promise ("You can now sign in with it.") because that is why they linked. The "another Richie account" error is the one case that needs a way out, and it has two: the account picker (most often the person chose the wrong account) and the honest path for the other case (sign in with it to reach that account and unlink it there).

### 4.3 Unlinking (S9): `Security-unlink`

| Element | Copy |
|---|---|
| Title | Unlink your Google account? |
| Description | You won’t be able to sign in with Google anymore. You can still use {ways}. Board: "…your passkeys or an email code." ({ways} as §2.4: passkeys, the other linked provider, an email code. With Microsoft linked too: "You can still use your passkeys, Microsoft or an email code.") |
| Actions | **Cancel** (outline, initial focus), **Unlink** (destructive); busy "Unlinking…" (also in the confirm’s sr-only polite paragraph, B-28), Cancel `aria-disabled` |
| Then | Step-up when due (the confirm closes, step-up opens, AS §4.0 hand-off). Cancel or Escape at either step: nothing changes, focus back to **Unlink** |
| Done | The row shows "Not linked. Works with any Google account." and **Link**; slot (polite, success icon) "Google unlinked."; **focus to that row’s Link** (the pressed button is gone; as AS’s Remove photo → Upload photo); activity entry; security email. No toast: the row stays |
| Failure | Inside the confirm, as §2.4: "We couldn’t unlink Google. Check your connection." (variants); inside step-up when it ran (§2.4 rows) |
| Devices | Devices that signed in with Google stay signed in; the email suggests checking them (§10 Q-S3) |

**Rationale.** The copy says what stops working and what still works, the pattern of every AS confirm. Unlinking is gated like removing a passkey: someone with a borrowed session could otherwise remove a way the owner signs in.

### 4.4 Step-up for linking and unlinking (S10, copy)

Layouts: AS `Stepup` (screen A), `Stepup-code` (screen B), `Stepup-code-locked`.

| Screen | Link Microsoft | Unlink Google |
|---|---|---|
| A, description | To link Microsoft, use a passkey or an email code. | To unlink Google, use a passkey or an email code. |
| B, description | To link Microsoft, enter the code we sent to **jamie.rivera@northwind.coffee**. | To unlink Google, enter the code we sent to **jamie.rivera@northwind.coffee**. |
| B, locked or expired | To link Microsoft, you’ll need a new code. We’ll send it to **jamie.rivera@northwind.coffee**. | To unlink Google, you’ll need a new code. We’ll send it to **jamie.rivera@northwind.coffee**. |
| After success | The dialog closes, focus returns to **Link**, which goes busy (§4.2 step 2) | The commit reads "Unlinking…"; then §4.3 Done |

A linked provider isn’t a step-up method, but any successful sign-in, Google and Microsoft included, starts the 10-minute window (B-18, §10 Q-S1). Screen C ("Can’t get the code?") offers **Request account recovery** per B-25 (specified in spec-recovery).

---

## 5. Activity entries and security emails

### 5.1 Recent security activity (AS §4.3)

Rows as AS: title (Body), then Detail lines; neutral tile with a Phosphor glyph. Rows with an address use the AS long-email wrap. The time line is "{when}. {device} in {place}."

| Event | Glyph | Title | Line 1 | Line 2 (board sample) |
|---|---|---|---|---|
| Passkey added | `key` | Added the passkey “YubiKey 5 Series with NFC” | — | Today at 9:31 AM. Chrome on macOS in Portland, Oregon. |
| Passkey renamed | `key` | Renamed the passkey “Pixel 8” to “Work phone” | — | {when, where} |
| Passkey removed | `key` | Removed the passkey “Pixel 8” | — | {when, where} |
| Provider linked (from settings or from sign-in, P6) | `link-simple` | Linked Microsoft | jamie.rivera@northwind.coffee | Today at 9:31 AM. Chrome on macOS in Portland, Oregon. |
| Provider unlinked | `link-break` | Unlinked Google | jamie.rivera@gmail.com | {when, where} |
| Signed in with a provider | `sign-in` | Signed in with Google / Signed in with Microsoft | — | {when, where} |
| Account created with a provider (WP3.5) | `user-circle` | Created your account with Google | — | {when, where} |
| Email changed | `envelope-simple` | Changed your email | jamie@rivera.studio | {when, where} |

Titles wrap freely (`.wrap-all`, §2.2). Dates follow B-16: "Today", "Yesterday", then a date.

The link rows name the provider account because the risky case is someone linking **their own** Google account to keep a way in; the owner spots a foreign address at a glance. Failed attempts (canceled, already linked elsewhere) change nothing and aren’t logged here.

### 5.2 Security emails (always sent to the account’s current address; one button each; sender "Richie")

Rules for every one (B-7, B-8): an absolute date and time **with the account’s time zone** (else UTC), never "today"; no serial comma; one button; the same fine print as every security email in this area: "You’re getting this because it’s a security change on your Richie account. We always send these." Layout: WP3.4’s `Email-recovery-alert` drawing. The button opens `/signin?returnUrl=/account/security%23{section}`, which goes straight to Sign-in and security when the person is still signed in.

| Event | Subject | Body | Button |
|---|---|---|---|
| Passkey added | A passkey was added to your Richie account | Hi Jamie, the passkey “YubiKey 5 Series with NFC” (a security key) was added to your Richie account on November 5 at 9:31 AM Pacific Time, from Chrome on macOS in Portland, Oregon. You can now use it to sign in. / If this wasn’t you, sign in now and remove it in Sign-in and security. Then sign out of any device you don’t recognize. If you can’t sign in, contact Richie support. | Check Sign-in and security |
| Passkey removed | A passkey was removed from your Richie account | Hi Jamie, the passkey “Pixel 8” was removed from your Richie account on November 5 at 9:36 AM Pacific Time, from Chrome on macOS in Portland, Oregon. You can’t sign in with it anymore. / If this wasn’t you, sign in now and check Sign-in and security. If you can’t sign in, contact Richie support. | Check Sign-in and security |
| Provider unlinked | Google was unlinked from your Richie account | Hi Jamie, you can’t sign in to Richie with the Google account jamie.rivera@gmail.com anymore. It was unlinked on November 5 at 9:36 AM Pacific Time, from Chrome on macOS in Portland, Oregon. Devices that signed in with it stay signed in. / If this wasn’t you, sign in now and check Sign-in and security. If you can’t sign in, contact Richie support. | Check Sign-in and security |
| Email changed (to the **old** address) | §3.4 | §3.4 | This wasn’t me |

**The "provider linked" email (one template, owned here; spec-sso §8.5 points to it; COV-09, B-7).** Sent whether the link was made in settings (S8) or while signing in (P5 passkey path, P6):

| Part | Copy |
|---|---|
| To | The account email |
| Subject | Microsoft was linked to your Richie account (Google: "Google was linked to your Richie account") |
| Preview text | You can now sign in to Richie with jamie.rivera@northwind.coffee. |
| Title | Microsoft was linked to your Richie account |
| Body | Hi Jamie, you can now sign in to Richie with the Microsoft account jamie.rivera@northwind.coffee. It was linked on November 5 at 9:31 AM Pacific Time, from Chrome on macOS in Portland, Oregon. |
| Body 2 | If this wasn’t you, sign in now and unlink it in Sign-in and security. Then sign out of any device you don’t recognize. If you can’t sign in, contact Richie support. |
| Button | **Check Sign-in and security** → `/signin?returnUrl=/account/security%23providers` |
| Fine print | You’re getting this because it’s a security change on your Richie account. We always send these. |

"Contact Richie support" in an email is a link to the support form through spec-recovery’s handoff contract (B-19), `topic=signin-changed-not-me` ("Someone changed how I sign in", B-35, spec-recovery §15.1); the S6 email keeps `email-changed-not-me`. No email for a rename.

---

## 6. State matrix

### 6.1 how-we-design §4 checklist → board, copy, harness or n/a

| Checklist item | Passkeys (S1–S4) | Change email (S5–S6) | Google and Microsoft (S7–S10) |
|---|---|---|---|
| Default, realistic data | AS `Security` | AS `Profile`, `Profile-email-change` | `Security` (WP3.1), `Security-mobile`, `Security-dark` |
| Long or extreme data | `Security-passkey-limit` (two 40-character names, 10 rows); rename too long: `Security-rename-error-mobile` | Long new address: copy (AS long-email rule, wraps after the @); short local part in the sidebar: `Profile-email-changed` (§8) | Long provider email: copy (`.ra-email-wrap` row line) |
| Empty | AS `Security-no-passkeys` | n/a | Nothing linked: copy (both rows "Not linked", §4.1) |
| Initial loading | AS `Security-loading` | AS §4.0 loading table | Copy: the providers group takes the AS skeleton rows (2 rows, tile + 16 × 40 % + 12 × 60 %) |
| Load failed | `Security-load-error`, `Security-load-error-dark` | n/a (static row) | Copy: "We couldn’t load your linked accounts" (§2.3) |
| Busy | AS `Security-passkey-waiting`; "Saving…", "Removing…": copy; each announced once in a polite paragraph drawn on the boards (B-28, §1.2) | "Sending code…", "Checking code…": copy | "Taking you to Microsoft…": copy (layout `Security`, measured §1.2); "Unlinking…": copy (`Security-unlink` reserves it and draws the confirm’s polite paragraph) |
| Success | `Security-passkey-added`; "Passkey renamed.": copy (slot as on `Security-passkey-added`); removed toast: copy (AS) | `Profile-email-changed` | `Security-linked`; "Google unlinked.": copy (slot as on `Security-linked`) |
| Field validation | `Security-rename-error-mobile` (drawer; DOM order: commit first below 768 (B-30); drawn with the commit first, B-41); empty name: copy | `Profile-email-taken`, `-mobile` (drawer; DOM order: commit first below 768 (B-30); the wrapper keeps the shared desktop DOM, B-41); empty, malformed, same: copy (same slot) | n/a |
| Server and network errors | `Security-remove-failed`; rename, add: copy (§1.3) | Copy (§3.1, §3.2) | `Security-link-error`; provider error, before-redirect failure: copy (same slot + note) |
| Offline | AS `Preferences-offline` page alert; action lines: copy | same | same |
| Rate limit or cooldown | `Security-passkey-limit` (10; live slot kept, the limit line is a note) | AS `Profile-email-code` (1:00); L1 countdown and the 10-address check limit on step 1: copy (§3.1), width reserved on `Profile-email-taken`, measured (COV-26) | L9 "Link in 41:12": copy, measured in the row (COV-26); names per B-27 |
| Destructive confirmation | AS `Security-remove-passkey`, `-mobile` (action sheet; DOM order: commit first below 768 (B-30)) | n/a | `Security-unlink` (below 768 an action sheet; DOM order: commit first below 768 (B-30)) |
| Step-up | AS `Stepup*` | AS `Stepup*` (inside the dialog) | Copy (§4.4) on AS `Stepup*` |
| Device waiting, failed, canceled | AS `Security-passkey-waiting`, `-failed`; already registered, not supported: copy | n/a | Canceled at the provider: copy |
| Not allowed | Limit: `Security-passkey-limit` | Same address: copy | One account per provider: copy |
| Codes (wrong, locked, expired, new, send failed) | n/a | `Profile-email-code-mobile` (new code; drawer; DOM order: commit first below 768 (B-30); drawn with the commit first, B-41); others copy on AS `Stepup-code-error`, `Stepup-code-locked` | n/a (step-up codes as AS) |
| Sign-in request expired | n/a (signed in) | n/a | Signed out while away: copy (§4.2); session end in settings is WP3.2’s E2 |
| Enumeration and limits | 10 passkeys | Reveals the new address has an account; L1 + Q-S4 | Reveals "linked elsewhere" to the provider account’s owner; L9 |
| Focus after the action | §6.2 | §6.2 | §6.2 |
| Widths, short height, 200 % zoom | Harness (rig, 10 viewports) | Harness | Harness |
| Dark | `Security-dark`, `Security-load-error-dark` (B-21); every other state: forced-dark build (`BOARD_THEME=dark`, §12) | Forced-dark build | `Security-dark` (brand marks on black) |
| Text spacing (WCAG 1.4.12) | Checked at 320 and 1440 on every board (§12): no clipped text since passkey names and activity titles stopped clamping (RIG-03) | same | same |
| States don’t move the page (B-32) | Passkeys slot reserved; adding a row is a content change | Dialog keeps one height | Linked and not linked rows are one height (RIG-05, measured 0 px at 1440, 390, 320); the link-error note is an accepted push on a fresh page (48 / 68 / 88 px at 1440 / 390 / 320) |
| Forced colours | Rig `--forced-colors` on `Security-load-error`, `Security-remove-failed`, `Security-rename-error-mobile` | (same field-error rules) | `Security-link-error` |
| Visible focus | `Security-rename` (field), `Security-remove-failed` (**Remove passkey**), `Security-rename-error-mobile` (field) | `Profile-email-taken` (field), `Profile-email-code-mobile` (active slot), `Profile-email-changed` (**Change email**, focus returned) | `Security-unlink` (**Cancel**) |

### 6.2 Focus after each action

| Action | Focus |
|---|---|
| Rename saved / canceled | The row’s **Rename** |
| Rename failed (server) / invalid | **Save** / the field |
| Passkey added | **Add a passkey**; the 10th: the new row’s **Rename** |
| Passkey removed | Passkeys `h2` (AS) |
| Remove failed | **Remove passkey** (or step-up’s commit) |
| Load error, Try again succeeded | Passkeys `h2` |
| Change email step 1 error / send failed | The field / **Send code** |
| New code sent / locked | The code input / the error message |
| Email changed | **Change email** |
| Return from Microsoft (any outcome) | Section `h2` "Google and Microsoft", then the slot’s polite paragraph announces (errors too, A11Y-15) |
| Unlinked | That row’s **Link** |
| Step-up canceled | The button that started it |

### 6.3 Copy-only states and the board whose layout they reuse (Part A §5.3)

| Copy-only state | Final copy | Layout drawn on |
|---|---|---|
| Rename saving; "Passkey renamed." | §2.1 | `Security-rename`; slot as on `Security-passkey-added` |
| Rename empty, server failure | §2.1 | `Security-rename-error-mobile` (same slot) |
| Passkey already registered, save failed, prompt failed | §2.2 | Slot on `Security-passkey-added` (AS `Security-passkey-failed` for an alert line) |
| Not supported; the 10th added | §2.2 | Button hidden + quiet line on `Security-passkey-limit` |
| Load error offline and server; other lists | §2.3 | `Security-load-error` |
| Remove the last passkey; removing; failure after step-up | §2.4 | AS `Security-remove-passkey`; `Security-remove-failed`; AS `Stepup-code` |
| Change email empty, malformed, same address, send failed, L1 limit, address-check limit | §3.1 | `Profile-email-taken` |
| Change email wrong code, locked, expired, send failed, change failed | §3.2 | AS `Stepup-code-error`, `Stepup-code-locked`; `Profile-email-code-mobile` |
| Security email to the old address | §3.4 | `Email-recovery-alert` (WP3.4) |
| Link busy; failure before the redirect; L9 limit | §4.1, §4.2 | `Security` (row action), slot on `Security-link-error` |
| Step-up before linking or unlinking | §4.4 | AS `Stepup`, `Stepup-code` |
| Canceled at the provider; organization hasn’t approved; timed out; provider error; signed out while away | §4.2 | Slot and note on `Security-link-error` |
| Unlinking; unlinked; unlink failed | §4.3 | `Security-unlink`; slot on `Security-linked`; `Security-remove-failed` |
| Activity entries, security emails | §5 | `Security-linked`, `Security-passkey-added`; `Email-recovery-alert` |

---

## 7. Boards

All in `.design/edges/project/`, linking `./account.css` then `./edges.css`, `<script src="./support.js"></script>` kept, `<title>` = board title, `$preview` = board size. Full copies are generated from the bases by `.design/edges/wp33/make-copies.py` and overlays by `make-overlays.py` (re-run after a base change; every edit asserts its anchor, so a changed base fails loudly instead of half-patching).

| File | Board title | Size | Kind | Imports | State |
|---|---|---|---|---|---|
| `Security-mobile` | Sign-in and security, mobile | 390×844 | W | Security | `Security` at 390; row actions wrap under the text |
| `Security-dark` | Sign-in and security, dark | 1440×1900 | W (`.dark`, `min-height:1900px`, helmet `body{margin:0;background:#000}`) | Security | Brand marks on black tiles |
| `Security-linked` | Sign-in and security, Microsoft linked | **1440×2000** (measured 1972; CANVAS-CHANGES WP3.3-1) | F (Security) | Account-sidebar | Microsoft row linked, slot "Microsoft linked…", activity "Linked Microsoft" |
| `Security-link-error` | Sign-in and security, Microsoft linked to another account | **1440×2000** (measured 1924 after the RIG-05 row reservation; WP3.3-2) | F | Account-sidebar | Error in the polite paragraph + note |
| `Security-unlink` | Sign-in and security, unlinking Google | 1440×900 | O | Security | Unlink confirm, focus on Cancel |
| `Security-passkey-added` | Sign-in and security, passkey added | **1440×2100** (measured 2048; WP3.3-3) | F | Account-sidebar | New YubiKey row first, status, activity entry |
| `Security-passkey-limit` | Sign-in and security, 10 passkeys, the limit | 1440×2700 (measured 2628) | F | Account-sidebar | 10 rows, button hidden, live slot kept, limit note |
| `Security-load-error` | Sign-in and security, passkeys didn’t load | 1440×1900 (measured 1778; kept, WP3.3-5) | F | Account-sidebar | Destructive alert + Try again; Add a passkey hidden |
| `Security-load-error-dark` (new, B-21) | Sign-in and security, passkeys didn’t load, dark | 1440×1900 | W (`.dark`, `min-height:1900px`, helmet `body{margin:0;background:#000}`) | Security-load-error | The load error on black |
| `Security-rename` | Sign-in and security, renaming a passkey | 1440×900 | O | Security | Rename dialog, field focused, slot reserved |
| `Security-rename-error-mobile` | Sign-in and security, passkey name too long, mobile | 390×844 | O at 390 | Security | Drawer, "Use 40 characters or fewer." DOM order: commit first below 768 (B-30): drawn with **Save** first in the footer’s DOM (`.ri-set-commit-first`, B-41) |
| `Security-remove-failed` | Sign-in and security, passkey not removed | 1440×900 | O | Security | Failure line inside the confirm, focus on Remove passkey |
| `Profile-email-taken` | Profile, new email already has an account | 1440×900 | O | Profile | Step 1 error, field focused |
| `Profile-email-taken-mobile` | Profile, new email already has an account, mobile | 390×844 | W | Profile-email-taken | Step 1 drawer. DOM order: commit first below 768 (B-30); the wrapper keeps the shared desktop DOM, its visual order is normative (B-41) |
| `Profile-email-code-mobile` | Profile, new code sent, mobile | 390×844 | O at 390 | Profile | Step 2 drawer, "New code sent…", focus on the code. DOM order: commit first below 768 (B-30): drawn with **Change email** first in the footer’s DOM (`.ri-set-commit-first`, B-41) |
| `Profile-email-changed` | Profile, email changed | 1440×1000 (measured 920) | F (Profile, **own sidebar copy**) | none | New address in the row and the sidebar, status, focus back on Change email |

**Mobile overlays.** The two "O at 390" boards have no desktop counterpart, so they are overlays drawn directly at 390 × 844 (inert import at `hint-size="390px,844px"` plus the layer). They carry no 390 px wrapper `div`: the layer and the inert page are fixed to the board frame, so the frame sets the width, and the rig’s fluid build can test them at 320 and 375 (with a wrapper `div` the build kept the 390 width and reported a false overflow).

**Interactive.** Full copies and wrappers keep the base’s links (Profile ↔ Security in the sidebar, **Change email** → `Profile.dc.html#email`). No board links to another WP3.3 board: the states are outcomes of server round trips.

---

## 8. CSS (`edges.css`, WP3.3 section)

Layout only. No tokens, no colours, no overrides of a shared recipe value. New colour pairs: none.

| Selector | Rule | suiss mapping | Why |
|---|---|---|---|
| `[data-slot=alert] > .ri-set-alert-action` | `grid-column: 2; margin-top: 12px` | `AlertAction` moved below the description (AS §8 row 17) | AS row 17 names `.ra-alert-action`, but `account.css` doesn’t ship it; without it the button would fall into the icon column |
| `.ri-set-field-slot` | `min-height: 20px`; `40px` under `@container ra-code (max-width: 399px)` | Own `StatusSlot reserve="fixed"` inside `Field` | The rename and change-email field slot; longest string 1 line at 400, 2 at 342 and 272 (§12.4) |
| `[data-slot=alert-dialog-header] > .ri-set-confirm-error` (+ `> svg`) | `display: block; margin-top: 6px; text-align: center; text-wrap: balance`; icon `inline-block`, `vertical-align: -3px`, 6 px after | `AlertDialogHeader` + own `StatusLine` | The failure inside a confirm; icon inline with the first word, as `account.css` does for the code meta error (as a flex item the icon sat at the left edge, apart from the wrapped text: seen in the first render) |
| `[data-slot=alert] > .ri-set-alert-text` | `grid-column: 2; display: flex; flex-direction: column; gap: 2px; min-width: 0` | `AlertTitle` + `AlertDescription` inside a `div role="alert"` | A11Y-14: the alert role covers the message, not **Try again** |
| `[data-slot=item-description].ri-set-2l` | `min-height: 40px` | `ItemDescription className="min-h-10"` in a `ProviderRow` | RIG-05: a provider row that isn’t linked is as tall as a linked one |
| `[data-slot=dialog-footer].ri-set-commit-first` (revision round) | below 768 `flex-direction: column`; from 768 `row-reverse` with `justify-content: flex-start` | `DialogFooter` rendering the commit first below 768 (B-30) | B-41: the two boards drawn only at 390 put the commit first in their DOM, so Tab order matches the drawer’s visual order; a wide render still reads Cancel, then the commit |
| (merged) `.ra-email-local.ri-email-short` | in `edges.css` §6 since B-3 | Own `EmailText`, `short` prop | `account.css` §15’s `min-width: 2.6em` beat `max-width: max-content` ("jamie @rivera.studio"); the scoped `.ri-set-email-short` was removed when the PM merged it |

---

## 9. Handoff mapping

### 9.1 `@repo/ui`

| Piece | Build with | Notes |
|---|---|---|
| Rename dialog | `modal` (Dialog ≥768, Drawer below), `field` (`Field`, `FieldLabel`, `Input`, `FieldError`), own `StatusSlot reserve="fixed"` | Select the input’s text on open (`onOpenAutoFocus` → `input.select()`). Count with `Intl.Segmenter` (graphemes), after `trim()`. `FieldError` without `role` (AS §8 row 12) |
| Confirms (remove, unlink) | `confirm` pattern (`AlertDialog` / `AlertSheet`), async action as a plain `Button` in `ConfirmFooter` (suiss gotcha 4) | Failure: own `StatusLine` (error) inside `AlertDialogHeader`, `role="alert"` |
| Load error | `alert` (`variant="destructive"`), `AlertTitle`, `AlertDescription`, `AlertAction` below the description, `Button variant="outline" size="sm"` | Busy "Trying again…" |
| Provider rows | `item` (`ItemMedia` brand tile: Paper with a hairline, `<img>` of Google’s official G, Part A P-3; Microsoft’s 21 px mark), `Button variant="outline" size="sm"` | Busy: `Spinner data-icon="inline-start"` with `aria-hidden`, `role` removed |
| Change email | AS §8 rows 18, 20 | `useCodeTimer` from the server’s next-allowed time; the "New code sent" line stays until `onChange` gets its first character |
| Email in the sidebar | Own `EmailText` with `short` when the local part has ≤ 5 characters | §8 |
| Section slots | Own `StatusSlot reserve="section"` | Return outcomes written after focusing the `h2` |

### 9.2 IdentityServer and ASP.NET Core: linking and unlinking

**Which host owns what (COV-08; spec-sso §13.2 references this paragraph).** The Google and Microsoft schemes are registered **once, in the identity host** (the Duende IdentityServer application that serves `/signin`, `/signup` and the `/account/*` settings pages and owns the user store). Nothing else holds provider client secrets or sees provider tokens. Settings linking is an identity-host route too: the settings page’s **Link** is a top-level form `POST` to `/account/security/link/{scheme}` on the identity host (never `fetch`: a provider redirect can’t run inside one). If the settings UI is ever served from another origin, it keeps that top-level navigation to the identity host and doesn’t proxy the challenge. Each provider has **one redirect URI per environment**, the handler’s `CallbackPath`, shared by sign-in and linking: Google `https://{identity host}/signin-google`, Microsoft `https://{identity host}/signin-microsoft` (set `CallbackPath` on each `AddOpenIdConnect`, since two OIDC schemes can’t share `/signin-oidc`), registered for production, staging and local development (`https://localhost:{port}/signin-google`, `…/signin-microsoft`). After the handler validates the response it redirects to the app-level callback named in `AuthenticationProperties.RedirectUri`: `/signin/external/callback` for sign-in and sign-up (WP3.5), `/account/security/link/callback` for linking. Neither is registered at the provider. **Step-up freshness** is read from the identity host’s own session: its cookie carries `auth_time` (`JwtClaimTypes.AuthenticationTime`), set by every successful sign-in, email code, passkey, Google or Microsoft (B-18), and re-issued by step-up (`HttpContext.SignInAsync` with a new `IdentityServerUser { AuthenticationTime = now }` and the step-up method in `amr`). The link and unlink endpoints require `now − auth_time ≤ 600 s`; in the 7 days after a recovery they require a one-use step-up grant bound to the action (`link:microsoft`, `unlink:google`) instead. A settings client that uses tokens reads `auth_time` from its id_token only to decide whether to show step-up first; the identity host always re-checks.

| Step | Implementation |
|---|---|
| Schemes | **One registration, the same text in spec-settings §9.2 and spec-sso §13.2** (one handler for both providers, registered once in the identity host; COV-08). Google: `AddOpenIdConnect("google", "Google", …)` with `Authority = "https://accounts.google.com"`, `ResponseType = "code"`, scopes `openid email profile`, `CallbackPath = "/signin-google"`. Microsoft: `AddOpenIdConnect("microsoft", "Microsoft", …)` with `Authority = "https://login.microsoftonline.com/common/v2.0"` (any tenant, Part A §8 Q23) and the issuer validated per tenant (`https://login.microsoftonline.com/{tid}/v2.0`), `ResponseType = "code"`, scopes `openid email profile User.Read`, the optional claims `email`, `xms_edov`, `given_name` and `family_name` set in the app registration’s token configuration, `CallbackPath = "/signin-microsoft"` (two OIDC schemes can’t share `/signin-oidc`). Both: `SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme`, `SaveTokens = false` (Richie keeps no provider tokens, so unlinking has nothing to revoke at the provider), `MapInboundClaims = false` and `GetClaimsFromUserInfoEndpoint = false`, so the callback reads the id_token’s own claim names. **Claim mapping:** link key Google `sub`, Microsoft `tid` + `oid`; email `email`, confirmed by Google’s `email_verified` or Microsoft’s `xms_edov` (the consumer tenant `9188040d-6c67-4c5b-b112-36a304b66dad` counts as confirmed); names `given_name`, `family_name`, `name`; photo Google’s `picture` (Microsoft’s from Graph at sign-up, spec-sso §8.1). `preferred_username` and `upn` are never used as an email. Not `AddGoogle` (OAuth 2.0 plus userinfo: no id_token, and the confirmed flag arrives as `verified_email`) and not `AddMicrosoftAccount` (no id_token, so no `tid`, `oid` or `xms_edov`). |
| Start | `POST /account/security/link/{scheme}` (antiforgery). Server checks: signed in; step-up (above); L9; no existing link for this provider on this account. Then `Challenge(scheme)` with `AuthenticationProperties { RedirectUri = "/account/security/link/callback", Items = { scheme, purpose = "link", sub = <Richie user id>, nonce, started = <UTC> } }` and `prompt=select_account` |
| Callback | `/account/security/link/callback`: `AuthenticateAsync(ExternalCookie)`. Discard if: no Richie session, or `Items.sub` ≠ the session’s `sub` (→ redirect to `/signin?returnUrl=/account/security%23providers&login_hint=…&signin_reason=session_ended`, then "Microsoft wasn’t linked because you were signed out.", COV-17), or older than 20 minutes (→ "Linking Microsoft timed out. Nothing changed."). Provider key: Google `sub`; Microsoft `tid` + `oid` (Part A §3.2). `FindByLoginAsync(provider, key)`: another user → link-error; none → `AddLoginAsync(user, new UserLoginInfo(provider, key, displayName))` plus the display email and `linkedAt` (or the Duende User Management equivalent). Then `SignOutAsync(ExternalCookie)`, write the activity entry, send the email, set the one-time flash, redirect to `/account/security#providers` |
| Provider errors | `error=access_denied` → canceled; `AADSTS65001` / `90094` in `error_description` → organization hasn’t approved; a correlation failure or an expired link request → timed out; anything else or a remote failure → provider error with the reference (first 12 hex of the trace ID, Part A §4.5); log the provider’s `error_description`, never show it |
| Mobile footers | Below 768 the app renders the commit first in the DOM in drawers and action sheets (B-30); the boards share one DOM across widths |
| Unlink | `DELETE /account/security/link/{scheme}` (antiforgery), step-up check, `RemoveLoginAsync`. Richie sessions that signed in with that provider stay signed in |
| Sign-in with a linked provider | WP3.5’s `/signin/external/callback` uses the same `FindByLoginAsync` lookup; "Last used" on the row updates on each such sign-in, and so does the displayed provider email |
| Email change | One transaction: verify the code and update the email; on failure keep the code valid. Then `signalCurrentUserDetails` on the client (§3.3) and the old-address email with a signed "This wasn’t me" token (30 days) |
| Passkey removal | Server deletes the credential; the client calls `signalAllAcceptedCredentials` with the remaining IDs (§2.4) |

---

## 10. Open questions, with defaults

Design proceeds on the default.

| # | Question | Default | Confirm |
|---|---|---|---|
| Q-S1 | Should a linked Google or Microsoft account count as a step-up method ("Use Google")? | **Decided (B-18):** no **step-up** method; step-up stays a passkey or an email code. But any successful **sign-in**, Google and Microsoft included, starts the 10-minute window, so a person who just signed in with Google can change their email at once (R1’s route, spec-recovery) | PM (decided) |
| Q-S2 | In the "linked to another Richie account" error, show that account’s masked address? | **No.** The person can reach it by signing in with the provider; naming it adds little and leaks it on a shared device | Security |
| Q-S3 | Should unlinking sign out the devices that last signed in with that provider? | **No.** The email asks to check devices; Where you’re signed in has the tools | Security, product |
| Q-S4 | Rate limit on change-email existence checks (step 1 says "already belongs to another Richie account") | **10 checks per account per hour**; the state is specified in §3.1 ("Too many address checks") | Security, engineering |
| Q-S5 | Duplicate passkey names | **Allowed** | Product |
| Q-S6 | Email the new address after a change? | **No** (it received the code; the status confirms) | Product |
| Q-S7 | Default passkey name source | **The AAGUID name** (synced provider or security key model), else "Passkey" | Engineering |
| Q-S8 | Sign-in with an address that was just changed away from | **Treat as unknown** (reveal nothing) | Security |
| Q-S9 | Microsoft admin-approval wording | **Decided (B-9):** "Your organization hasn’t approved Richie." / "Ask your IT admin to allow Richie, or link a personal Microsoft account." | PM (decided) |
| Q-S10 | Log renames in security activity? | **Yes, logged; no email** | Product |

---

## 11. Departures, and the questions Part A asked me to resolve

**Departures from Part A (binding), each with its reason:**

1. **S6 email button.** Part A: **Sign in** (+ "If this wasn’t you" → Help). Here: **This wasn’t me** → support with a token. The old address can no longer sign in to the account, and the reader who needs a button is the owner whose email was changed without consent (§3.4).
2. **Board sizes.** `Security-linked` and `Security-link-error` 1440 × 2000, `Security-passkey-added` 1440 × 2100 (Part A §5.2: 1900), measured and filed in CANVAS-CHANGES (WP3.3-1 to -4, including the +100 shift of the rows below).
3. **Focus after unlink** goes to the row’s **Link**, not the section `h2` that WP3.1 gave `tabindex="-1"` "for focus after unlink". The row stays, so focus stays in it (AS Remove photo → Upload photo). The `h2` keeps `tabindex="-1"` for the return from a provider (§1.4).
4. **Mobile overlay boards** have no 390 px wrapper `div` (§7), so the rig can test them below 390. Board size, imports and rendering on the canvas are as Part A §5.1.
5. **"New code sent…" persistence** (AS §4.4 said 5 s) is an AS delta, made because of Part A §2.1’s longer string (§3.2).

**Resolved (Part A §7.2 "Resolve"):**

- **Unlink confirm copy:** "Unlink your Google account?" / "You won’t be able to sign in with Google anymore. You can still use your passkeys or an email code." (§4.3).
- **Unlink needs step-up:** yes, after the confirm (§1.1, §4.4).
- **What "already linked to another Richie account" tells the person:** select **Link** again to pick a different account, or sign in with that account to reach the other Richie account and unlink it there (§4.2).
- **Passkey rows at the limit with long names:** names wrap freely (`.wrap-all`, no clamp): 40 characters fit 1 line at 1440 and 2 at 390 and 320; nothing is cut (§2.2).

**Base boards:** `Profile` and `Account-sidebar` are unchanged. `Security` was unchanged in Revision 1 (P-6) and **changed in the Phase 4 fix round** with the PM’s permission (RIG-05, plus RIG-03 and A11Y-13 on the same rows; §14). WP3.4 re-copies it for `Security-recovered`.

---

## 12. Test results

### 12.0 Phase 4 fix round (Revision 2, current)

After the fixes in §14, on `Security` (changed) and all 16 WP3.3 boards (`.design/edges/qa/wp33-p4/`):

| Run | Blockers | Majors | Minors | Minors discarded as |
|---|---|---|---|---|
| Light build, 10 viewports × light and dark scheme (`light/`) | 0 | 0 | 264 | 234 `target-size/<44px` (36 px sidebar items and 32 px row buttons on fine pointers ≥768; buttons of the inert page behind the six overlays) and 30 axe "needs review" contrast over the scrim or Mist, as §12.1 |
| `BOARD_THEME=dark` build, `--schemes dark` (`dark/`) | 0 | 0 | 264 | the same |
| WCAG 1.4.12 text spacing at 320 and 1440 (`.design/edges/wp33/ts.mjs`, the PM’s `ts.mjs` pointed at `out-wp33`) | — | — | — | No clipped text, no button overflow, no horizontal scroll on any WP3.3 board. The one line it prints is "Skip to content" on the shell boards: the visually hidden skip link (1 × 1 until focused), a false positive of the script |

Layout (RIG-05, `.design/edges/wp33/section-probe.mjs`): "Where you’re signed in" starts at 1060 / 1372 / 1580 (1440 / 390 / 320) on both `Security` and `Security-linked`: 0 px. `Security-link-error` pushes it 48 / 68 / 88 px with its note (accepted, §4.2). Board heights at 1440: `Security` 1876, `Security-linked` 1972, `Security-link-error` 1924, `Security-passkey-added` 2048, `Security-passkey-limit` 2628, `Security-load-error` 1778: all inside their board sizes. Countdown labels (COV-26, `countdown-probe.mjs`): §3.1 and §4.1. New slot strings (`measure-p4.json`): all 1 line at 458, at most 2 at 328 and 266. Drift: only the intended own-sidebar copy. Icons: 0 bad.

### 12.1 Rig (Revision 1)

`node .design/qa-rig/build.mjs .design/edges/project .design/edges/out-wp33 .design/edges/blobs` (0 warnings); `lsof -i :8813` empty before every run; `node .design/qa-rig/run.mjs --out .design/edges/out-wp33 --port 8813 --boards <15> --results .design/edges/qa/wp33`. 15 boards × 10 viewports × light and dark:

**0 blockers, 0 majors, 245 minors** (`.design/edges/qa/wp33/report.md`).

Fixed along the way: the two 390 overlays overflowed at 320 and 375 in the fluid build (2 blockers each) because their wrapper `div` kept 390 px: dropped the wrapper (§7). The confirm failure line’s icon sat apart from its text: inline icon (§8). The sidebar’s "jamie @rivera.studio" gap: `ri-set-email-short` (§8).

**Minors discarded as known false positives** (how-we-design §5, rig README):

| Count | What | Why it is noise |
|---|---|---|
| 218 | `target-size/<44px` | 36 px sidebar items and 32 px row buttons on fine pointers ≥768 (by design, 44 below 768 and on coarse pointers); buttons of the **inert** page behind the six overlays |
| 27 | `axe-review/color-contrast` "needs review" | Text over the dialog scrim or Mist (alpha surfaces); the computed pairs are AS §7.1’s |

Keyboard orders checked: rename = Name, Cancel, Save; confirms = Cancel, then the action; change email = the field or code, (Use a different email), Cancel, the commit. Ghost busy labels appear in the listed names (known noise).

### 12.2 Forced colours

`--forced-colors` on `Security-link-error`, `Security-load-error`, `Security-remove-failed`, `Security-rename-error-mobile` (`.design/edges/qa/wp33/forced/`): 0 blockers, **51 majors, all `axe/color-contrast`, discarded**: axe reads the authored colours under forced-colours emulation (for example `#1d1d1f` on a forced black Canvas, 1.24:1). The unchanged base boards give the same findings (`Security` 20, `Main` 12, same selectors and ratios) in a control run. Checked by eye: the alert, confirm, drawer and groups keep their outlines; the field error and confirm line are readable; buttons keep their borders; brand marks keep their colours.

### 12.3 Looked at myself

1440, 390 and 320 light for every board; `Security-dark` at 1440; a forced-dark build (`class="dark"` on every `body`, `.design/edges/out-wp33-dark`) at 390 for the six overlay and error boards. Nothing overflows; the 10-passkey list wraps its long names cleanly at 320; brand tiles read on black; the drawers keep their footers visible.

### 12.4 Status strings (`node .design/wp3-1/measure-status.mjs`)

Lines at 458 / 328 / 266 (AS section slots) and 400 / 342 / 272 (dialog bodies), Inter and SF; data in `.design/edges/wp33/measure-settings.json`. Every section-slot string is **1 line at 458** and at most 2 at 328 and 266 (3 are reserved below 350). Every rename and change-email slot string is 1 line at 400 except "That email already belongs to another Richie account. Use a different one." (2 lines at every width; it sits in step 1, which is shorter than step 2, so nothing moves). Examples:

| String | 458 | 328 | 266 | 400 | 342 | 272 |
|---|---|---|---|---|---|---|
| Microsoft linked. You can now sign in with it. | 1 | 1 | 2 | 1 | 1 | 2 |
| That Microsoft account is linked to another Richie account. | 1 | 2 | 2 | 1–2 | 2 | 2 |
| You’ve tried to link an account 5 times in an hour. | 1 | 1–2 | 2 | 1 | 1 | 2 |
| Passkey added. Rename it so you can tell your passkeys apart. | 1 | 2 | 2 | 2 | 2 | 2 |
| We couldn’t rename this passkey because you’re offline. | 1 | 2 | 2 | 1 | 2 | 2 |
| New code sent. Use the code in the newest email. | 1 | 2 | 2 | 1 | 1–2 | 2 |
| We couldn’t remove the “Pixel 8” passkey. Check your connection. (confirm, no reservation) | 1 | 2 | 2 | 2 | 2 | 2 |

### 12.5 Drift and icons

`node .design/qa-rig/drift.mjs .design/edges/project`: one drift, intended: `Profile-email-changed`’s own sidebar copy differs from `Account-sidebar` only in the address (as AS’s `Profile-name-saved`). `python3 -I .design/canvas-tools/check-icons.py .design/edges/icons <boards>`: **0 bad** (`key`, `link-simple`, `warning-circle`, `check-circle` and the base icons).

---

## 13. For the other WPs and the PM

- **WP3.4:** **`Security` changed in the Phase 4 fix round; re-copy it for `Security-recovered`.** The changes: the Microsoft description has `class="ri-set-2l"`; passkey and activity titles use `wrap-all` instead of `wrap-2`; the passkey buttons’ `aria-label`s quote the name ("Rename the “Pixel 8” passkey"). The page is 20 px taller at 1440 (1876). The S6 email uses your `Email-recovery-alert` layout, your masking rule and the support handoff contract. During the 7 recovered days, step-up runs for link and unlink too.
- **WP3.5:** settings linking shares your callback’s `FindByLoginAsync` lookup; the row’s "Last used" updates on your provider sign-ins. The provider error uses Part A §4.5’s reference.
- **WP3.2:** a Richie session that ends while the person is at Microsoft lands on your Sign in (E3) with `returnUrl=/account/security#providers`.
- **PM:** CANVAS-CHANGES WP3.3-1 to -6 (three taller boards, the row shift, the new `Security-load-error-dark`); CSS-REQUESTS WP3.3-1 (merged as B-3). Board files: `.design/edges/project/Security-{mobile,dark,linked,link-error,unlink,passkey-added,passkey-limit,load-error,load-error-dark,rename,rename-error-mobile,remove-failed}.dc.html`, `Profile-email-{taken,taken-mobile,code-mobile,changed}.dc.html`, and the changed base `Security.dc.html` (before: `.design/edges/wp33/Security.before-p4.dc.html`). Generators: `.design/edges/wp33/make-copies.py`, `make-overlays.py`.
- **WP3.5:** spec-sso §8.5 and §13.2 can point at §5.2 (the one "provider linked" email) and §9.2 (hosts, redirect URIs, `auth_time`).

---

## 14. Fix log (Phase 4)

Findings from `qa/p4-rig.md`, `qa/p4-a11y.md`, `qa/p4-copy.md`, `qa/p4-coverage.md` owned by WP3.3, and the Part B rulings that land on these pages. All fixed; none left open.

| ID | Severity | Fix | Where |
|---|---|---|---|
| RIG-03 | major | **Fixed.** Passkey names and activity titles wrap freely (`.wrap-all`) instead of the 2-line clamp, in the `Security` base and both generators; text-spacing check at 320 and 1440 is clean | `Security`, every full copy; §2.2, §5.1 |
| RIG-05 | minor | **Fixed, with a different cause than reported.** The section’s status slot was already reserved (20 px); the 20 px move came from the Microsoft row growing from one description line to two when linked. The row that isn’t linked now reserves two lines (`.ri-set-2l`), so linking moves nothing at 1440, 390 or 320. The link-error note stays an accepted push | `Security` base (allowed for this finding); §4.1, §12.0 |
| A11Y-02 | major | **Fixed.** `Security-passkey-limit` keeps the passkeys slot (polite + alert paragraphs); the limit line is a `.ra-note` after it; "Passkey renamed." lands in the polite paragraph above the note | `Security-passkey-limit`; §2.2 |
| A11Y-13 | minor | **Fixed.** Names in the row buttons’ accessible names are quoted: "Rename the “Spare YubiKey, kept in the roastery safe” passkey" | `Security` base, generators; §2.1, §2.4 |
| A11Y-14 | minor | **Fixed.** `role="alert"` moved to `.ri-set-alert-text` around the title and description; **Try again** sits outside it | `Security-load-error`; §2.3, §8 |
| A11Y-15 | minor | **Fixed.** Return-from-provider outcomes, errors included, are written into the polite paragraph with the error look; `role="alert"` only while focus stays on a pressed button | `Security-link-error`; §1.4, §4.2 |
| COPY-02 | major | **Fixed.** No serial commas in the security emails: "If this wasn’t you, sign in now and remove it in Sign-in and security. Then sign out of any device you don’t recognize." | §5.2 |
| COPY-13 | minor | **Fixed.** "We couldn’t load your passkeys" (and the other lists) | `Security-load-error`; §2.3 |
| COPY-24 | polish | **Fixed.** "Passkey added. That’s 10, the limit." | §2.2 |
| COPY-25 | polish | **Fixed.** Note: "…sign in with it to reach the other Richie account and unlink it there first." | `Security-link-error`; §4.2 |
| COV-08 | major | **Fixed.** One paragraph: schemes in the identity host, settings Link as a top-level POST to it, one redirect URI per provider per environment (`/signin-google`, `/signin-microsoft`), two app-level callbacks, `auth_time` from the identity session | §9.2 |
| COV-09 | major | **Fixed.** One "provider linked" email for settings and sign-in: absolute date with the zone, button "Check Sign-in and security", both pieces of advice, the global fine print | §5.2 |
| COV-17 | minor | **Fixed.** The link callback’s redirect to Sign in adds `signin_reason=session_ended`, so E3 shows | §4.2, §9.2 |
| COV-18 | minor | **Fixed** per B-9: "Your organization hasn’t approved Richie." / "Ask your IT admin to allow Richie, or link a personal Microsoft account." | §4.2, §10 Q-S9 |
| COV-26 | minor | **Fixed.** Measured: "Link in 41:12" 110 px (133 with text spacing) fits its button and keeps the row height at 1440, 390 and 320; "Send code in 41:12" reserved in the change-email commit (188 px at 1440, Cancel no longer moves, it moved 3 px before; full width in drawers) | `Profile-email-taken`, `Profile-email-code-mobile`; §3.1, §4.1 |

| Ruling | Applied |
|---|---|
| B-6 | "We couldn’t link Microsoft…" / "We couldn’t unlink Google…"; "reach" retired (§1.3, §4.2) |
| B-7, B-8 | §5.2: one linked email; every email time names the zone ("9:31 AM Pacific Time"), §3.4 too |
| B-9 | §4.2 organization approval |
| B-10, B-19 | "This wasn’t me" only on S6; support links through spec-recovery’s contract (§3.4, §5.2) |
| B-15, B-27 | Countdown names with a fixed clock time and `aria-hidden` digits; rate-limit countdowns from `Retry-After` (§1.2, §3.1, §4.1) |
| B-16 | Relative dates stop at yesterday (§2.2, §5.1) |
| B-18 | Q-S1 decided; §4.4 |
| B-20, B-31 | "New code sent…" until typing; "Error: " prefix and selected text after a failed code (§3.2) |
| B-21, B-33 | New board `Security-load-error-dark`, 1440 × 1900 (§7; CANVAS-CHANGES WP3.3-6) |
| B-25 | Step-up screen C’s recovery route referenced (§4.4) |
| B-28 | A polite paragraph drawn in each confirm and dialog slot for the busy text (§1.2; `Security-unlink`, `Security-remove-failed`, `Security-rename*`, `Profile-email-taken`) |
| B-30 | Handoff: commit first in the DOM below 768 (§2.1, §9.2) |
| B-1 (reasoning) | A link request older than 20 minutes says "Linking Microsoft timed out. Nothing changed." instead of a generic error (§4.2) |

---

## Revision round (Phase 5)

After the grade (`evaluation.md`) and rulings B-34 to B-43. Every change is written into the sections above.

1. No change in this file.
2. No change in this file.
3. No change in this file.
4. **B-34:** §3.1’s L1 slot is "You’ve asked for 5 codes in an hour. Try again in 41:12." (1 line at 400, 2 at 342 and 272: the field-slot rule holds).
5. **B-35:** the security emails’ "Contact Richie support" uses `topic=signin-changed-not-me` (§5.2); S6’s **This wasn’t me** uses `email-changed-not-me` (§3.4).
6. No board here has an arrival alert (`Security-recovered` is spec-recovery’s).
7. **B-39:** §3.2 points at Part A §4.7’s code-screen grammar; the change-email dialog keeps the AS layout (alternatives above the footer).
8. No change in this file.
9. No change in this file.
10. No change in this file.
11. No change in this file.
12. **Stale text:** §11’s passkey-name sentence now says names wrap freely (`.wrap-all`); §3.1 gains the address-check limit row (10 per account per hour: **Send code in 41:12**, "You’ve tried 10 addresses in an hour. Try again in 41:12.", "You can send a code now.", layout `Profile-email-taken`) and Q-S4 points to it; §4.2’s provider-error link is `topic=provider&ref=9F2C-41AB-7D03&from=S8`; §9.2’s scheme registration is the same text as spec-sso §13.2 (`AddOpenIdConnect` for both, the claim mapping, each `CallbackPath`).
13. No change in this file.
14. **B-41:** `Security-rename-error-mobile` and `Profile-email-code-mobile` draw the commit first in the footer’s DOM (`.ri-set-commit-first`, edges.css WP3.3; generated by `make-overlays.py`); "DOM order: commit first below 768 (B-30)" is in §2.1, §3.2, §6.1 and §7 for every drawer and sheet.

**Boards:** changed `Security-rename-error-mobile`, `Profile-email-code-mobile` (no size change).

**Tests (revision round, 2026-11-05).** Rig on the 52 changed, new and base boards (`Main`, `Security` and every copy), port 8831, 10 viewports: light build (`.design/edges/qa/rev/light`) **0 blockers, 0 majors, 396 minors**; `BOARD_THEME=dark` build with `--schemes dark` (`.design/edges/qa/rev/dark`) **0 / 0 / 396**. Every minor is a known class: 158 targets on inert pages behind overlays, 105 footer links at 24 px on fine pointers, 80 sidebar items at 36 px, 30 axe "needs review" contrast items (scrims, transparent code inputs, **Link** on Mist), 23 inline links. Text spacing (1.4.12, 320 and 1440, both builds): only the known false positives (the skip link; the sr-only ", opens in a new tab" in **Contact support**). Block labels at 320: 90 block buttons, widest 226 px (≤ 256), none taller than 44. Drift: the intended sidebar copies only. Icons: 0 bad. Every `href`, form `action` and `dc-import` target exists.
