# Identity edges: coverage inventory (WP2.2)

Date: 2026-11-05. Documentation only: nothing on a canvas, artifact or app was changed.

## How to read this

**Status**
- `drawn`: a board exists.
- `copy-only`: the copy or rule is written in a spec (or review) but no board draws it.
- `missing`: no board and no decided copy. A decision may exist (see the rules list after each table), but nothing says what the screen looks like or says.

**Sources checked**
- Live canvas, Richie Identity: `https://claude.ai/artifact/9EbW44Zt3zAyXumY4m4Rr2`, version `1793882377-cd80`, 35 boards in `project/canvas.json`. Referred to as **ID canvas**.
- Live canvas, Richie Account Settings: `https://claude.ai/artifact/SQiXuTqyASonZHTVzVFW3y`, version `1793877977-4d5d`, 56 `*.dc.html` boards. Referred to as **AS canvas**. The local mirror `.design/canvas/project/` has the same 56 boards (file list compared with the live list; no differences). Where the live list and the mirror could differ, the live one was used.
- `docs/design/account-settings/spec.md` (Revision 5), cited as **spec §**. `docs/design/account-settings/evaluation.md`, cited as **eval**. `docs/design/how-we-design.md`, cited as **HWD**.
- Identity review, `https://claude.ai/artifact/WBt4MiWTL6hjzD2B6gfVFD`, cited as **review** (UX evaluation sections: Verdict, "Since the evaluation", "Still open", "Missing pages" §4, "Engineering notes" §5, cross-cutting C1 to C6).
- Board copy was read from the board files for: ID `Signin`, `Signin-unknown`, `Signin-passkey-failed`, `Signin-send-failed`, `Verify-locked`, `Verify-expired`, `Verify-sent`, `Verify-incomplete`, `Passkey-signin`, `Passkey-signin-failed`; AS `Stepup-cant-get-code`, `Security*`, `Profile-email-*`. Other ID boards were checked by file name and title only.

**ID boards that exist (for reference):** `Main`, `Signup-{mobile,dark,error,exists}`, `Signin`, `Signin-{mobile,dark,error,unknown,passkey-waiting,passkey-failed,send-failed}`, `Verify`, `Verify-{mobile,dark,error,expired,checking,resend,locked,sent,incomplete}`, `Passkey-offer`, `Passkey-offer-{mobile,dark,waiting}`, `Passkey-{created,not-created,exists}`, `Passkey-signin`, `Passkey-signin-{mobile,dark,waiting,failed}`.

---

## 1. Edge pages

