# Identity edges: state coverage and buildability (WP4.5 tester report)

Tester WP4.5, Phase 4, Richie Design Program, identity edges. 2026-11-05. Audit only: no boards, specs or `docs/engineering/` were edited. Read: `spec.md` Part A §3, §4.4-4.7, §5, §8, §9 and Part B rulings B-1 to B-5; `spec-edge.md`, `spec-settings.md`, `spec-recovery.md`, `spec-sso.md`; `inventory.md`; AS §4.4; all 73 boards in `.design/edges/project/` (links, forms, slots and copy compared with the specs by script).

**Legend.** `Board` = drawn on that board. **C** = copy-only: spec section, then the board whose layout it reuses. **H** = no board; covered by the rig or the forced-dark build only (Part A §5.3 departure for dark; mobile is by plan). **n/a** has a reason. **MISSING** = a state the page needs and nothing specifies or draws. "(!)" marks a copy-only state that adds an element, height or focus target (see the COV id).

**Result in one line.** All 73 boards exist and match `canvas.json`; copy on the boards matches the specs. No state is wholly missing, but 14 major issues remain: one dead end (J7), one lockout (R2), an unspecified support form, three contradictions between specs, and four copy-only states that break §5.3. See Findings.

## 1. State matrix

### 1.1 Edge pages (E1-E14, plus A1 states WP3.2 owns)

| Page | Default | Loading / busy | Error: validation | Error: server | Error: offline | Error: rate limit | Empty / success | Mobile | Dark |
|---|---|---|---|---|---|---|---|---|---|
| E1 Sign-in timed out | `Timeout` | n/a (a link; full navigation) | n/a (no input) | n/a (a failed redirect is E4) | E8 | n/a | -> `Main` with email; variants (sign-up, passkey E1a, provider E1b, no email) C spec-edge §4 E1 · `Timeout` | `Timeout-mobile` | `Timeout-dark` |
| E2 Signed out in settings | `Session-expired` | n/a (links) | n/a (no input) | n/a (is the failure) | n/a (a 401 cannot arrive offline) | n/a | nothing-unsaved variant C §4 E2 · `Session-expired`; "We put back what you typed" C · AS `Profile-name-saved` slot | `Session-expired-mobile` | H |
| E3 Sign in after session ended | C spec-edge §4 E3 · `Signin-provider-canceled` (alert layout; 3 description lengths undrawn) | as A1 (`Main`, `Signin-redirecting`) | as A1 | as A1 | `Signin-offline` | `Signin-limited` | -> return URL | H | H |
| E4 Something went wrong | `Error` | MISSING for "Try again" on the other-routes variant (reload, undefined) (!) COV-34 | n/a | is the page | E8 | n/a | "Reference copied" / clipboard fallback C §4 E4 · `Error`; third-party, sign-out and other-route variants C · `Error`, `Error-notfound` | `Error-mobile` | `Error-dark` |
| E5 Link doesn’t work | `Error-link` (recovery link); owner-cancel and other-link variants C · `Error-link` | n/a | n/a | n/a (same 410 for every cause) | E8 | L7 -> E10 | links | H | H |
| E6 Page not found | `Error-notfound` | n/a | n/a | n/a | E8 | n/a | links | H | H |
| E7 Offline, inline | `Signin-offline` | button busy per page, C §4 E7 | n/a | C §4 E7 · `Signin-offline` | is the state | -> E9 | "You’re back online." C · slot | H | `Signin-offline-dark` |
| E8 Offline page | `Offline` | "Trying again…" C §4 E8 · `Offline` | n/a | "We couldn’t load the page" C | "You’re still offline" C | n/a | "You’re back online" C, then reload | H | H |
| E9 Too many codes, inline | `Signin-limited` | n/a | n/a (field errors are A1’s) | n/a (is the 429) | E7 line | is the state | 0:00 C §4 E9 · `Signin-limited`; L2 variant C | H | H |
| E10 Too many requests | `Limited` | n/a (Try again reloads) | n/a | n/a | E8 | is the page; >60 min and 0:00 C §4 E10 · `Limited` | 0:00 C | H | H |
| E11 Sign out? | `Signedout-prompt` | E12 C §4 E12 · `Signedout-prompt` | n/a | C (three patterns) | C | n/a | -> `Signedout` / `Signedout-everywhere` | H | H |
| E12 Signing out | C (busy on E11) | C | n/a | C | C | n/a | -> E13 / E14 | H | H |
| E13 Signed out | `Signedout` | n/a | n/a | n/a | E8 | n/a | is the success; "Back to {client}" C · `Signedout` (!) COV-13 | `Signedout-mobile` | `Signedout-dark` |
| E14 Signed out everywhere | `Signedout-everywhere`; "Back to {client}" (!) COV-13 | n/a | n/a | n/a | E8 | n/a | is the success | H | H |

### 1.2 Account settings gaps

#### Passkeys (S1-S4)

| State | Default | Loading / busy | Error: validation | Error: server | Error: offline | Error: rate limit / not allowed | Empty / success | Mobile | Dark |
|---|---|---|---|---|---|---|---|---|---|
| List | AS `Security` | AS `Security-loading` | n/a | S3 `Security-load-error` | C spec-settings §2.3 offline · `Security-load-error` | n/a | AS `Security-no-passkeys` | `Security-mobile` | `Security-dark` |
| Load error, Try again | `Security-load-error` | "Trying again…" C §2.3 | n/a | repeat C | C | n/a | rows return, focus to h2 C | H | H |
| Add a passkey | AS `Security` | AS `Security-passkey-waiting` | n/a | C §2.2 "save your new passkey" · slot on `Security-passkey-added` | C | limit: `Security-passkey-limit` | Added: `Security-passkey-added` | H | H |
| Added, was the 10th | C §2.2 · `Security-passkey-limit` (button gone, focus moves to new row) | n/a | n/a | n/a | n/a | n/a | C | H | H |
| Already registered | C §2.2 · slot on `Security-passkey-added` / AS `Security-passkey-failed` | n/a | n/a (the browser rejects) | n/a | n/a | n/a | n/a | H | H |
| Not supported | C §2.2 · `Security-passkey-limit` quiet line | n/a | n/a | n/a | n/a | n/a | n/a | H | H |
| Prompt canceled / timed out | AS `Security-passkey-failed` (new string C) | n/a | n/a | n/a | n/a | n/a | n/a | H | H |
| Limit (10) | `Security-passkey-limit` | n/a | n/a | n/a | n/a | is the state | Remove returns Add C | H (rows wrap at 320 measured) | H |
| Rename | `Security-rename` | "Saving…" C §2.1 · `Security-rename` | too long: `Security-rename-error-mobile`; empty C · same | C "rename this passkey" · same slot | C | n/a | "Passkey renamed." C · slot on `Security-passkey-added` | `Security-rename-error-mobile` | H |
| Remove | AS `Security-remove-passkey` (+ mobile) | "Removing…" C §2.4 | n/a | S4 `Security-remove-failed` | C (offline wording) | n/a | toast C (AS); last-passkey description C · AS `Security-remove-passkey` | AS | H |
| Remove failed after step-up | C §2.4 · AS `Stepup-code` | C | n/a | C | C | n/a | n/a | H | H |