| State | Status | Source | Notes |
|---|---|---|---|
| Sign-in request expired while on Verify (IdentityServer login request timed out) | missing | Review §5 Engineering notes, "Security and IdentityServer"; review "Missing pages" item 5; review "Still open" | Only the instruction exists: "Restart it with a friendly page, not an error page." No title, body or button. |
| Sign-in request expired while waiting for a passkey (`Signin-passkey-waiting`, `Passkey-signin-waiting`) | missing | Same as above | Same instruction. The existing passkey failed pages say "The request timed out or you cancelled it." (`Passkey-signin-failed`), which is about the passkey prompt, not the IdentityServer request. |
| Sign-in request expired on a form left idle (Sign in, Create account, Passkey offer) | missing | Review §5 (same note, generalised) | No source names these pages. Same restart pattern would apply. |
| Signed-in session expired while in account settings (navigation or a save returns 401) | missing | None. Spec §3.1 to §4.0 never mention it | The 10-minute step-up (spec §4.4) is not this: it is a re-check inside a live session. |
| Signed-in session expired in the middle of a dialog (step-up, rename, change email) | missing | None | Typed input would be lost. Spec §4.0 says dialogs close nothing on outside click so input is never lost by accident. |
| Generic error (IdentityServer error page, unknown failure, bad request) | missing | Review "Missing pages" item 5 ("a generic error") | No copy. Neither canvas has an error page. |
| Offline on sign-in pages (Sign in, Create account, Verify, Passkey pages) | missing | HWD §6 "Missing from identity" ("server or network error") | The `Alert` pattern is settings-only. Identity has no offline treatment. |
| Offline on settings pages (page-level alert) | drawn | AS `Preferences-offline` (spec §4.0 "Offline, on every page", §4.5) | Alert default variant, `WifiSlash` icon. Title "You’re offline", description "Changes won’t save until you’re back online." The other settings pages reuse the pattern: copy-only for them (spec §5 "Offline" row says "same"). |
| Offline or network failure on a single action (inline) | copy-only | Spec §4.0 "Failure, offline and loading copy" | Three patterns: "We couldn’t {phrase} because you’re offline." / "We couldn’t {phrase}. Check your connection." / "We couldn’t {phrase}. Try again in a moment." Phrase table covers every settings action. |
| Code not sent on Sign in (server or network failure) | drawn | ID `Signin-send-failed` | "We couldn’t send a code. Try again in a minute." Status below the button, `role="status"`. |
| Code not sent on Create account, Passkey sign-in ("Get a code by email instead"), Verify ("Send a new code") | missing | Review C2 ("No rate-limit or send-failure state anywhere a code is sent"); only Sign in got a board | Settings has the pattern as copy (`Stepup` "Send failed": "We couldn’t send a code. Check your connection." with a **Try again** button, spec §4.4). The two copies differ ("Try again in a minute" vs "Check your connection"). |
| Too many requests: full page for a 429 (rate limited anywhere) | missing | Review "Missing pages" item 5 ("too many requests") | No page, no copy, no `Retry-After` handling decided. |
| Code send cooldown (60 s) on Verify | drawn | ID `Verify` ("Send a new code in 0:59"), `Verify-resend`, `Verify-sent` | Timer, available state, and "New code sent. Use the code in the newest email." |
| Code locked after 4 wrong tries | drawn | ID `Verify-locked` | "No tries left for this code." and "You can send a new code in 0:27"; button "Send a new code" unavailable. |
| Code expired | drawn | ID `Verify-expired` | "This code has expired. Send a new one to continue." |
| Sign in: re-entering the same address within 60 s of a send | copy-only | Review C2 | "go straight to Verify without sending, and show ‘Send a new code in 0:41’". Not drawn. |
| Rate limit on the Sign in email lookup (unknown-email probing) | missing | Review "Since the evaluation" ("rate-limit the lookup") | Decision to rate-limit; no limit number, no screen, no copy. |
| Rate limit on signed-out data download code requests | copy-only | Spec §4.8 `Export-start` row "Existence and limits", §9 Q8 | 5 per address and 20 per IP per hour; button becomes "Email me a code in 41:12"; polite line "You can ask for a new code now." The deletion export pages are drawn (AS `Export-start`, `Export-request-code`) but the limited state is not. |
| Settings code send rate limit | drawn | AS `Stepup-code-locked`, `Profile-email-code` | Countdown in the button; 1:00 timer. |
| Signed out: logout confirmation page ("You’re signed out") | missing | Review "Missing pages" item 5 ("signed-out confirmation (IdentityServer logout)") | No board, no copy. |
| Signed out: logout prompt (IdentityServer asks "Do you want to sign out?" when the logout request is not trusted) | missing | Review item 5 (implied by "IdentityServer logout") | Not named separately in any source. Listed because the scope names it. |
| Sign out of this device from settings (sidebar footer, mobile index) | copy-only | Spec §3.3 "Sign out" | "Signs out this device only, with no confirmation, and lands on identity sign-in." No signed-out message on the landing. |
| Signed out everywhere: after account deletion request | drawn | AS `Data-deleted` | "Your account will be deleted on December 5" / "We’ve signed you out on every device." Deletion only. |
| Signed out everywhere: "Sign out of other devices" (confirm, toast) | copy-only | Spec §4.3 "Where you’re signed in" | Confirm: title "Sign out of 2 other devices?" Toast "Signed out of 2 other devices". No board. |
| Signed out because another device ended this session (revoked) | missing | None | Related to the session-expired rows above. |
| Account scheduled for deletion, shown on sign-in | drawn | AS `Deletion-pending` | Not an edge page in the strict sense; listed because it interrupts sign-in. |

**Counts, scope 1:** drawn 8, copy-only 5, missing 11 (24 rows).

### Decided rules that constrain this item