#### Email settings (S5, S6)

| State | Default | Loading / busy | Error: validation | Error: server | Error: offline | Error: rate limit | Empty / success | Mobile | Dark |
|---|---|---|---|---|---|---|---|---|---|
| Step 1, new address | AS `Profile-email-change` | "Sending code…" C §3.1 | belongs to another account: `Profile-email-taken`; empty, malformed, same address C §3.1 · `Profile-email-taken` | C "send a code" | C | L1 "Send code in 41:12" C · `Profile-email-taken` (width unmeasured, COV-26); Q-S4 10 checks (default only) | -> step 2 | `Profile-email-taken-mobile` | H |
| Step 2, code | AS `Profile-email-code` | "Checking code…" C | wrong, locked, expired C · AS `Stepup-code-error`, `Stepup-code-locked` | change failed C §3.2 | C | resend countdown (AS); locked countdown C | **New code sent**: `Profile-email-code-mobile` only (no desktop, desktop H); persistence conflicts with R4/P6 (COV-04) | `Profile-email-code-mobile` | H |
| Address taken between steps | C §3.2 -> step 1 error | n/a | `Profile-email-taken` | n/a | n/a | n/a | n/a | H | H |
| Done | `Profile-email-changed` | n/a | n/a | n/a | n/a | n/a | status + focus on **Change email** drawn | H | H |
| Security email (old address) | C §3.4 · `Email-recovery-alert` | n/a | n/a | n/a | n/a | n/a | n/a | H (email < 600) | H |

#### Google and Microsoft section (S7-S10)

| State | Default | Loading / busy | Error: validation | Error: server | Error: offline | Error: rate limit | Empty / success | Mobile | Dark |
|---|---|---|---|---|---|---|---|---|---|
| Section | `Security` | skeleton rows C §6.1 (AS) | n/a | "Couldn’t load your linked accounts" C §2.3 · `Security-load-error` (no board) | C | n/a | nothing linked: C §4.1 · `Security`; linked rows `Security`, `Security-linked` | `Security-mobile` | `Security-dark` |
| Link | `Security` | "Taking you to Microsoft…" C §1.2 · `Security` (measured) | n/a | provider error + reference C §4.2 · slot on `Security-link-error` | C "reach Microsoft" | L9 "Link in 41:12" C (COV-26) | Linked: `Security-linked` | H | H |
| Linked to another Richie account | `Security-link-error` | n/a | is the state | n/a | n/a | n/a | n/a | H | H |
| Canceled at provider | C §4.2 · slot on `Security-link-error` | n/a | n/a | n/a | n/a | n/a | n/a | H | H |
| Org approval needed | C §4.2 (wording differs from P3, COV-18) | n/a | n/a | n/a | n/a | n/a | n/a | H | H |
| Signed out while away | C §4.2 (E3 trigger unclear, COV-17) | n/a | n/a | n/a | n/a | n/a | n/a | H | H |
| Step-up to link | C §4.4 · AS `Stepup*` | AS | AS | AS | AS | AS | AS | AS | H |
| Unlink confirm | `Security-unlink` | "Unlinking…" C §4.3 | n/a | C "unlink Google" · `Security-remove-failed` | C | n/a | "Google unlinked." C · slot on `Security-linked` | H | H |

### 1.3 Account recovery (R1-R15)

| Page | Default | Loading / busy | Error: validation | Error: server | Error: offline | Error: rate limit | Empty / success | Mobile | Dark |
|---|---|---|---|---|---|---|---|---|---|
| R1 Can’t get the code? | `Recover` | passkey and provider busy C §3.1 · AS `Security-passkey-waiting`, `Signin-redirecting` | n/a | provider/passkey errors C | C | L6 countdown on methods C (WP3.5) | variants (passkey, no sign-in in progress) C · `Recover` | `Recover-mobile` | `Recover-dark` |
| R2 Which account | `Recover-account` | "Checking…" C | empty, malformed, no account, open request, wait, deleted C §3.2 (links in errors (!) COV-30) | C | C | L2 / L5 countdown C · `Signin-limited` (no support exit, COV-06) | -> R3; draft-expired alert C · `Signin-provider-canceled` | H | H |
| R3 New email | `Recover-email` | "Sending code…" C | empty, malformed, same, taken, L5 C | C | C | L1 C | -> R4 | H | H |
| R4 Code | `Recover-code` | "Checking code…" C | incomplete, wrong, locked, expired C · AS `Stepup-code-*` | send/check failed C | C | resend 1:00 drawn; locked C | -> R5; new-code message (conflict COV-04) | H | H |
| R5 Details | `Recover-details` | "Sending request…" C | name, devices, too long C | C | C | `Recover-limited` | -> R6; expired confirmation C | `Recover-details-mobile` | H |
| R6 Under review | `Recover-review` | "Canceling…" C | n/a | C | C | L7 -> E10 | requester confirm and owner view C · `Recover-cancel` (!) COV-33 | H | H |
| R7 Approved, on hold | `Recover-approved` | n/a | n/a | C (cancel) | C | L7 | "today" variant C | `Recover-approved-mobile` | H |
| R8 Completed | `Recover-completed` | n/a | n/a | n/a | n/a | L7 | is success | H | H |
| R9 Declined | `Recover-declined` | n/a | n/a | n/a | n/a | L7 | date-gated new request C | H | H |
| R10a / R10b Canceled | C §3.9 · `Recover-declined` (R10b removes a button (!) COV-33) | n/a | n/a | n/a | n/a | L7 | n/a | H | H |
| R11 Old-address email | `Email-recovery-alert` | n/a | n/a | n/a | n/a | n/a | n/a | H (< 600) | H |
| R15 Other 13 emails | C §4.1 · `Email-recovery-alert` | n/a | n/a | n/a | n/a | n/a | n/a | H | H |
| R12 Cancel from old address | `Recover-cancel` | "Canceling…" C | n/a | C | C | L7 | approved, already canceled/closed/completed variants C · `Recover-canceled`, `Recover-declined` | H | H |
| R13 Canceled, owner | `Recover-canceled` | n/a | n/a | n/a | n/a | n/a | is success | H | H |
| R14 Recently recovered | `Security-recovered` | AS | n/a | AS | AS | n/a | undone / expired C | H | H |

### 1.4 Google and Microsoft in sign-in (A1-A4, P1-P7)

| Page | Default | Loading / busy | Error: validation | Error: server | Error: offline | Error: rate limit | Empty / success | Mobile | Dark |
|---|---|---|---|---|---|---|---|---|---|
| A1 Sign in (provider states) | `Main` | `Signin-redirecting` (Google), Microsoft C | A1 field errors (identity canvas) | P3 `Signin-provider-error` / challenge 5xx C | C · `Signin-offline` (wording conflict COV-14) | L6 C · alert + `Signin-limited` | P2 canceled `Signin-provider-canceled` | `Main-mobile` | `Main-dark`, `Signin-provider-error-dark` |
| A4 Welcome back | `Signin-remembered-provider` | C · `Signin-redirecting` | n/a | C [P3] | C | C | -> return URL | H | H |
| A2 Create account | `Signup` | "Sending code…" C | first name, email, has account C §5.2 (link in error (!) COV-30) | C | C | L1, L2, L6 C | -> Verify (identity canvas) | `Signup-mobile` | H |
| A3 Finish creating | `Signup-finish` | "Creating account…" C | first name C | C | C | L6 C | -> return URL; code step C · `Provider-link-code` | `Signup-finish-mobile` | H |
| P4 No account | `Provider-no-account` | "Taking you to Google…" C, **no slot to report failure** (COV-10) | n/a | C | C | L6 | -> A3 | H | H |
| P5 Link provider | `Provider-link` | "Sending code…", "Waiting for your device…" C | n/a | C (slot) | C | L1 C | -> P6; sign-up wording C · `Provider-link` | `Provider-link-mobile` | H |
| P6 Link, enter code | `Provider-link-code` | "Linking Microsoft…" C | incomplete, wrong, locked, expired C · AS | C | C | resend countdown | linked -> return URL; new-code (conflict COV-04) | H | H |
| P7 No confirmed email | `Provider-no-email` | switch account busy, **no slot** (COV-10) | n/a | C | C | L6 | -> A1 | H | H |
| "Signed in, not linked" | C spec-sso §7.3 · `Provider-no-email` (not in IA, COV-11) | n/a | n/a | n/a | n/a | n/a | -> return URL | H | H |

## 2. Buildability: what an engineer would have to ask

The ten checklist items per page. A tick means the spec gives it with numbers or copy; otherwise a question and the finding that tracks it. Pages not listed are fully specified (E6, E8, E10, E11-E14, S1-S6, R3-R5, R7-R8, R13, R14, A2, A3, P4-P7 gave route, title, trigger, copy, targets, focus, live region, limits, reveals, emails and component mapping).

| Page | Open questions an engineer would have to ask |
|---|---|
| E1 | Does a GET (page load) reset the 20-minute clock, or only a successful POST? (COV-36) |
| E2 / E3 | Which settings pages hold drafts besides Profile, where does the "put back" status go on them, and where does focus land after restore? Who shows E2 in the Richie app? Does the link-callback session end set `signin_reason`? (COV-29, COV-17) |
| E4 | What is the busy state of Try again on the other-routes variant? Which Duende error codes map to each variant? (COV-34) |
| E5 | Which routes consume a token and which are reusable? (COV-24) |
| E7 | Link or `fetch` submit for provider buttons? Which offline wording? (COV-14) |
| E9 | Which string at 0:00 for R2 and A2: "You can try again now." or "You can continue now."? (COV-16) |
| E13 / E14 | How tall is the page with "Back to {client}"? (COV-13) |
| S1-S6 | Settings emails: which time format and zone? (COV-25) |
| S7-S10 | Which host runs the Google and Microsoft schemes and callbacks, and how many redirect URIs? (COV-08) What exact string and button for the "linked" email? (COV-09) Does the link-limit label fit the row? (COV-26) |
| R1 | Where does "Back to the code" go from P6, A5, the Help article? (COV-03, COV-12) What if the person has no passkey for Richie? (COV-31) Is there a support route? (COV-06) |
| R2 | What does a real owner do when a stranger’s request is open? (COV-05) Is the deletion date revealed? (COV-07) |
| R6 / R12 | Which token authorises Cancel on the status page? (COV-24) What happens at `changes_at` if the completion job fails? (COV-23) |
| Support form | Everything (COV-02). |
| A1 provider buttons | Form POST or link? What does the page say when the fetch fails offline? (COV-14) |
| P4, P7 | Where does a failed "Use a different account" press report? (COV-10) |
| "Signed in, not linked" | Is it a page? Where is it in the IA? (COV-11) |
| A3 | Where does Cancel go when the person came from sign-in? (COV-32) |

## 3. Findings