- Verify: codes are 8-character Crockford Base32, expire after 5 minutes, allow one send per 60 s and 4 tries per code (Duende User Management limits). Review footer, review §5; spec §4.0 "Countdowns start at 1:00".
- Timers come from the server's next-allowed time, not a client guess (spec §4.0 "Copy rules"; review §5 "Drive timers from server timestamps").
- One verb for a new code: **"Send a new code"** everywhere; identity should adopt it (spec §4.0; review "Since the evaluation"). Already adopted in the ID boards read.
- Do not auto-create: **Sign in rejects unknown emails**, and revealing that an account exists is accepted, with the lookup rate-limited (review "Decided by the product owner"). Create account shows "already has an account". Both pages therefore no longer give identical responses for known and unknown emails (this supersedes review C1 and §5 "Nothing anywhere says ‘account not found’").
- The IdentityServer sign-in request can expire on Verify or while waiting for a passkey; restart it with a friendly page, not an error page (review §5).
- Error copy says what to do, with real numbers; never "invalid", "oops" or an exclamation mark (spec §4.0 "Copy rules").
- The three failure patterns (offline, network failure, server error) and the settings offline alert (spec §4.0).
- Step-up lasts 10 minutes after the last sign-in or step-up, per device (spec §4.4, §9 Q2).
- Sign out: this device only, no confirmation, lands on identity sign-in; "Sign out of other devices" needs no step-up (spec §3.3, §4.3, §4.4).
- Deletion signs the person out everywhere and revokes every grant (spec §4.8 lifecycle).
- Identity pages carry nothing workspace-related (review "Decided by the product owner"; spec §1.6 principle 2).
- Status and alert roles: `role="alert"` only when focus does not move to the message (spec §4.0). Identity errors should take focus on arrival (review "Applied on the canvas").
- Solo-page frame: centred column max 460, 30/36 title, 1x shiba mark 16 px above the title, footer Help / Privacy / Terms (spec §4.8 "Solo page frame"). The ID canvas uses the 62 x 78 mark with a state badge (`Passkey-signin-failed` uses a neutral badge).
- Document titles use an en dash and "Error: " prefix on a failed submit (spec §3.1). ID board titles do not use the dash form yet.

---

## 2. Account settings: passkey management and email settings

### 2a. Passkeys (`/account/security#passkeys`)

| State | Status | Source | Notes |
|---|---|---|---|
| Passkey list, 2 rows (desktop) | drawn | AS `Security` (spec §4.3) | Rows: MacBook Pro (iCloud Keychain, added March 12, last used today), Pixel 8. Actions **Rename**, **Remove**. |
| Passkey list, mobile and dark | drawn | AS `Security-mobile`, `Security-dark` | |
| Loading (skeleton rows) | drawn | AS `Security-loading` (spec §4.3 "Loading") | sr status "Loading your sign-in settings…". |
| Empty | drawn | AS `Security-no-passkeys` | "No passkeys yet" / "Add a passkey so you can still sign in if you lose access to your email." (also the recovery nudge, eval N2-04 resolved). |
| List load error | copy-only | Spec §4.3 "Load error" | `Alert` (destructive), "Couldn’t load your passkeys", "Check your connection.", action **Try again**. "The same pattern applies to every list." |
| Add: idle | drawn | AS `Security` | Button **Add a passkey** (default, `Plus`), status slot reserved. |
| Add: step-up needed first | drawn | AS `Stepup` (screen A), `Stepup-code` | Description "To add a passkey, use one you already have or an email code." |
| Add: waiting for device | drawn | AS `Security-passkey-waiting` | "Waiting for your device…" / "Follow the prompt on your screen to finish." |
| Add: canceled or timed out | drawn | AS `Security-passkey-failed` | "The passkey prompt closed. Try again." (eval SE-4 asked for a next step; the board text is as shown.) |
| Add: server rejects the new credential | copy-only | Spec §4.0 failure table | Phrase "save your new passkey". |
| Add: already registered (`InvalidStateError`) | copy-only | Spec §4.3 "Add-a-passkey states" | "You already have a passkey on this device. Use it to sign in." |
| Add: browser does not support passkeys | copy-only | Spec §4.3 | Button hidden; "This browser can’t create passkeys. Try the latest Chrome or Safari." |
| Add: at the limit (10) | copy-only | Spec §4.3 | Button hidden; "You have 10 passkeys, the limit. Remove one to add another." |
| Add: added | copy-only | Spec §4.3 | New row first ("Added just now"); "Passkey added. Rename it so you can tell your passkeys apart." |
| Rename dialog (desktop dialog, mobile drawer) | copy-only | Spec §4.3 "Rename" | "Rename passkey"; field **Name**; **Cancel**, **Save** ("Saving…"). No board. |
| Rename errors | copy-only | Spec §4.3 | "Enter a name for this passkey." / "Use 40 characters or fewer." Server failure: phrase "rename this passkey" (spec §4.0). |
| Rename done | copy-only | Spec §4.3 | Status "Passkey renamed." |
| Remove confirm (desktop) | drawn | AS `Security-remove-passkey` | "Remove the “Pixel 8” passkey?" Body: "You won’t be able to sign in with it anymore. You can still use your other passkey or an email code." |
| Remove confirm (mobile action sheet) | drawn | AS `Security-remove-passkey-mobile` | |
| Remove confirm, last passkey | copy-only | Spec §4.3 "Remove" | "It’s your only passkey. After you remove it, you’ll sign in with an email code." |
| Remove: step-up after the confirm | drawn | AS `Stepup` family | Hand-off rules in spec §4.0. |
| Remove: failure | copy-only | Spec §4.3, §4.0 | Alert inside the confirm, or inside step-up when it ran. Phrase "remove the “Pixel 8” passkey". |
| Remove: toast and focus | copy-only | Spec §4.3 | Toast "Passkey removed" / "You can also delete it from Google Password Manager." Focus to the "Passkeys" `h2`. |
| Passkey list, long names or many rows | missing | Spec §5 "Long or extreme data" has no Security passkeys entry | Row names are unconstrained in the spec (rename caps at 40). |
| Passkey activity entries ("Added the passkey “Pixel 8”") | drawn | AS `Security` (recent security activity) | Entries for rename, remove, and add-failed are not specified. |