| ID | Severity | Page/board | Gap or contradiction | Evidence | Suggested resolution | Owner |
|---|---|---|---|---|---|---|
| COV-01 | major | R1 / Change email / `Security` (J7) | Dead end and three-way contradiction on J7 (lost mailbox, Google or Microsoft linked, no passkey). R1 sends the person to sign in with the provider, then to Change email. Change email needs step-up; step-up offers only a passkey or an email code (the lost mailbox). spec-recovery says a "provider-backed session" satisfies step-up; spec-settings Q-S1 and spec-sso OQ-2 say a provider sign-in is neither a step-up method nor "recent". spec-settings §1.1 and AS §4.4 say a sign-in under 10 minutes old needs no step-up, which would let J7 work for 10 minutes only. As written, a Google-only person cannot change their email. | spec-recovery §2.2 step 4; spec-settings §1.1, Q-S1; spec-sso OQ-2; spec.md §3.4 J7, §8 Q20; AS §4.4 "Trigger" | PM decides one rule. Recommended: a provider sign-in from R1 starts the 10-minute window (so Change email needs no step-up for 10 minutes), step-up still excludes providers afterwards, and R1 copy tells provider-only people to change their email straight away. Otherwise strike J7 and send them to the recovery request. Then align the three specs. | PM |
| COV-02 | major | Support form (E4, E5, R1 note, R6, R7, R9, R10, R12, R14, S6 email, step-up C) | The "Contact support" destination is never specified, yet about 12 pages and 6 emails point to it. An engineer cannot build it: URL, fields, topics list, how a signed-out person gives a reachable contact (the account mailbox is dead), how tokens (dispute, status) and the E4 reference attach, what it does with the topic. Inventory lists this as missing; spec-recovery Q-R10 gives a one-line default only. | inventory.md rows 170-172; spec-recovery Q-R10, §4.1 #6, §3.6, §3.9; spec-edge §4 E4, E5; spec-settings §3.4; board `#support` placeholders | Add a one-page support-form spec (fields, contact email required when signed out, topics, token and reference attachment, confirmation, reply promise) or an explicit "out of scope, owned by support" handoff with the minimum contract the pages rely on. | PM |
| COV-03 | major | P6 `Provider-link-code`, R1 `Recover` | P6 offers "Can’t get the code?" to R1, and R1 "Back to the code" always goes to Verify (`/signin/code`). From P6 that is the wrong page (the sign-in has no Verify state) and the return is lost. spec-sso §16 asks WP3.4 to return to "the code page that opened it"; spec-recovery §3.1 and §7.1 never do. R1 also offers Sign in with Microsoft, which from P6 loops back to P5. Part A §3.1 lists only A5, A6 and Help as R1 entries, not P6. | spec-sso §7.3, §16; spec-recovery §3.1, §7.1, §7.4; spec.md §3.1 R1; boards `Provider-link-code` -> `Recover`; `Recover` "Back to the code" -> `#verify` | WP3.4 adds an `from=link` variant of R1 (primary "Back to the code" returns to `/signin/external/link/code`; hide the provider that triggered it) and adds P6 to R1 entry points. Update Part A R1 row. | WP3.4 |
| COV-04 | major | R4 `Recover-code`, P6 `Provider-link-code`, S5 `Profile-email-code-mobile` | Contradiction on how long "New code sent. Use the code in the newest email." stays. spec-settings: until the first character is typed (a documented AS delta). spec-recovery R4 and spec-sso P6: for 5 s. Same string, same code component, three behaviours across the area. | spec-settings §0 table, §3.2; spec-recovery §3.4 "New code sent"; spec-sso §7.3 "New code sent" | PM rules once (recommended: until first character, per WP3.3, because the person leaves to read mail). Apply to R4, P6 and A3 code step. | PM |
| COV-05 | major | R2 `Recover-account` | Lockout for the real owner. If an attacker (or anyone) already has an open request for the account, R2 refuses with "This account already has a recovery request. We sent its status to the new email it named." The owner has lost their mailbox, so they never see R11, cannot cancel, and the error offers no support link, no wait time and no way to dispute. The open request can last 72 h hold plus review time. | spec-recovery §3.2 states table; spec.md §8 Q11; spec-recovery §11 Q-R1 | Add Contact support (topic "Recovery I didn’t ask for") to that error, and say how long a request can stay open. Consider letting a second request replace an open one that has had no support action. | WP3.4 |
| COV-06 | major | R1-R4 (`Recover`, `Recover-account`, `-email`, `-code`), R2 limited states | "Signed-out recovery must reach support" fails for the early steps. Contact support appears first at R5 limited and R6 onward. R1 has none (only footer Help, whose "Can’t sign in" article is undesigned and links back to R1). R2 errors for no account, deleted account, and the L2 or L5 countdown ("Continue tomorrow at 9:15 AM") give no support route; R3 and R4 give none when the new mailbox never receives a code. A shared-office IP hits L5 for 24 h with no exit. | spec-recovery §3.1-§3.5; spec.md §3.5 L5; board `Recover-account`, `Recover-email`, `Recover-code` (no support link) | Add a quiet "Contact support" note to R2, R3 and R4 limit and failure states, and to R1 below the recovery block, opening the form in a new tab. | WP3.4 |
| COV-07 | major | R2 `Recover-account` (reveals) | R2 reveals more than Sign in: "We deleted the Richie account for this email on December 5, so it can’t be recovered" tells any visitor that an account existed and when it was deleted, for any typed email. The spec states R2 reveals "no more than Sign in" plus open request or wait. The sample date is also after "today" (Nov 5). | spec-recovery §3.2 states table; spec.md §1.7 principle 5, §3.1 R2 Reveals | Use the same answer as A1 for accounts deleted over 30 days ago ("No Richie account uses this email"), or have PM accept the leak explicitly. Fix the sample date. | WP3.4 |
| COV-08 | major | S7-S10 vs A1/P5 (linking) | Ownership of linking is ambiguous. spec-settings has `POST /account/security/link/{scheme}` and `/account/security/link/callback` under the settings app with its own `Challenge`, `ExternalCookie` and `FindByLoginAsync`; spec-sso registers Google and Microsoft in the IdentityServer host with callback `/signin/external/callback`. Part A §3.2 says settings linking "uses its own callback". An engineer must ask: which host owns the external schemes, how many redirect URIs are registered at Google and Microsoft, how the BFF-side route writes a login into the identity user store, and how step-up `auth_time` is read. | spec-settings §9.2; spec-sso §13.2; spec.md §3.2 External login row | WP3.3 and WP3.5 write one paragraph: schemes live in the identity host; settings routes proxy to it (or settings is hosted in it). List the two redirect URIs per provider. | WP3.3 |
| COV-09 | major | Provider-link emails (S8 vs P6/P5) | Both specs claim "one template" for the "provider linked" security email but disagree: button "Check Sign-in and security" vs "Review Sign-in and security"; closing advice "sign out of any device you don’t recognize" vs "contact Richie support"; fine print differs; the SSO body says "linked today at 10:12 AM" (wrong when read later) while recovery §2.5 requires the date and time zone ("November 8 at 4:10 PM Pacific Time") and settings gives no zone. | spec-settings §5.2 row "Provider linked"; spec-sso §8.5; spec-recovery §2.5, §4 | One template owned by WP3.3: date, time and zone from the account setting; one button label; both pieces of advice. WP3.5 references it. | WP3.3 |
| COV-10 | major | P4 `Provider-no-account`, P7 `Provider-no-email` | Secondary actions on P4 and P7 ("Use a different Google/Microsoft account", busy "Taking you to Google…") start the challenge with `fetch`, so offline, network and 5xx failures are possible. spec-sso §10 says "copy (slot)", but §8.4 lists no live region for P4 and P7 and neither board has a `.ri-slot`. A failure line has nowhere to render, and adding a slot adds an element and height, breaking Part A §5.3. | spec-sso §7.1, §7.4, §8.4, §10; boards `Provider-no-account`, `Provider-no-email` (no `ri-slot`) | Add the reserved slot to P4 and P7 on the boards (WP3.5 redraw), or specify those two buttons as plain links with no busy state. | WP3.5 |
| COV-11 | major | "Signed in, not linked" (`/signin/external/not-linked`) | A page with its own route, title, copy and two buttons exists only as a copy-only paragraph inside P6. It is absent from Part A §3.1, the §5.2 board list and the §3.4 journeys, and it reuses `Provider-no-email` while dropping that board’s note and switch line (a different height). No board, no state matrix row, no focus rule, no exit if "Continue" target is lost. | spec-sso §7.3 end; spec.md §3.1 P-rows, §5.3 | Add it to Part A §3.1 as P8 with a matrix row, or draw it. Say what happens if the return URL is invalid. | WP3.5 |
| COV-12 | major | A5 / A6 (`Passkey-signin`, `Verify*` on the identity canvas) | The only entries into R1 from a live sign-in are deltas to eight Verify boards and the passkey boards on the identity canvas: a new link line, a moved "Check spam" sentence, a new centred line with the passkey link. Part A §5.3 lists them as copy-only, but they add an element, a focus target and a height to shipped, tested pages, and are not on this canvas (R1 "Back to the code" is a dead `#verify` anchor here). Nothing in this canvas proves the entry works. | spec.md §3.1 A5, A6, §5.3; spec-recovery §7.1, §7.2, §7.4; board `Recover` `#verify` | Hand the deltas to the identity canvas as a tracked change with redrawn Verify and Passkey-signin boards, or draw one `Verify-help` board here. | WP3.4 |
| COV-13 | major | E13 `Signedout`, E14 `Signedout-everywhere` | "Back to {client}" is classed copy-only ("Signedout + an outline button") but adds a button: a new element, focus target and about 56 px of height under the primary. This is the case Part A §5.3 rules out. The heights and the mascot/h1 position invariant were measured only without it. | spec.md §5.3 row "E13 Back to {client}"; spec-edge §4 E13, E14, §5 matrix | Draw `Signedout-client` (E13 with the second button) or relax §5.3 for this case and measure it in the rig. | WP3.2 |
| COV-14 | major | A1 provider buttons vs E7 vs R1 (offline copy and mechanism) | Same buttons, different failure copy and mechanism. spec-edge E7: "We couldn’t reach Google because you’re offline", provider buttons are links intercepted when offline. spec-sso §3.1: "We couldn’t sign you in because you’re offline", buttons are form submits using `fetch`. spec-recovery R1 (D-R14): "We couldn’t reach Google because you’re offline". spec-edge §13 notes the SF wrapping problem but the specs were not aligned. Two behaviours on A1 means two implementations. | spec-edge §4 E7 table and Behaviour; spec-sso §3.1, §11 D2, D12; spec-recovery §3.1 D-R14; spec.md §4.2 | Adopt spec-sso’s "sign you in" phrases on A1, A2, A4, R1 and E7 and its submit-button mechanism. Update spec-edge E7 and spec-recovery R1. | WP3.5 |
| COV-15 | minor | E1 / E5 / spec-sso callback table | Stale provider callback: B-1 settles it as E1b, but Part A §3.1 E5 row, spec-sso §2 row 1, OQ-11 and §16 still say E5. Engineers reading spec-sso will build the wrong page. | spec.md Part B B-1; spec-sso §2 row 1, OQ-11, §16; spec-edge D7 | Edit spec-sso (and Part A E5 entry points) to say E1b. | WP3.5 |
| COV-16 | minor | E9 / R2 / A2 / L6 strings | One rate bucket (L2, shared with L6), four phrasings: "This network tried to sign in 30 times in an hour." (E9), "This network has tried 30 emails in an hour." (R2), alert "Too many sign-in attempts from this network" (L6); 0:00 line "You can try again now." (E9 for A2, R2) vs "You can continue now." (R2). | spec-edge §4 E9 table; spec-recovery §3.2; spec-sso §3.5 | Take spec-edge E9 strings as canonical; WP3.4 and WP3.5 reference them. | WP3.4 |
| COV-17 | minor | E3 / S8 (session ends at the provider) | E3 shows only with `signin_reason=session_ended` (D8). spec-settings says a session ended during linking lands on E3 but its callback sends the person to Sign in with only `returnUrl` and `login_hint`, so E3 would not show and the slot message "Microsoft wasn’t linked because you were signed out." is the only cue. | spec-edge §4 E3 Behaviour, D8; spec-settings §4.2 row "Signed out while away", §9.2 Callback, §13 | State that the callback adds `signin_reason=session_ended`. | WP3.3 |
| COV-18 | minor | S8 org approval vs P3 org variant | Two wordings for the same Microsoft case: "Your organization has to approve Richie first." + "Ask your Microsoft 365 admin to allow Richie, or link a personal Microsoft account." (settings) vs "Your organization hasn’t approved Richie" + "Ask your Microsoft admin to allow Richie, or use your email or a passkey." (sign-in). Different contexts, but admin naming (365) differs. | spec-settings §4.2; spec-sso §3.3 | Pick "Microsoft admin" in both; keep context-specific second sentences. | WP3.3 |
| COV-19 | minor | Dark coverage (all areas) | Dark is drawn for 8 boards (Main, Signedout, Timeout, Error, Signin-offline, Signin-provider-error, Security, Recover). Every other error and limit state (E9, E10, E5, E11 failure, S3, S4, S5, R5 limited, P2-P7 pages) is harness-only. how-we-design §4 asks to draw dark for every error. Recorded departure (§5.3), but no check was run on the canvas itself. | spec.md §5.3 "Dark boards"; how-we-design §4 checklist | PM accepts the departure explicitly, or adds dark wrappers for `Limited`, `Signin-limited`, `Recover-limited`, `Security-load-error`. | PM |
| COV-20 | minor | Mobile coverage | No 390 board for 20+ state pages with the longest copy: `Error-link`, `Error-notfound`, `Offline`, `Limited`, `Signedout-prompt`, `Signedout-everywhere`, `Recover-account/-email/-code/-review/-completed/-declined/-cancel/-canceled`, `Provider-no-account/-no-email/-link-code`, `Signin-remembered-provider`, `Security-recovered`. Harness covers overflow but not copy wrapping judgments (for example `Recover-declined` two-line h1 at 390 noted in text only). | spec.md §5.2; spec-recovery §6 matrix; spec-edge §6 | PM confirms harness-only is accepted, or adds `Recover-declined-mobile` and `Signedout-prompt-mobile`. | PM |
| COV-21 | minor | Part A tables vs Part B departures | Part A §3.1, §3.4 and §5.2 still carry values the domain specs replaced: E11 "Cancel" (now Stay signed in, J13), R3 and R5 shared title, S6 "Sign in" button, R12 Keep -> R6, P4 title, E5 "HTTP 400/410" (410), E13 route without `logoutId`, board sizes. Part B says the domain spec wins, but an engineer starting at Part A gets the old behaviour. | spec.md Part B intro; spec-edge §10; spec-recovery §1.2; spec-settings §11; spec-sso §11 | Add a one-table "supersedes" list to Part A §3.1 or regenerate the table. | PM |
| COV-22 | minor | Canvas wiring (`Recover-*`, `Provider-link-code`) | Primary actions are not wired, so J8 cannot be clicked through: R2 Continue, R3 Send code, R4 Continue, R5 Send request, R6 Cancel request, R12 Cancel the request (should reach `Recover-canceled`, spec §7.4 omits it), `Provider-link-code` Link Microsoft (`#richie`). Only Back links and Keep the request are linked. | boards `Recover-account` (form action `#`), `Recover-email`, `Recover-code`, `Recover-details`, `Recover-cancel`, `Provider-link-code`; spec-recovery §7.4 | Wire forms to the next board; list the links in §7.4. | WP3.4 |
| COV-23 | minor | R7 -> R8 completion | No failure path for the completion job: the spec re-checks "still approved, account exists, new address still free", but says nothing if the check fails (account deleted during the hold, address taken despite the reservation), what the requester sees at `changes_at`, or which emails go. R7 reloads at `changes_at` + 5 s and would show R8 before the job has run if it is delayed. | spec-recovery §2.5, §9.2 Completion job, R7 states table | Specify failure (page shows R6-like "We’re finishing your request", support alerted) and make R8 depend on the stored status, not the clock. | WP3.4 |
| COV-24 | minor | R6 / R12 tokens | Token semantics unclear: "single purpose (status, cancel, dispute)" yet the requester’s Cancel on R6 uses `/recover/status/cancel?token=` (which token?), the status link must survive many opens while E5 says "used" tokens fail, and R6 "owner view" is opened by the cancel token. Engineers cannot tell which routes consume a token. | spec-recovery §2.4, §3.6, §9.2 Routes; spec-edge §4 E5 | Add a token table: type, route, reusable or one-shot, lifetime, what it authorises. | WP3.4 |
| COV-25 | minor | Email time and zone (settings, SSO, recovery) | Recovery emails name the account’s time zone; settings security emails show "November 5 at 9:31 AM" with no zone and SSO uses "today at 10:12 AM". Wrong when the email is read the next day. | spec-recovery §2.5; spec-settings §5.2; spec-sso §8.5 | One date-format rule for all security emails (absolute date, time, zone). | WP3.5 |
| COV-26 | minor | Unavailable buttons with long labels | New unavailable labels were not measured at 320 or in the 144 px row actions: "Link in 41:12", "Continue tomorrow at 9:15 AM", "Send request tomorrow at 9:15 AM", "Start a new request on November 12", "Sign in with Google in 41:12". spec §4.6 measures slot strings only. | spec-settings §4.1; spec-recovery §3.2, §3.5, §3.9; spec-sso §3.5 | Measure with the rig at 320 and 390 and record; set wrapping behaviour. | WP3.3 |
| COV-27 | minor | Part A §3.5 limits and lifetimes | The one rate-limit table lacks limits introduced later: Q-S4 (10 email-existence checks per account per hour), L9 present, 10 passkeys, draft 30 min, new-address confirmation 30 min, tokens 30 days after close, external cookie 20 min, remembered account 90 days, sign-out token. Engineers must gather them from four specs. | spec.md §3.5; spec-settings Q-S4; spec-recovery §2.4; spec-sso OQ-4, OQ-10 | Append a "limits and lifetimes" table to Part A §3.5. | PM |
| COV-28 | minor | L2 / L6 per-IP numbers | L2 and L6 share 30 per hour per IP and arriving at any provider callback counts, so an office network (the sample customer is a coffee roastery) throttles all staff at a sign-in rush; L5 allows 5 recovery requests per IP per 24 h. Numbers are marked "engineering confirms" but the product impact is unstated beyond E10’s shared-network note. | spec.md §3.5; spec-edge E10 note; spec-sso §2 row 6 | Engineering confirms numbers; consider counting failures only for provider sign-ins. | PM |
| COV-29 | minor | E2 draft restore | Only Profile’s name slot is named for "We put back what you typed". Which pages have page-level fields to restore (Preferences?), where the status goes elsewhere, and where focus lands after restore are unspecified. The Richie app (not settings) 401 case is left to "if it adopts the hint". | spec-edge §4 E2, §13, QE-10; spec.md §3.3 rule 6 | List the pages with drafts and the slot for each; say who owns E2 in the Richie app. | WP3.2 |
| COV-30 | minor | R2 `Recover-account`, A2 `Signup`, A3 `Signup-finish` | Copy-only error states contain a link: R2 wait ("Contact support"), A2 and A3 "Sign in instead". A link inside a field error read through `aria-describedby` is not announced as a link and adds a tab stop to a layout drawn without one; the A3 case is a link inside `role=alert`. | spec-recovery §3.2; spec-sso §5.2, §6.2; spec.md §5.3 | Move the link below the field (a note) or draw the state. | WP3.5 |
| COV-31 | minor | R1 passkey path | R1 "Sign in with a passkey" is discoverable; when the person has no passkey for Richie on any device, the failure copy is "The request timed out or you canceled it." There is no "no passkey found" line, so the person may retry forever. | spec-recovery §3.1 states table; AS §4.4 screen A failed copy | Reuse AS "…or this device has no passkey for Richie" wording. | WP3.4 |
| COV-32 | minor | A3 `Signup-finish` Cancel | Cancel always goes to A2, even when the person came from A1 via P4 or from the sign-in return URL, which drops them on Create account. Part A fixes it as A2. | spec.md §3.1 A3; spec-sso §6.1, §8.2 | Return to the start page recorded in the challenge (`start` item). | WP3.5 |
| COV-33 | minor | R6 owner view, R6 requester cancel, R10b | Copy-only states that remove rows or swap actions on a drawn layout (owner view drops the Account row; requester confirm drops From; R10b has no unavailable second button), so heights differ from `Recover-review`, `Recover-cancel` and `Recover-declined`. Small but §5.3 says no height change. | spec-recovery §3.6, §3.9, §3.10 | Accept and measure, or draw `Recover-review-owner`. | WP3.4 |
| COV-34 | minor | E4 other-routes variant | The "other identity route" E4 stacks Try again + Go to Richie with the reference line and note; the combination is not drawn (`Error` has one button, `Error-notfound` has no reference). Busy label for Try again is undefined (it reloads). | spec-edge §4 E4 table; §5 matrix | Draw or measure the combination; define busy state. | WP3.2 |
| COV-35 | minor | Footer Help, Privacy, Terms | Footer links and the Help article "Can’t sign in" (first link R1, the only support route from a typed-URL visit) are `#help` placeholders. The article’s content is not specified. | spec-recovery §7.3; spec.md §5.1 Links | State whether the article is in scope; list its minimum content. | PM |
| COV-36 | minor | E1 clock | The 20-minute sign-in clock is "since the last step on an identity page". Whether GETs (page loads, countdown reloads) reset it, or only successful POSTs, is not stated, and E1’s note says "A sign-in lasts 20 minutes". | spec.md §8 Q1; spec-edge §4 E1 Behaviour | Define: sliding on successful POST only; reword note if so. | WP3.2 |
| COV-37 | minor | Signed-in lost mailbox (step-up screen C) vs recovery request | Signed-in people with a lost mailbox and no passkey use AS screen C ("Contact support", topic "I can’t get my email code") while signed-out people use the recovery flow with a 72-hour hold, old-address alert and 2-day promise. Two routes to the same outcome with different safeguards and no reply-time on screen C. | inventory.md rows 167-172; spec-recovery §1, §11 | PM decides whether screen C links to R2 or support applies the same hold. | PM |
| COV-38 | minor | Recovery sample data | R2 deleted-account example says "deleted … on December 5" while today is November 5 (a future date). | spec-recovery §3.2 states table, header "today is Thursday, November 5, 2026" | Use "October 3" or similar. | WP3.4 |

## 4. Journeys walked

Part A §3.4 J1-J21 first, then extra paths from the domain specs. Pass = every step has a next step and a way out, and the specs agree. Board links were checked by script against the spec.

| # | Journey | Result | Notes |
|---|---|---|---|
| J1 | New person signs up with Google | pass | P4 and A3 both have a way out. A3 Cancel goes to A2 not the start page (COV-32) |
| J2 | First Microsoft sign-in links via code | pass | P5/P6 exits fine. `Main` Microsoft -> `Provider-link` (B-2) matches |
| J2b | J2 but the mailbox is lost (P6 "Can’t get the code?") | **fail** | R1 returns to Verify, offers Microsoft again (loop) (COV-03) |
| J3 | Create account with a provider whose email has an account | pass | P5 sign-up wording is copy-only |
| J4 | Microsoft gives no confirmed email | pass | P7 slot missing for the switch-account button (COV-10) |
| J5 | Sign-in times out on Verify | pass | `Timeout` -> `Main` link matches |
| J6 | Lost mailbox, has a passkey | pass | R1 passkey -> signed in -> Change email with step-up by passkey. No-passkey copy gap (COV-31) |
| J7 | Lost mailbox, Google linked, no passkey | **fail** | Step-up cannot be met (COV-01) |
| J8 | Lost mailbox, nothing else: R1-R14 | pass with defects | Every page has Back, Sign in, Cancel or Contact support from R5 on. Early steps have no support exit (COV-06). Canvas forms are unwired (COV-22). Completion failure path missing (COV-23) |
| J8b | Real owner finds a stranger’s open request at R2 | **fail** | No next step (COV-05) |
| J8c | Blocked at R2 by L2 or L5 ("Continue tomorrow") | **fail** | No support exit for up to 24 h (COV-06) |
| J9 | Owner gets the alert and cancels | pass | `Email-recovery-alert` -> `Recover-cancel` link matches; Keep -> `Recover-review` matches; Cancel -> `Recover-canceled` not wired (COV-22) |
| J10 | Owner misses the alert | pass | Completed email #6 and R12 "already changed" -> support. Support form unspecified (COV-02) |
| J11 | Signed out while saving in settings | pass | `Session-expired` Sign in again -> `Main`; E3 only with `signin_reason` |
| J12 | Sign out from settings | pass | `Signedout` links match |
| J13 | Untrusted logout link | pass | Part A says "Cancel"; domain spec "Stay signed in" (COV-21); board link `#richie` matches the spec |
| J14 | Bad client configuration | pass | E4 -> Go to Richie / reference -> support |
| J15 | Offline | pass | E7 and E8 exits; provider offline wording conflicts (COV-14) |
| J16 | Throttled | pass | `Limited` has a countdown button and footer Help |
| J17 | Link Microsoft in settings | pass | Cancel at every step; host ambiguity (COV-08) |
| J18 | Unlink Google | pass | |
| J19 | Remembered Google account | pass | `Signin-remembered-provider` links to `Main` for Not you? and Sign in another way, as spec |
| J20 | Expired recovery link | pass | `Error-link` -> `Recover-account` and `Main` match |
| J21 | Deleted account signs in with a provider | pass (out of area) | Handed to AS `Signin-deleted` |
| J22 | Stale provider callback | pass after B-1 | spec-sso still says E5 (COV-15) |
| J23 | Third-party sign-out and "Back to {client}" | pass | Layout not drawn (COV-13) |
| J24 | Requester cancels own request | pass | R6 -> confirm -> R10b -> Start a new request; token unclear (COV-24) |
| J25 | Declined, wait, new request | pass | R9 date-gated button; R2 accepts after the date |
| J26 | Signed-in lost mailbox, no passkey (step-up screen C) | pass, weak | Contact support only; different safeguards from recovery (COV-37) |
| J27 | Link attempt after session ended at the provider | pass | Sign in -> Security with slot message (COV-17) |
| J28 | Lost mailbox, Microsoft/Google not linked, R1 -> provider -> P4 | pass | Offers "Create an account", which a lost-mailbox owner could take as recovery; note text on P4 is the only guard |