**Counts, 2a:** drawn 12, copy-only 12, missing 1 (25 rows).

### 2b. Email (`/account/profile#email`, `/account/security`)

| State | Status | Source | Notes |
|---|---|---|---|
| Email row on Profile | drawn | AS `Main` (spec §4.2 Section B) | jamie.rivera@northwind.coffee; "We send email codes and security alerts here."; **Change email**. |
| Email row on Security ("Email codes") | drawn | AS `Security` | "Always on. Each code works once and expires after 5 minutes." Link **Change email** to `Main#email`. |
| Long email | drawn | AS `Profile-extreme` | Wraps after the @, domain never truncated (spec §4.0 long-email rule). |
| Step-up before change email | drawn | AS `Stepup`, `Stepup-code`, `-error`, `-locked`, `-mobile`, `-dark` | "To change your email, use a passkey or an email code." |
| Change email step 1 (desktop, filled field) | drawn | AS `Profile-email-change` | Title "Change your email"; field **New email**; **Cancel**, **Send code**. |
| Step 1 error: empty | copy-only | Spec §4.2 step 1 | "Enter your new email address." |
| Step 1 error: malformed | copy-only | Spec §4.2 | "Enter a full email address, like name@example.com." |
| Step 1 error: same address | copy-only | Spec §4.2 | "That’s already your email. Enter a different one." |
| Step 1 error: belongs to another account | copy-only | Spec §4.2, §9 Q4 | "That email already belongs to another Richie account. Use a different one." (Q4: say so rather than stay enumeration-safe.) |
| Step 1 error: send failed | copy-only | Spec §4.2, §4.0 | "We couldn’t send a code. Check your connection." Offline and server variants in §4.0. |
| Step 1 busy | copy-only | Spec §4.2 | "Sending code…" (the board carries it as a hidden state). |
| Change email step 2, code entry | drawn | AS `Profile-email-code` | "Check your new email"; "Enter the code we sent to **jamie@rivera.studio**."; "Codes expire after 5 minutes."; "Send a new code in 1:00"; **Use a different email**. |
| Step 2: wrong code with tries left | copy-only | Spec §4.2 ("Errors as in §4.4") | "That code isn’t right. You have 3 tries left." (2, then "1 try left"). Drawn for step-up only (AS `Stepup-code-error`). |
| Step 2: locked (4th wrong try) | copy-only | Spec §4.2, §4.4 | Drawn for step-up only (AS `Stepup-code-locked`). Copy in step 2 would need its own description ("you’ll need a new code"). |
| Step 2: expired | copy-only | Spec §4.4 | "This code has expired." Button **Send a new code**. |
| Step 2: new code sent | copy-only | Spec §4.2, §4.4 | "We sent a new code." for 5 s. Polite sr "We sent a new code." |
| Step 2: send failed | copy-only | Spec §4.4 | **Try again**. |
| Step 2: busy | copy-only | Spec §4.2 | "Checking code…". |
| Done: dialog closes, row shows new address | copy-only | Spec §4.2 "Done" | Status "Email changed. We’ve let your old address know." No "email changed" board (`Profile-name-saved` is the name). |
| Security email to the old address | missing | Spec §4.2 "Done" says it is sent; the §4.8 email table has only deletion emails | No subject, body or button. Review does not specify it. |
| Mobile drawers for change email | missing | AS has `Stepup-mobile` only; no `Profile-email-*-mobile` | Spec §4.0 says dialogs become bottom drawers below 768 px. Spec §5 harness covers `Profile-email-change` at widths. |
| Passkeys signal new email | copy-only | Spec §4.2 "Done" | `PublicKeyCredential.signalCurrentUserDetails()` where supported. Behaviour only. |
| Preferences: "Emails from Richie" section | drawn | AS `Preferences`, `-mobile`, `-dark`, `-offline` (spec §4.5) | "Account and security" row "Always on" (cannot be turned off); Product news (off), Getting started tips (on), Feedback requests (off). Note: "Every email from us also has an unsubscribe link." |
| Preferences save failed (control reverts) | copy-only | Spec §4.5 "Errors" | "We couldn’t save that change. Check your connection." |
| Unsubscribe landing page (from an email link) | missing | Spec §4.5 note promises the link; no page | Signed-out destination, not designed or specified. |