Loops found: P6 -> R1 -> Microsoft -> P5 -> P6 (not a dead end, a loop; COV-03). Dead ends found: J7, J8b, J8c.

## 5. Contradictions between specs and boards

Settled already by a Part B ruling (no action beyond editing text): stale provider callback (B-1, COV-15); Main Microsoft link (B-2). Everything below is not settled.

| Topic | Spec A says | Spec B says | Id |
|---|---|---|---|
| Step-up after provider sign-in | spec-recovery §2.2: "provider-backed session" meets step-up | spec-settings Q-S1, spec-sso OQ-2: it does not | COV-01 |
| "New code sent" lifetime | spec-settings: until first character | spec-recovery R4, spec-sso P6: 5 s | COV-04 |
| Provider offline copy and mechanism | spec-edge E7: "reach Google", links | spec-sso §3.1: "sign you in", POST buttons; spec-recovery R1: "reach Google" | COV-14 |
| Linking host and callback | spec-settings §9.2: settings app | spec-sso §13.2 and Part A §3.2: identity host | COV-08 |
| Link-email template | spec-settings §5.2 | spec-sso §8.5 ("one template") | COV-09 |
| R1 "Back to the code" target | spec-recovery: Verify only | spec-sso §16: the page that opened it | COV-03 |
| L2/L6 strings and 0:00 text | spec-edge E9 | spec-recovery §3.2, spec-sso §3.5 | COV-16 |
| Org-approval wording | spec-settings §4.2 | spec-sso §3.3 | COV-18 |
| Time format in emails | spec-recovery §2.5 | spec-settings §5.2, spec-sso §8.5 | COV-25 |
| Cancel on sign-out prompt | Part A J13, §3.1 | spec-edge D2 (settled by "domain spec wins") | COV-21 |