**Counts, 2b:** drawn 7, copy-only 15, missing 3 (25 rows).

### Decided rules that constrain this item

- Step-up is required for: change email, add a passkey, remove a passkey, request a data download, delete account. Not for rename, names, photo, preferences, or any sign-out (spec §4.4).
- Step-up window: 10 minutes after last sign-in or step-up, per device (spec §4.4, §9 Q2).
- Passkey limit is 10 (spec §4.3). Each device name is at most 40 characters (spec §4.3).
- After the confirm closes, step-up opens; cancel or Escape returns focus to the **Remove** button; failures show inside step-up (spec §4.0 "From a confirm to step-up").
- Toast rule: a toast only when the acted-on thing left the screen (passkey removed, device signed out, app removed); toasts have no action buttons (spec §4.0).
- Dialog rules: no close (×); Escape counts as Cancel; outside click closes nothing; every dialog is a form; one height per dialog; async confirms stay open; mobile uses floating drawers and action sheets (spec §4.0).
- Change email: "Your current address keeps working until then." Status "Email changed. We’ve let your old address know." A security email goes to the old address (spec §4.2).
- Changing to an address that already has an account says so (spec §9 Q4).
- One verb, "Send a new code"; the term "email code" (not "sign-in code", "verification code"); "device" (not "session"); sr label "Email code, 8 letters and numbers" (spec §4.0 naming table).
- Code rules: Crockford Base32 normalisation (I and L to 1, O to 0), 4 + 4 slots, paste fills 8, no auto-submit in settings (spec §4.4). Note the review's engineering notes ask for auto-submit on the 8th character in identity; the two specs disagree (open question for the identity canvas).
- Countdowns start at 1:00 (Duende minimum 60 s between sends); codes expire after 5 minutes; 4 tries (spec §4.0, §4.4).
- No passwords, ever; the one allowed line is "There’s no password to forget." (spec §1.6 principle 3).
- Failure copy uses the phrase table in spec §4.0 (e.g. "send a code", "rename this passkey", "save your new passkey").
- Preferences: security email and codes "can’t be turned off" and show "Always on", never a disabled switch (spec §4.5).
- Settings fields use the suiss filled style with a permanent 1 px `--input` edge, 44 px tall (spec §2.2). Identity still draws 52 px outlined fields.
- Passkey remove toast hints at the password manager copy: "You can also delete it from Google Password Manager." Provider names are exempt from the no-passwords rule (spec §1.6).

---

## 3. Account recovery when someone lost access to their email