Numbers cross-checked and consistent: 20-minute sign-in (Q1, E1, spec-sso OQ-10); 30-minute draft (E2, R2-R5, new-address confirmation); 72 h hold from approval; 7-day wait after decline or owner cancel; 30-day undo (E5, R12, completed email, S6); 7-day recently recovered; 5 codes per hour; 30 lookups per hour; 5 link attempts per hour; 10 passkeys; 8-character code, 4 tries, 5 minutes. Dates in the recovery samples add up (Nov 5 + 3 = Nov 8, + 30 = Dec 8, + 7 = Nov 15; Nov 6 + 7 = Nov 13).

## 6. Open questions

### 6.1 Block an engineer (no usable default, or the stated default conflicts)

| # | Question | Why it blocks | Finding |
|---|---|---|---|
| 1 | How does a provider-only person (no passkey, mailbox lost) pass step-up to change email? | J7 cannot be built; three specs differ | COV-01 |
| 2 | What is the support form (URL, fields, topics, contact for signed-out people, token and reference intake)? | About 12 pages and 6 emails depend on it | COV-02 |
| 3 | Which host owns the Google and Microsoft schemes, callbacks and redirect URIs for linking? | Two callbacks, one user store | COV-08 |
| 4 | How long does "New code sent…" stay? | Same component, three behaviours | COV-04 |
| 5 | What does a real owner do when a stranger’s request is open at R2? | Lockout | COV-05 |
| 6 | Where does R1 "Back to the code" go from P6 and other entries? | Wrong return | COV-03 |
| 7 | Does R2 reveal that an account was deleted, and when? | Reveal beyond A1 | COV-07 |
| 8 | Is "Signed in, not linked" a page in the IA? P4/P7 slot? | Missing state homes | COV-10, COV-11 |
| 9 | One security email for provider linking: which text and button? | Two templates | COV-09 |
| 10 | Provider buttons: link or POST; which offline wording? | Two implementations | COV-14 |