| State | Status | Source | Notes |
|---|---|---|---|
| Settings, step-up screen B: "Can’t get the code?" link | drawn | AS `Stepup-code` (spec §4.4 screen B "Alternatives") | Plain buttons **Use a passkey instead** (only with a passkey) and **Can’t get the code?** (always). |
| Settings, screen C, with a passkey (desktop) | drawn | AS `Stepup-cant-get-code` (spec §4.4 screen C) | Title "Can’t get the code?"; list "Look in your spam or junk folder." / "Wait a minute, then send a new code." / "Use a passkey instead, if you’ve set one up." + button; paragraph "Can’t get email at **…** any more? Contact Richie support. We’ll check it’s you and help you change your email."; **Back**, **Contact support**. |
| Settings, screen C, with a passkey (mobile drawer) | drawn | AS `Stepup-cant-get-code-mobile` | |
| Settings, screen C, no passkey (two-item list, no passkey button) | copy-only | Spec §4.4 screen C ("otherwise the list has two items") | The board draws the with-passkey variant only. |
| Contact support: form, new tab, topic "I can’t get my email code", account attached | missing | Spec §4.4 screen C footer row | Behaviour is decided; the support form is outside both canvases. |
| Support reply expectation on screen C ("We usually reply within 1 business day") | missing | Eval R2.5 item 3 ("Product must confirm") | Not on the board. |
| Support form must collect a reachable contact, not the dead mailbox | missing | Eval R2.5 item 2 | Not specified. |
| Signed-out entry from Verify ("Can’t find “Your Richie code”… use a different email") | drawn | ID `Verify` family | Only "Check spam, or use a different email." There is no lost-mailbox route on any Verify state. |
| Signed-out entry from Verify (lost mailbox, with or without a passkey) | missing | Eval R2.5 item 4 ("same route at identity sign-in… a handoff item for the identity canvas") | |
| Signed-out entry from Sign in (no passkey on this device, no mailbox) | missing | Same | `Signin` has no recovery link; footer **Help** is a `#help` placeholder. |
| Signed-out entry from Passkey sign-in (passkey fails, mailbox lost) | missing | Review "Missing pages" item 7 ("reachable from Help") | `Passkey-signin-failed` offers **Try again** and "Get a code by email instead" only. |
| Recovery with a passkey: use it as the way back in | drawn | ID `Passkey-signin`; AS `Security-no-passkeys` nudge | Passkey sign-in works without email; the nudge text is on `Security-no-passkeys`. |
| Recovery without a passkey: support-assisted | copy-only | Spec §9 Q7 (working assumption) | Support verifies identity under a written procedure, then changes the email. |
| Written support verification procedure and who may change an email | missing | Eval R2.5 "Product must confirm" 1 | Not written. Called the main account-takeover route. |
| "Recently recovered" state: security-activity entry | missing | Spec §9 Q7 | Entry decided to exist, no copy, no icon, no board. |
| "Recently recovered": step-up for every sensitive action for 7 days | copy-only | Spec §9 Q7 | Rule only. How step-up says it (a changed note instead of "we won’t ask again for 10 minutes") is not specified. |
| Notification to the old and the new address when support changes the email | missing | Spec §9 Q7, eval R2.5 | Rule only. No email subject or body. Spec §4.8 has deletion emails only. |
| Account-takeover warning after recovery ("If this wasn’t you…") | missing | None | |
| Passkey nudge: adding a passkey as the way back in | drawn | AS `Security-no-passkeys` | "Add a passkey so you can still sign in if you lose access to your email." |
| Deleted-account addresses (Signin-deleted, Export-start) as a recovery for lost data | drawn | AS `Signin-deleted`, `Export-start`, `Export-request` | Separate from lost mailbox; listed because eval ties "lost the link" to it (spec §5 matrix row "Can’t receive the code, or lost the link"). |

**Counts, scope 3:** drawn 7, copy-only 3, missing 10 (20 rows).

### Decided rules that constrain this item

- **Support-assisted recovery is a working assumption** (spec §9 Q7), not a confirmed decision: support verifies identity under a written procedure, then changes the email; the old and the new address are both notified; the account is marked *recently recovered*: a security-activity entry, and step-up required again for every sensitive action for 7 days.
- Screen C opens from **Can’t get the code?** inside the same dialog, keeps its height, and **Back** returns to screen B with the code, tries and countdown unchanged (spec §4.4 screen C).
- The passkey button and its list item appear only when the person has a passkey and the browser supports WebAuthn (spec §4.4).
- **Contact support** opens the support form in a new tab with topic "I can’t get my email code" and the signed-in account attached; accessible name "Contact support, opens in a new tab" (spec §4.4).
- The signed-out equivalent is a handoff item for the identity canvas: "A signed-out person with no passkey and no mailbox can’t reach step-up at all" (eval R2.5 item 4).
- Identity review asks that recovery be reachable from Help (review "Missing pages" item 7). Footer **Help** is on every ID board, as the same link in the same place (review "Measured baseline": consistent help 3.2.6).
- Passkey recovery nudge wording (spec §4.3; eval N2-04 resolved).
- A passkey-signed-in person who has lost the mailbox is already unblocked; recovery matters when there is no passkey (spec §9 Q7 text).
- Sign in rejects unknown emails (review). A recovered email change goes through the same checks as Q4: "That email already belongs to another Richie account."
- "Not you?" on Passkey sign-in removes the remembered account from the device (review decisions); it is not a recovery route.
- Rate limits for code sends: one per 60 s; 5 per address and 20 per IP per hour is the stated limit for signed-out export only (spec §9 Q8). No limit is stated for sign-in sends.
- Copy rules: "we" for Richie, "you" for the user, real numbers, no "oops" (spec §4.0).
- Open product questions still unresolved: written procedure, support reply time, whether the support form can reach a person with no mailbox (eval R2.5).