### 6.2 Open but with a stated default (engineer can proceed)

| Source | Questions and defaults |
|---|---|
| Part A §8 | Q1 20 minutes; Q2 14 days idle, 90 max; Q3 labels (decided P-1); Q4 custom Google button; Q5 confirmed email rules; Q6 never auto-link; Q7 Google sign-up skips the code; Q8 hold starts at approval; Q9 "2 business days"; Q10 support checks R5 details; Q11 refuse second request; Q12 7-day wait; Q13 undo 30 days; Q14 sign out all, keep passkeys and apps; Q15 gateway thresholds; Q16 service worker; Q17 sign out of every device; Q18 ClientUri; Q19 no passkey offer after provider; Q20 step-up to link and unlink; Q21 link a different email; Q22 provider email change ignored; Q23 any Microsoft tenant; Q24 recovery allowed during deletion; Q25 company SSO later |
| spec-edge | QE-1 pending sign-in keeps method; QE-2 no reason on E2; QE-3 activity entry for sign out everywhere; QE-4 third-party grants kept; QE-5 resolved by spec-recovery (R13); QE-6 resolved (30 days); QE-7 JSON 429 for fetch; QE-8 first offline visit unsupported; QE-9 client named from store; QE-10 draft scope; QE-11 L1 edit clears countdown |
| spec-settings | Q-S1 no provider step-up (conflicts with J7); Q-S2 no masked account; Q-S3 unlink keeps devices; Q-S4 10 checks per hour; Q-S5 duplicate names allowed; Q-S6 no email to new address; Q-S7 AAGUID names; Q-S8 old address unknown; Q-S9 admin wording; Q-S10 renames logged |
| spec-recovery | Q-R1 no alert on sign-in during request; Q-R2 answers kept 30 days; Q-R3 browser and city in old-address email; Q-R4 account time zone; Q-R5 one decline reason; Q-R6 support may ask for more; Q-R7 second agent; Q-R8 new address reserved; Q-R9 no A1 link to R1; Q-R10 support form contact (default given, form unspecified) |
| spec-sso | OQ-1 unconfirmed Microsoft email never routes to P5; OQ-2 fresh provider sign-in is not recent (conflicts with J7); OQ-3 use provider photo; OQ-4 remembered 90 days; OQ-5 User.Read each time; OQ-6 "Sign up with Microsoft" brand check; OQ-7 sign in silently if linked; OQ-8 one per provider; OQ-9 10 s callback timeout; OQ-10 20-minute external cookie; OQ-11 resolved by B-1 (E1b) |
| Numbers engineering confirms | L1-L9, gateway 429 (COV-27, COV-28 ask for a consolidated table and a look at office networks) |