---

## 4. Single sign-on

| State | Status | Source | Notes |
|---|---|---|---|
| "Continue with Google" and "Continue with Microsoft" buttons on Sign in and Create account | missing | Review "Missing pages" item 8 ("Future: Google/Microsoft sign-in and workspace-enforced SAML SSO… decide where they go now") | Not on any ID or AS board. |
| Workspace-enforced SAML or OIDC SSO | missing | Review item 8 | |
| Email-domain routing / home realm discovery (type email, then route to the company IdP) | missing | Review item 8 ("routing by email domain changes the Sign in layout") | Today's Sign in has one email field with `username webauthn` autocomplete and a separate "Sign in with a passkey" button. |
| SSO error and edge pages (IdP failure, user not found at IdP, domain not allowed, SSO enforced so no email code) | missing | None | |
| Signing in with an external provider when an account already exists for the same email (linking) | missing | None | Conflicts with review C1 (Sign in and Create account are the same server operation) if unresolved. |
| Settings: where linked external providers appear (Sign-in and security) | missing | None | AS `Security` has "Email codes" and "Passkeys" only. |
| Settings: step-up for an SSO-only person (no email code, maybe no passkey) | missing | None | Spec §4.4 assumes an email code or a passkey is always available. |
| Settings: change email for an SSO-managed account | missing | None | Spec §4.2 assumes the email is the credential. |
| Connected apps (OAuth grants) | drawn | AS `Connected`, `-empty`, `-remove`, `-mobile`, `-dark` (spec §4.6) | **Not SSO.** These are third-party apps that use Richie as the provider; listed so nobody reads "OAuth" in the AS canvas as sign-in with external providers. |

**Counts, scope 4:** drawn 0 (one non-SSO row is `drawn` for disambiguation only), copy-only 0, missing 8.

Searched for "Google", "Microsoft", "SAML", "OIDC", "SSO", "single sign", "realm", "federat", "external provider" across the three docs, the spec and evaluation, and the 56 local AS boards: the only hits are Google Fonts links, "Google Password Manager" (passkey provider name, spec §4.3), and review "Missing pages" item 8. Both canvases are silent. The ID canvas file list was checked by name only; no SSO board exists in `project/canvas.json`.

### Decided rules that constrain this item

- No decision exists. Review "Still open" and "Missing pages" item 8 only ask for a decision: "decide where they go now".
- Sign-in today is an email code or a passkey, and the product has no passwords (spec §1.6 principle 3; review "Verdict"). The spec §1.6 principle 3 sentence "Sign-in is an email code or a passkey" would need changing if external providers ship.
- Accounts are personal, not workspace-owned (project memory `richie-product.md`; spec §1.6 principle 2: account settings show only the account). Workspace-enforced SSO would be a workspace concept that identity currently excludes ("Identity pages carry nothing workspace-related", review decisions).
- Sign in rejects unknown emails (review). Any provider-based first sign-in would need a separate rule for creating the account.
- Sign in and Create account are the same operation on the server for an unknown email, per the review's flow-level finding C1; the decision above removes the unknown-email auto-create path for Sign in. External provider first sign-in would reopen it.
- Conditional passkey UI: email field uses `autocomplete="username webauthn"` and the passkey button is already outline-secondary (review "Sign in"). Provider buttons would sit with it; no layout is decided.
- Layout and copy rules from the existing canvas: sentence case, 44 px fields in settings, 52 px fields in identity (to be aligned), "Send sign-in code" as the primary label today, and "or" as the divider (`Signin`).
- Suiss addendum for SSO blocks is in progress (WBS WP1.1 "Suiss addendum (edge, recovery, SSO blocks)", `docs/design/identity-edges/wbs.md`); not yet read here.
- Duende supports external providers and Home Realm Discovery in the IdentityServer UI (see the `duende-skills:identityserver-ui-flows` skill); no project decision uses it.

---

## Handoff items already waiting for the identity canvas

| Item | Source | What is waiting |
|---|---|---|
| **`Signin-deleted`**: "This account was deleted" on the Sign in email step, for an address deleted in the last 30 days | Spec §3.1 last row, §4.8 `Signin-deleted`; drawn on AS `Signin-deleted` "for reference" | Body "We deleted the Richie account for **…** on December 5. Until January 4, 2027, you can still ask for a copy of your data." Buttons **Download your data** (to `/export` with the address prefilled), **Create a new account** (outline; sign-up prefilled), **Use a different email** (plain). Shown after the email step, before any code is sent. A live account wins. After day 60, treat the address as new. |
| **`Deletion-pending`** after a successful sign-in in days 0 to 30 | Spec §4.8; AS `Deletion-pending` | Identity interstitial; **Keep my account** continues to the sign-in's return URL. Needs a place in the identity flow after Verify and after passkey sign-in. |
| Sign-up error for an address in its 30-day pending window | Spec §4.8 table ("Sign-up with the same email") | "That email already has a Richie account. Sign in to keep it." with the link "Sign in". ID `Signup-exists` is drawn but this variant is not noted. |
| **Return URL** on sign-in | Spec §4.8 Keep; eval DL-3 | Sign-in must hold the return URL (a deep link or an OAuth authorize request) across the interstitials. |
| **"Send a new code"** as the one verb | Spec §4.0 "Copy rules" | Already applied on the ID boards read (`Verify-locked`, `-expired`, `-sent`). Identity's label "Verify code" and "Send sign-in code" stay (review). |
| **44 px fields** (identity draws 52 px outlined) | Spec §2.2 "Identity follow-up", §2.2 bullet "Why not identity’s outline" | Move identity to 44 px suiss filled fields so the two apps match. |
| **Sr label "Email code, 8 letters and numbers"** | Spec §4.4 screen B (sr label row) | ID boards still say "Code from your email, 8 letters and numbers". |
| **"Last name optional"** | Spec §9 Q5 | Already applied in identity (review "Applied on the canvas"). |
| **Lost mailbox route from sign-in** | Eval R2.5 item 4 | Waiting. See section 3. |
| **Disabled / unavailable states** | Spec §2.3 "Disabled and unavailable" | Fixes identity's 2.09:1 dimmed primary; `Verify-locked` still uses `is-unavailable` and `aria-disabled` (not re-measured here). |
| **Dark tokens** | Spec §2.3 "Dark `muted`" | Use `#161617`, not `#1d1d1f`; dark filled primary stays `#0071e3`. |
| **Production head items** | Review "Still open" | Self-host Inter, add color-scheme and theme-color metas, `rc--system` class. Not design work. |
| **Auto-submit versus no auto-submit** | Review §5 (auto-submit on the 8th valid character in identity) vs spec §4.4 ("No auto-submit; Enter submits") | The two specs disagree; one should be changed. |
| **Cooldown shown the same way** | Spec §4.8 `Export-start` limited state | Countdown button pattern "Email me a code in 41:12". Identity has no matching state for sign-in rate limits yet. |
| **`Email me a code in m:ss`** format | Spec §4.0 | m:ss timers, tabular figures; identity Verify already uses "Send a new code in 0:59". |
| Document titles with an en dash and the "Error: " prefix | Spec §3.1 | ID boards use plain titles like "Sign in to Richie, code not sent". |

---

## Summary table

| Scope item | drawn | copy-only | missing | Rows |
|---|---|---|---|---|
| 1. Edge pages | 8 | 5 | 11 | 24 |
| 2a. Passkeys | 12 | 12 | 1 | 25 |
| 2b. Email | 7 | 15 | 3 | 25 |
| 3. Recovery | 7 | 3 | 10 | 20 |
| 4. SSO | 0 | 0 | 8 | 8 (+1 disambiguation row) |

## Biggest gaps

1. Every IdentityServer edge page (request expired, error, too many requests, logout) is missing, and settings has no session-expired state.
2. Offline on identity pages and a send-failed state on Create account, Passkey sign-in and Verify are missing.
3. Signed-out recovery for a lost mailbox: Verify, Sign in and Passkey sign-in have no route; support policy (Q7) is a working assumption with no procedure, reply time or contact collection.
4. Recently recovered: activity entry, 7-day step-up wording and the notification emails to old and new addresses have no copy.
5. Change email: error states and "new code sent / locked / expired" exist only as copy; no mobile drawers; the security email to the old address has no text.
6. Rename passkey, list load error and the added / at-limit / already registered / not supported states are copy-only; no board.
7. SSO is missing in every canvas, spec and review decision; it also touches the rules "Sign in rejects unknown emails", "no passwords", and "no workspace concepts".
