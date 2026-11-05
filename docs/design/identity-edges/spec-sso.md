# Richie identity edges, Part E: Google and Microsoft in sign-in (WP3.5)

**Revision 2, 2026-11-05** (Phase 4 fix round: §18 logs every finding; Part B rulings B-1 and B-6 to B-33 are applied; the Phase 5 revision round is written into the sections and listed at the end). Domain spec for WP3.5 of the identity edges workstream. It adds to [`spec.md`](spec.md) (Part A, cited as **A §**), the foundation; every departure is listed in §11 with its reason. **Precedence (one order for all five files, ruling B-37):** Part B rulings in `spec.md`, then the domain specs’ page tables and recorded departures, then Part A (Revision 2 synced it to both). A board that disagrees with its spec is a bug in the board. Settings rules are [`../account-settings/spec.md`](../account-settings/spec.md) Revision 5 (cited as **AS §**).

**Summary.** Google and Microsoft join Sign in as two more outline method buttons under "or", after the email form and the passkey, and join Create account as "Sign up with Google" and "Sign up with Microsoft". Everything that can happen after you leave for the provider comes back to the same frame: a busy button while we hand you over (P1), an info alert when you canceled (P2), a destructive alert with a reference when it failed (P3), and five short pages for the outcomes that need a decision: finish creating your account (A3), no account for this provider account (P4), link it to the account that already uses this email (P5, then a code on P6), and the provider didn't give us a confirmed email (P7); one more (P8) says you're signed in but the link couldn't be written. A remembered device whose last sign-in was Google opens on "Welcome back, Jamie" (A4). **A provider is never linked on its email alone:** the Richie account is proven first with an email code or a passkey (P5, P6), and links are stored by the provider's subject, never by email. No page reveals more than Sign in already does.

**Binding inputs:** D1 (Google and Microsoft only, linked to the personal account, room for company SSO later, A §0), PM decisions P-1 to P-3 (A §9: labels, our own outline button with Google's unaltered G, the G as an `<img>`), A §8 Q5 (when an email counts as confirmed), Q6 (never auto-link), Q7 (Google sign-up skips the code), Q19 (no passkey offer after a provider sign-up).

---

## Contents

1. [Boards](#1-boards)
2. [The external-login callback: one decision table](#2-the-external-login-callback-one-decision-table)
3. [Sign in states (A1: P1, P2, P3, limits, offline)](#3-sign-in-states-a1)
4. [Welcome back with Google (A4)](#4-welcome-back-with-google-a4)
5. [Create account (A2)](#5-create-account-a2)
6. [Finish creating your account (A3)](#6-finish-creating-your-account-a3)
7. [Pages after the provider (P4, P5, P6, P7)](#7-pages-after-the-provider)
8. [Cross-cutting rules: what we read, return URL, remembered account, focus, titles, emails and activity](#8-cross-cutting-rules)
9. [CSS (WP3.5 section of `edges.css`)](#9-css)
10. [State matrix](#10-state-matrix)
11. [Departures from Part A and the reference boards](#11-departures)
12. [Company SSO later](#12-company-sso-later)
13. [Handoff mapping](#13-handoff-mapping)
14. [Open questions, with defaults](#14-open-questions-with-defaults)
15. [Test results](#15-test-results)
16. [Links other WPs need](#16-links-other-wps-need)
17. [Changelog](#17-changelog)
18. [Fix log (Phase 4)](#18-fix-log-phase-4)

---

## 1. Boards

The 14 boards of A §5.2 and `Provider-not-linked` (P8, B-33) exist in `.design/edges/project/`, at the sizes `canvas.json` plans (no size change needed, §15). Kinds per A §5.1. Full copies (F) are generated from `Main.dc.html` by literal replacement, so they re-copy cleanly if `Main` changes.

| File | Board title | Size | Kind | State drawn | Measured page height at 1440 |
|---|---|---|---|---|---|
| `Signin-redirecting` | Sign in, taking you to Google | 1440×900 | F of `Main` | P1: Google button busy, focus drawn on it (the sign-in flow's focus board) | 900 |
| `Signin-provider-canceled` | Sign in, Google sign-in canceled | 1440×1000 | F | P2: info alert (the container takes focus, B-38), nothing else changed | 954 |
| `Signin-provider-error` | Sign in, Microsoft sign-in failed | 1440×1000 | F | P3: destructive alert with the reference and **Copy** (the container takes focus, described by the description and the reference, B-38) | 994 |
| `Signin-provider-error-dark` | Sign in, Microsoft sign-in failed, dark | 1440×1000 | W (dark) | P3 dark | 994 |
| `Signin-remembered-provider` | Sign in, welcome back with Google | 1440×900 | B | A4 | 900 |
| `Signup` | Create account, desktop | 1440×1000 | B | A2 with the two provider buttons | 974 |
| `Signup-mobile` | Create account, mobile | 390×844 | W | A2 at 390 (page 1058 tall, scrolls as on a phone) | — |
| `Signup-finish` | Create account, finish with Google | 1440×900 | B | A3, Priya's names and photo from Google | 900 |
| `Signup-finish-mobile` | Create account, finish with Google, mobile | 390×844 | W | A3 at 390 (page 970 tall) | — |
| `Provider-no-account` | Google account with no Richie account | 1440×900 | B | P4: **Sign up with Google** (method button, straight to A3), two plain links | 900 |
| `Provider-link` | Link Microsoft to your Richie account | 1440×900 | B | P5 | 900 |
| `Provider-link-mobile` | Link Microsoft to your Richie account, mobile | 390×844 | W | P5 at 390 | — |
| `Provider-link-code` | Link Microsoft, enter the code | 1440×900 | B | P6, focus drawn on the active slot; lede "…to link Microsoft", alternatives in one switch line below the commit (B-39) | 900 |
| `Provider-no-email` | Microsoft didn’t share a confirmed email | 1440×900 | B | P7, exclamation badge | 900 |
| `Provider-not-linked` (new, Phase 4) | Signed in, Microsoft not linked | 1440×900 | B | P8: exclamation badge, **Continue**, **Go to Sign-in and security**; no note or switch line, so it is shorter than `Provider-no-email` | 900 |

**Canvas walk** (links between boards): `Main` → Google → `Signin-redirecting`, Microsoft → `Provider-link` (B-2, the PM's change, carried by the full copies); `Signup` → Google → `Signup-finish`, Microsoft → `Provider-link`; `Signin-remembered-provider` → **Sign in another way** → `Main` (**Not you?** is now drawn as its form, whose action is `Main`); `Provider-no-account` → **Sign up with Google** → `Signup-finish`, **Sign in another way** → `Main`; `Provider-not-linked` → **Go to Sign-in and security** → `Security`; `Provider-link` → **Send code** (form action) → `Provider-link-code`, **Cancel** → `Main`; `Provider-link-code` → **Can’t get the code?** → `Recover` (WP3.4), **Cancel** → `Main`; `Provider-no-email` → `Main`, **Create an account** → `Signup`; `Signup-finish` → **Cancel** → `Signup` (the drawn journey started on A2). Provider redirects are `#google` / `#microsoft` (out of area). WP3.5 has not changed `Main`.

**Sample data** is A §4.12: Jamie Rivera (`jamie.rivera@northwind.coffee`, Google `jamie.rivera@gmail.com` linked, Microsoft work account of the same address being linked); Priya Natarajan (`priya.natarajan@gmail.com`, new); reference `9F2C-41AB-7D03`. Photos are illustrated stand-ins (Jamie's from AS §10.2; Priya's drawn the same way, no real person).

---

## 2. The external-login callback: one decision table

`GET /signin/external/callback` is a server endpoint with no UI. It reads the external cookie, then applies the first row that matches. **Mode** is `signin` (started on A1 or A4) or `signup` (started on A2), carried in the challenge's `AuthenticationProperties.Items`. **Subject** is the provider's stable key: Google `sub`; Microsoft `tid` + `oid`. **Confirmed** follows A §8 Q5: Google `email_verified = true`; Microsoft `xms_edov = true` or the consumer tenant (a personal Microsoft account's own address). Since June 2023 Microsoft omits an unverified `email` from multi-tenant id_tokens by default, so "no email" is the common Microsoft case and "unconfirmed" the rare one.

| # | Condition | Outcome | Page |
|---|---|---|---|
| 1 | The external cookie can't be read: `state` or correlation mismatch, expired correlation cookie, callback opened in another browser | Nothing signed in | **E1b** "Your sign-in timed out" with "Start again and choose Google" (WP3.2, B-1; not E5) |
| 2 | The sign-in request behind `returnUrl` expired (20 minutes, sliding with activity, B-29) | Nothing signed in | E1b (WP3.2) |
| 3 | `error=access_denied` (Google and Microsoft: the person declined or canceled; Microsoft `AADSTS65004`, `error_subcode=cancel`) | Nothing shared, nothing created | **P2** alert on the page it started from (A1, A2 or A4) |
| 4 | Microsoft says the organization must approve Richie (`AADSTS65001`, `90094`, `90095`) | Nothing shared | **P3**, organization variant |
| 5 | Any other provider error, failed code exchange, invalid id_token, missing subject, or no answer from the provider within 10 s | Reference logged with `error`, `error_description`, the `AADSTS` code and the trace ID | **P3** |
| 6 | Provider sign-ins and account creations from this IP over L6 (30 per hour, shared with L2) | Nothing created | Start page with the L6 alert (§3.5) |
| 7 | Subject is linked to account X, X active | Signed in to X (both modes: never a second account) | Return URL |
| 7a | Subject linked to X, X scheduled for deletion (days 0–30) | Signed in | `Deletion-pending` (AS §4.8), then the return URL |
| 8 | Not linked; email confirmed; it matches an active or pending account | Nothing linked yet | **P5** (sign-in wording, or the sign-up wording in `signup` mode) |
| 8a | Not linked; email confirmed; it matches only an account deleted in the last 30 days | Nothing linked | `signin`: `Signin-deleted` (AS §4.8, J21). `signup`: **A3** (a new account; AS: a live account wins) |
| 9 | Not linked; email confirmed; no account uses it | Nothing created yet | `signin`: **P4**. `signup`: **A3** |
| 10 | Not linked; no email claim | Nothing created | **P7** (sign-in wording, or the sign-up variant) |
| 11 | Not linked; email present but not confirmed | Nothing created | `signin`: **P7**. `signup`: **A3** with the code step (§6.3) |

**Rules behind the table.**

- **Linking is by subject, never by email** (A §3.2). Row 8 only *routes* to P5; the link is written after the email code (P6) or a passkey proves the Richie account. The provider's later email changes don't matter (A §8 Q22).
- **Matching** compares the provider's email, trimmed and lowercased, with the account's current email. No Gmail dot or plus folding.
- **One account per provider** per Richie account (A §5.2 settings rows). If proof succeeds but the account already has a different Google (or Microsoft) account linked, we sign the person in and don't link: **P8** (§7.5, board `Provider-not-linked`).
- **Unconfirmed emails never route to an existing account.** In `signup` mode an unconfirmed address that turns out to have an account stops at A3's existence check (§6.3) and sends the person to sign in with their email, then link in settings (J4). It is never offered P5. (See OQ-1 for the alternative.)
- After every outcome the external cookie is cleared, except while A3, P4, P5 or P6 still needs it (it then lives as long as the sign-in request, 20 minutes, OQ-10).

---

## 3. Sign in states (A1)

Route `/signin?returnUrl=…`; document title **"Sign in to Richie"** in every state below (B-13: the `h1` already names Richie; the alerts don't change it). `Main` (WP3.1) is the base: its copy is unchanged here. **Reveals:** as A1 (A §3.1). **Limits:** L1, L2, L6.

### 3.1 P1: taking you to Google (board `Signin-redirecting`)

| Element | Copy and rule |
|---|---|
| Google button, busy | Spinner (suiss `Spinner`, `aria-hidden`, no role) in the 20 px mark box, label **"Taking you to Google…"**, `aria-disabled="true"`, `data-busy`. It keeps focus. Never `disabled` |
| Microsoft button, busy (copy) | **"Taking you to Microsoft…"** |
| Email form, passkey and the other provider | Unchanged and enabled |
| Status slot | The polite paragraph receives the busy text once, visually hidden: `<span class="ra-sr">Taking you to Google…</span>` (B-28; drawn on the board). Nothing visible |

**Behaviour.** This is **the one mechanism for every provider button on a sign-in or sign-up surface** (A1, A2, A4, R1 and E7; COV-14): spec-edge E7 and spec-recovery R1 point here for both the mechanism and the copy. Settings Link has its own route and verb (spec-settings §4.2, B-6).

1. The method button is a submit button in a small form: `POST /signin/external/challenge` with `scheme`, `returnUrl` and the antiforgery token (a POST stops another site from starting a sign-in in your browser). Without JavaScript the server answers `302` to the provider, so it always works.
2. With JavaScript, pressing it sets the busy state at once and posts the same form with `fetch`; the server answers `200` with the provider's authorize URL (built server-side, with its state, nonce and correlation cookie), and the page calls `location.assign(url)`. That makes all three failure patterns real, in the slot's `p[role=alert]`, with focus kept on the button, which returns to idle:

   | Cause | Copy (phrase **sign you in**; on A2 **sign you up**) |
   |---|---|
   | `navigator.onLine` is false | **We couldn’t sign you in because you’re offline.** |
   | The `fetch` fails or takes over 10 s | **We couldn’t sign you in. Check your connection.** |
   | The challenge answers 5xx | **We couldn’t sign you in. Try again in a moment.** |

   **B-6:** the same three lines for the passkey button too (a failed passkey verification on our server), so a sign-in surface has one failure phrase whichever method was pressed; "reach" is retired. The WebAuthn prompt itself closing stays **The request timed out or you canceled it.** (A §2.1). "We couldn’t reach Microsoft. Check your connection." had also measured 2 lines at 358 in SF (A §4.6). The line appears while focus is on the button that was pressed, so the context is never lost; it mirrors E11's "We couldn’t sign you out."
3. If the navigation to the provider itself fails after `location.assign`, the identity service worker answers with **You’re offline** (E8, WP3.2); errors at the provider come back through the callback (P2, P3).
4. Pressing another method while one is busy starts that one; the first returns to idle.
5. **Back from the provider page** restores this page from the back-forward cache: on `pageshow` with `persisted`, every method button returns to idle and the slot empties. Without this, Back shows a button stuck at "Taking you to Google…".
6. The press doesn't count against L6; arriving at the callback does.

**Rationale.** The busy state is the button itself, in place: the person sees which way they chose and that we're moving, and nothing else on the page shifts (spinner and mark share one box, §9). No slot text, because the page is about to be replaced and a second copy of "Taking you to Google…" under **Send code** would sit next to the wrong control.

### 3.2 P2: canceled at the provider (board `Signin-provider-canceled`)

A suiss `Alert` (default variant, `Info` icon) as the first child of `.ri-body`, 24 above the form (A §4.10). No live `role`. **Focus moves to the alert container** (B-38): `role="group"`, `tabindex="-1"`, `aria-labelledby="alert-title"`, `aria-describedby="alert-desc"`, no ring, so the description (what to do next) is read with the title.

| Element | Copy |
|---|---|
| Alert title (`id="alert-title"`; the container it names is focused on arrival) | **You canceled signing in with Google** |
| Alert description (`id="alert-desc"`) | **Google didn’t share anything with Richie. You can try again or sign in another way.** |
| Microsoft (copy) | **You canceled signing in with Microsoft** / **Microsoft didn’t share anything with Richie. You can try again or sign in another way.** |
| On A2, sign-up wording (copy) | **You canceled signing up with Google** / **Google didn’t share anything with Richie. You can try again or create an account with your email.** |
| On A4 (copy) | The A1 wording, on A4's body above **Sign in with Google** |
| Everything else | `Main` unchanged: the email field is empty (or holds the `login_hint`), every method enabled |

**Behaviour.** Shown once: the callback sets a one-time server flag (TempData) and redirects to `/signin?returnUrl=…`; the URL carries no error, so a reload or a bookmark shows plain Sign in. No dismiss button: the page's own buttons are the actions. The alert is not live; focus on the container announces its title and description once (B-38).

**Rationale.** People cancel at a provider's consent screen because they hesitated to share. The one thing worth saying is that nothing was shared, then hand back every way in. Info, not destructive: nothing failed.

### 3.3 P3: provider error (board `Signin-provider-error`, dark `Signin-provider-error-dark`)

A suiss `Alert`, `destructive` variant (`WarningCircle` in Alarm, the card stays Mist), same placement and focus as P2, with the reference as a third row. The focused container is described by the description **and the reference** (`aria-describedby="alert-desc alert-ref"`, the reference paragraph carrying `id="alert-ref"`), so a screen-reader user hears the reference without hunting for it (B-38).

| Element | Copy |
|---|---|
| Alert title | **We couldn’t sign you in with Microsoft** |
| Description | **The sign-in didn’t finish. Try again in a moment, or use your email or a passkey.** |
| Reference row | **Reference 9F2C-41AB-7D03** (tabular, Ink 600) and a plain `sm` **Copy** button, `aria-label="Copy reference"` (A §4.5) |
| Copy feedback (A11Y-12) | **At the button:** its label reads **Copied** for 2 s, then **Copy** again (the button ends its row, so nothing moves). **For screen readers:** "Reference copied." is written once into the page slot's polite paragraph, visually hidden (B-28's method), so there is still one polite region and nothing appears far below under **Send code**, out of a magnifier's view. Clipboard unavailable: the code text is selected (its highlight is right there), the label reads **Press ⌘C** (**Press Ctrl+C** off Apple) for 4 s, and the sr text is "Press Command-C to copy." |
| Google (copy) | **We couldn’t sign you in with Google** / same description |
| On A2 (copy) | **We couldn’t sign you up with Google** / **The sign-up didn’t finish. Try again in a moment, or create an account with your email.** |
| Organization must approve Richie (Microsoft, copy, B-9) | **Your organization hasn’t approved Richie** / **Ask your IT admin to allow Richie, or use your email or a passkey.** + the reference. On A2: "…or create an account with your email." |
| Help | The footer **Help** carries the reference into the support form's topic (A §4.5) |

**Rationale.** We can't explain a provider failure, so it gets the E4 treatment inside Sign in: one plain sentence, a reference support can find, and the other ways in right below. Keeping the person on Sign in (not an error page) is the point of A §1.7 principle 1: their sign-in request is still alive. The organization variant exists because a Richie customer's work tenant that blocks user consent is a real, repeatable case with a fix the person can name.

### 3.4 Offline on Sign in (copy; layout `Signin-offline`, WP3.2)

| Situation | Copy | Where |
|---|---|---|
| Browser goes offline while idle | **You’re offline. We’ll keep what you typed.** (WP3.2, E7) | slot, `role=status` |
| Press the passkey, Google or Microsoft button offline (B-6) | **We couldn’t sign you in because you’re offline.** | slot, `role=alert` |
| The challenge request fails or times out | **We couldn’t sign you in. Check your connection.** | slot, `role=alert` |
| The challenge answers 5xx | **We couldn’t sign you in. Try again in a moment.** | slot, `role=alert` |
| Back online | **You’re back online.** for 5 s (WP3.2) | slot |
| Navigation to the provider fails after the hand-over | E8 page (WP3.2) | — |

### 3.5 Too many provider sign-ins from this network, L6 (copy; layout: the P2 alert on `Signin-provider-canceled` + the countdown buttons on `Signin-limited`)

| Element | Copy and rule |
|---|---|
| Alert (default, `Info`) title | **Too many attempts** (B-11). The alert container takes focus on arrival, labelled by the title and described by the description (B-38, as P2) |
| Description (B-34) | **This network has made 30 attempts in an hour. Try again in 42 minutes, or sign in with a passkey.** (B-34’s per-network line, then the wait in whole minutes, rounded up, from `Retry-After`; over an hour, "Try again at 2:30 PM", B-15). Only failed provider sign-ins count (B-23); the line doesn’t say "failed", as B-34 rules |
| Send code | **Send code in 41:12** (unavailable: `aria-disabled`, outline look, Slate, tabular; L6 shares L2's bucket) |
| Google, Microsoft | **Sign in with Google in 41:12**, **Sign in with Microsoft in 41:12** (unavailable, marks kept; one line at 320, measured) |
| Accessible names (B-27) | The digits are `aria-hidden`; each button has a fixed sr suffix with the clock time: "Send code, available at 4:53 PM", "Sign in with Google, available at 4:53 PM". Nothing ticks inside a name |
| Passkey | Enabled (a passkey sign-in is not a lookup) |
| At 0:00 | Buttons return; slot polite **"You can try again now."** (once) |
| Same on A2, A3, A4 | The alert with the page's own commit counting down (**Create account in 41:12**, "Create account, available at 4:53 PM") |

---

## 4. Welcome back with Google (A4)

Board `Signin-remembered-provider`. Route `/signin` when this device remembers an account whose last method was Google or Microsoft, and that provider is still linked (§8.3). Document title **"Welcome back – Richie"** (no name in titles). **Reveals:** the remembered name, photo and email, to this device only (as identity's `Passkey-signin`). **Limit:** L6.

| Element | Copy and rule |
|---|---|
| `h1` | **Welcome back, Jamie** (`translate="no"` on the name) |
| Lede | **Last time on this device, you signed in with Google.** (Microsoft: **…with Microsoft.**) |
| Account chip | Jamie's Richie photo (initials when there is none) and **jamie.rivera@northwind.coffee**, never truncated: when it can’t share one line with **Not you?** it wraps after the @ ("jamie.rivera@" / "northwind.coffee"), at every width (P-B; `.ri-head .ra-chip`, edges.css §9). Measured on the board: one line at 1440, two at 390, 360 and 320 |
| Chip action | **Not you?**, plain `sm` **submit button in its own small form** (`POST /signin/forget` with the antiforgery token; drawn that way, A11Y-17), `aria-label="Not you? Forget this account on this device"` |
| Method button | **Sign in with Google** (outline `.ri-method`, the G). Busy **Taking you to Google…** |
| Slot | Empty; a failed press uses §3.1's three lines (**We couldn’t sign you in because you’re offline.** / **…Check your connection.** / **…Try again in a moment.**) |
| Way out | **Sign in another way** (plain, block) |
| Canceled / error / L6 | P2, P3, L6 alerts on this page (copy) |

**Behaviour.**

- **Sign in with Google** starts the challenge with `login_hint=` the linked Google address, so Google preselects it. Whatever Google returns goes through §2: a different Google account linked to another Richie account signs in to *that* account (the subject wins) and becomes the remembered one.
- **Sign in another way** → A1 with the remembered Richie email filled; the device still remembers the account.
- **Not you?** → `POST /signin/forget` deletes the remembered-account cookie (nothing is sent to the account) → A1, empty. It changes state, so it is never a link (A11Y-17).
- No passkey button (A §7.4 default): **Sign in another way** reaches it on A1 in one step.

**Focus:** the `h1` on arrival (no field on the page).

**Rationale.** This is identity's passkey welcome-back page with Google in the passkey's place: the same chip, the same **Not you?**. The only primary is the Google button, drawn as the outline method button, not filled: method buttons are never filled (A §7.4), and Google asks for its G on a white, dark or neutral ground, which a blue fill isn't. One clear action and one quiet way out is enough hierarchy; adding a filled button only to satisfy "one filled primary" would create a second, competing action. (Departure 6.)

---

## 5. Create account (A2)

Board `Signup` (mobile `Signup-mobile`). Route `/signup?returnUrl=…` (also IdentityServer's `CreateAccountUrl`, so `prompt=create` lands here, §13). Document title **"Create your Richie account"** (B-13); **"Error: Create your Richie account"** after a failed submit. **Reveals:** whether an email has an account ("This email has a Richie account"), as A §3.1. **Limits:** L1, L2, L6.

### 5.1 Copy

| Element | Copy and rule |
|---|---|
| `h1` | **Create your Richie account** |
| Lede | **One account for everything you do in Richie.** |
| First name | Label **First name**; `autocomplete="given-name"`, `autocapitalize="words"`, required |
| Last name | Label **Last name (optional)**; `autocomplete="family-name"` |
| Names hint (shared, `aria-describedby` of both) | **Customers see your first name when you reply.** |
| Email | Label **Email**; placeholder **name@example.com**; `type="email"`, `autocomplete="email"` (no `webauthn`: nobody has a passkey yet) |
| Email hint | **You’ll get a code by email to confirm it’s yours.** |
| Commit | **Create account** (default, `lg`, block), `aria-describedby` = the legal line. Busy **Sending code…** |
| Slot | Empty, reserved (A §4.6) |
| Legal line (12/16 Slate, centred) | **By creating an account, you agree to the Terms of Service and acknowledge the Privacy Policy.** Links: Terms of Service, Privacy Policy |
| Divider | **or** |
| Google | **Sign up with Google** (outline `.ri-method`, the G). Busy **Taking you to Google…** |
| Microsoft | **Sign up with Microsoft** (the four squares). Busy **Taking you to Microsoft…** |
| Switch | **Already have an account?** **Sign in** → A1 |

No passkey button: you can't sign up with a passkey you don't have yet (identity offers one after the first email-code sign-in).

### 5.2 States (all copy; layout `Signup`, alerts as on `Signin-provider-*`)

**Error markup (A11Y-08, the AS `Profile-name-error` pattern).** The names hint and the email hint each sit in a `.ra-slot` row (drawn on the boards). An error is its **own** paragraph in that row, never the hint's text swapped:

- First name: `<p data-slot="field-error" id="first-name-error">` (WarningCircle + text) in the names row; the hint gets `visibility: hidden` but stays referenced. First name: `aria-invalid="true" aria-describedby="first-name-error name-hint"`. **Last name keeps `aria-describedby="name-hint"`** and is never described by the first-name error.
- Email: `<p data-slot="field-error" id="email-error">` in the email row; `aria-invalid="true" aria-describedby="email-error email-hint"`.
- **No links inside an error** (COV-30): an error read through `aria-describedby` isn't announced as a link. The way forward is the page's own switch line, whose **Sign in** link carries the typed address as `login_hint` once the error shows.

| State | Copy and rule |
|---|---|
| First name empty on submit | **Enter your first name.** in the names row (no shift). Focus to First name |
| Email empty or malformed | **Enter a full email address, like name@example.com.** in the email row |
| Both | Both errors; focus to the first invalid field (First name) |
| Email has a Richie account | **This email has a Richie account. Sign in instead.** The switch line's **Sign in** now opens A1 with that address filled |
| Email's account is scheduled for deletion (days 0–30) | **This email has a Richie account. Sign in to keep it.** (the switch line's **Sign in**, as above) |
| Email's account deleted in the last 30 days | No error: a new account is created (AS §4.8) |
| Sending | Commit **Sending code…** (`aria-disabled`); the busy text is written once into the slot's polite paragraph, visually hidden (B-28) |
| Send failed | Slot alert: **We couldn’t send a code because you’re offline.** / **We couldn’t send a code. Check your connection.** / **We couldn’t send a code. Try again in a moment.** |
| L1 reached (5 codes per address per hour) | Commit **Create account in 41:12** (unavailable; digits `aria-hidden`, name "Create account, available at 4:53 PM", B-27), slot **You’ve asked for 5 codes in an hour. Try again in 41:12.** (B-34; the digits `aria-hidden` with sr "at 4:53 PM", as drawn on `Signin-limited`) → at 0:00 **You can send a code now.** |
| L2 reached (per IP) | Same countdown; slot **This network has made 30 attempts in an hour.** (B-34). With its countdown the line measures 2 lines at 358, so the slot carries the line alone and the countdown is the commit’s (spec-edge E9, §7; flagged for the PM) → at 0:00 **You can try again now.** |
| Within 60 s of a code to this address | No new send: straight to Verify with the countdown running (identity review C2) |
| Offline while idle / back online | WP3.2's E7 lines in the slot |
| Provider busy, canceled, error, L6 | §3.1–3.5, sign-up wording; a failed press: **We couldn’t sign you up because you’re offline.** / **We couldn’t sign you up. Check your connection.** / **We couldn’t sign you up. Try again in a moment.** |
| Provider outcomes | §2 rows 7 to 11 in `signup` mode: A3, P5 (sign-up wording), signed in, P7 (sign-up variant), A3 with the code step |
| Success | Verify (identity canvas, sign-up) → account created → passkey offer (identity, as today) → return URL |

Field errors have no `role`: focus moves to the field and `aria-describedby` reads them. Each error is one line at 358 (measured, §15).

**Rationale.** Create account is Sign in's twin: the same column, the same commit-and-slot, the same "or", the providers in the same order. Names sit side by side from a 400 px column and stack below it, where "Last name (optional)" would wrap at 171 px and misalign the two fields. The legal line sits under the commit it qualifies, after the reserved slot so a status never pushes it; the provider buttons don't need it because they lead to A3, whose own **Create account** carries it.

---

## 6. Finish creating your account (A3)

Board `Signup-finish` (mobile `Signup-finish-mobile`). Route `/signup/finish` (needs the external cookie; without it → A2). Document title **"Finish creating your account – Richie"**. Reached from A2's provider buttons and P4's **Sign up with Google**. **Reveals:** nothing new (the person sees their own provider email). **Limit:** L6.

### 6.1 Copy

| Element | Copy and rule |
|---|---|
| `h1` | **Finish creating your account** |
| Lede, Google with a photo | **We’ll use your Google email and photo. Check your name, then create your account.** |
| Lede, no photo | **We’ll use your Google email. Check your name, then create your account.** |
| Lede, Microsoft | **We’ll use your Microsoft email and photo. Check your name, then create your account.** |
| Lede, Microsoft email not confirmed | **We’ll send a code to this email to confirm it’s yours. Check your name first.** |
| Chip | The provider photo (initials from the provider's name when there is none) and the provider email, **priya.natarajan@gmail.com**. Not a field: editing it would undo the proof |
| First name | Prefilled from `given_name` (else the first word of `name`, else empty) |
| Last name (optional) | Prefilled from `family_name`; **empty when the provider gives none** (A §7.4) |
| Names hint | **Customers see your first name when you reply.** |
| Commit | **Create account**. Busy **Creating account…** (unconfirmed email: **Sending code…**) |
| Slot | Empty |
| Legal line | As A2 |
| Second action | **Use a different Google account** (outline) → the challenge again with `prompt=select_account`, mode kept |
| Way out | **Cancel** (plain) → **the start page recorded in the challenge** (the `start` item, COV-32): A2 when the person began on Create account (the drawn journey), A1 when they came from Sign in through P4, A4 when from Welcome back; same return URL. The external cookie is cleared, nothing is created |

"Use a different Google account" is a **link** (a `GET` challenge with `prompt=select_account`), not a fetch-driven method button: it has no busy state, and a failure is the navigation's own (E8 offline, E4 server), so A3's slot stays for **Create account** only (the COV-10 decision, §7.1).

Error markup as A2 (§5.2): `first-name-error` in the names row; Last name keeps `name-hint`.

### 6.2 States

| State | Copy and rule |
|---|---|
| First name empty | **Enter your first name.** (the hint row) |
| Creating | **Creating account…** (`aria-disabled`; busy text once in the slot's polite paragraph, B-28) |
| Create failed | Slot alert: **We couldn’t sign you up because you’re offline.** / **We couldn’t sign you up. Check your connection.** / **We couldn’t sign you up. Try again in a moment.** (departure 3) |
| Unconfirmed email that has an account (checked on submit, before any code) | Slot alert: **This email has a Richie account. Sign in instead.** No link in the alert (COV-30): the outline second action's label becomes **Sign in with your email instead** (same place, same size; → A1 with the email filled), and **Cancel** stays |
| L6 | §3.5 alert; commit **Create account in 41:12** |
| External cookie or sign-in request expired (20 min, sliding, B-29) | E1b (WP3.2); two minutes before, WP3.2's time-limit warning ("Still there? …", **Keep going**) appears in the slot |
| Success | Account created with the names, the confirmed email, the photo and the link (scheme + subject); signed in; the device remembers it with last method Google; activity **Created your account with Google**; **no passkey offer** (A §8 Q19); → return URL |

### 6.3 The code step for an unconfirmed Microsoft email (copy; layout `Provider-link-code`)

Route `/signup/finish/code`. Document title **"Check your email – Richie"** ("Error: Check your email – Richie" after a failed submit, B-31). The code-screen grammar is Part A §4.7 (B-39). AS §4.4 code rules; B-31: arrival focus on the code input, and after a failed submit focus stays in the input with its text selected.

| Element | Copy |
|---|---|
| `h1` / lede | **Check your email** / **Enter the code we sent to {email} to create your account.** (the lede names the purpose, B-39) |
| Code, meta, wrong, locked, expired | As P6 (§7.3) |
| New code sent | **New code sent. Use the code in the newest email.** stays until the first character is typed (B-20) |
| Commit | **Create account**; busy **Creating account…** |
| Way out | **Cancel** (plain, stacked under the commit) → the start page (as §6.1) |
| Alternatives line (one switch line below the commit, B-39) | **Use a different email** → A2 with the names kept (there is no account yet, so no passkey and no recovery). Tab order: the code, Create account, Cancel, Use a different email |

**Rationale.** The provider has already told us who this is, so A3 asks only what customers will see (the name) and states what we take (email and photo) in the lede rather than hiding it in a policy. The email is shown as the account chip because it is a fact proven by the provider, not an input. One filled action; changing account and canceling are quieter and stacked, as on step-up screen A.

---

## 7. Pages after the provider

### 7.1 P4: no Richie account for this Google account (board `Provider-no-account`)

Route `/signin/external/no-account`. Document title **"No Richie account uses this Google account"** (B-13: the `h1` names Richie). Reached by §2 row 9 in `signin` mode. **Reveals:** that the provider's email has no Richie account, which A1 already reveals for any typed email. **Limit:** L6.

| Element | Copy and rule |
|---|---|
| `h1` | **No Richie account uses this Google account** |
| Lede | **Create an account with it, or sign in another way if you already use Richie.** |
| Chip | Provider photo and **priya.natarajan@gmail.com** |
| Primary | **Sign up with Google** (COPY-17: the same name as on Create account, P-1), drawn as the outline method button with the G; a **link** to A3 that reuses the external cookie (no second trip to Google, so no busy state) |
| Secondary | **Use a different Google account** (plain **link**: a `GET` challenge with `prompt=select_account`) |
| Way out | **Sign in another way** (plain link) → A1 with the same return URL; external cookie cleared |
| Note | **Use Richie with a different email? Sign in with it, then link Google in Sign-in and security.** |
| Microsoft (copy) | **No Richie account uses this Microsoft account** / **Sign up with Microsoft** (the four squares) / **Use a different Microsoft account** / note "…link Microsoft…" |

**No status slot, by decision (COV-10).** Every control on P4 is a plain navigation: none uses the `fetch` mechanism of §3.1, none has a busy state, and a failure is the navigation's own (E8 when offline, E4 on a server error, P2 or P3 when Google answers). That keeps the page free of an element that would only ever hold a line for one rare case. The same holds for P7 and A3's "Use a different Google account". (The other option, a reserved `.ri-slot` with the §3.1 lines, would add 20 px and a second behaviour for a secondary control.)

**Focus:** the `h1`. **Rationale.** Three people land here: someone new (create), someone who picked the wrong Google account (switch), and someone whose Richie account uses a work email (sign in that way, then link). The page serves all three in that order of likelihood, and the note gives the third person the one fact they need. The first action carries the G and the Create account label because it *is* signing up with Google; the two quieter ways are plain, so the page has one clear choice (as A4). The `h1` is identity's own "No Richie account uses this email" sentence, so the two read as the same answer. No badge: it is a question, not a failure (A §4.9).

### 7.2 P5: link Microsoft to your Richie account (boards `Provider-link`, `Provider-link-mobile`)

Route `/signin/external/link`. Document title **"Link Microsoft to your Richie account"** (B-13). Reached by §2 row 8. **Reveals:** that the email has an account (as A1); not the account's name, photo or methods. **Limits:** L1 (codes), L6.

| Element | Copy and rule |
|---|---|
| `h1` | **Link Microsoft to your Richie account** |
| Lede | **This email already has a Richie account. Confirm it’s yours to link Microsoft.** (COPY-14; 2 lines at 390) |
| Chip | Initials **JR** from the *provider's* name claims, and **jamie.rivera@northwind.coffee** (the provider email, which is the account email). Never the Richie photo or name |
| Commit | **Send code** (default) → sends an email code to that address → P6. Busy **Sending code…** (written once to the slot's polite paragraph, visually hidden, B-28) |
| Slot | Empty |
| Divider | **or** |
| Passkey | **Use a passkey instead** (outline `.ri-method`, key). Busy **Waiting for your device…** Hidden, with the "or", when the browser has no WebAuthn |
| Way out | **Cancel** (plain) → A1 (A2 for the sign-up wording); external cookie cleared; nothing linked |
| Sign-up wording (J3, copy) | `h1` **You already have a Richie account**; lede **Confirm this email is yours to link Google and sign in.**; the same buttons; Cancel → A2. Document title **"You already have a Richie account"** (B-13) |
| Google (copy) | **Link Google to your Richie account**, lede "…Confirm it’s yours to link Google." |

| State | Copy |
|---|---|
| Send failed | Slot alert: **We couldn’t send a code because you’re offline.** / **…Check your connection.** / **…Try again in a moment.** |
| L1 reached | **Send code in 41:12** (unavailable; name "Send code, available at 4:53 PM", B-27), slot **You’ve asked for 5 codes in an hour. Try again in 41:12.** (B-34, as drawn on `Signin-limited`) → **You can send a code now.**; the passkey stays enabled |
| A code was sent less than 60 s ago | Straight to P6 with the countdown running |
| Passkey canceled or timed out | Slot alert: **The request timed out or you canceled it.** |
| Passkey belongs to another Richie account | Slot alert: **That passkey is for a different Richie account.** |
| Passkey verification fails on our side (B-6) | Slot alert: **We couldn’t sign you in because you’re offline.** / **We couldn’t sign you in. Check your connection.** / **We couldn’t sign you in. Try again in a moment.** |
| Passkey succeeds | Linked, signed in, → return URL (the §8.5 email and activity entries) |
| Offline while idle | WP3.2 E7 lines |

**Behaviour.** The passkey request is discoverable (no `allowCredentials`, so no credential IDs leak); the returned user handle must equal the matched account. The button shows whether or not the account has a passkey, exactly as on Sign in: hiding it would tell a visitor that this account has none (A §1.7 principle 5). **Focus:** the `h1`.

**Rationale.** P5 is Sign in with the email field replaced by the address we already know: one commit (**Send code**), the slot, "or", the passkey. The lede says why we're asking (an account already uses this email) and what happens after (we link Microsoft), in one breath. The chip shows only what the provider gave us, so a person who controls the Microsoft account but not the Richie account learns nothing Sign in wouldn't tell them.

### 7.3 P6: link Microsoft, enter the code (board `Provider-link-code`)

Route `/signin/external/link/code`. Document title **"Check your email – Richie"**; **"Error: Check your email – Richie"** after a failed submit (B-31). The code-screen grammar is Part A §4.7 (B-39): head, code, meta, the commit (with Cancel stacked under it), then the alternatives as one switch line. **Tab order:** the code, **Link Microsoft**, **Cancel**, **Use a passkey instead**, **Can’t get the code?**. **Limits:** L3 (4 tries), L4 (5 minutes, the factor's own lifetime: essential, B-29), L1 for new codes. AS §4.4 code behaviour throughout; strings per A §2.1. B-31: every code screen behaves the same (arrival focus on the input; after a failed submit focus stays in it with the text selected).

| Element | Copy and rule |
|---|---|
| `h1` | **Check your email** |
| Lede | **Enter the code we sent to jamie.rivera@northwind.coffee to link Microsoft.** (the lede names the purpose, B-39; long-email markup, A §4.8; 2 lines at 1440 and 390, 3 at 320, measured) |
| Code | One real input over 8 slots (4 + 4), sr label **Email code, 8 letters and numbers**, `autocomplete="one-time-code"`, Crockford Base32 (I, L → 1; O → 0; hyphens and spaces ignored), paste fills all 8, **no auto-submit** (A §2 row 12). Focus on arrival (drawn) |
| Meta line 1 | **Codes expire after 5 minutes.** |
| Meta line 2 | **Send a new code in 1:00** (tabular, from the server) → at 0:00 the plain button **Send a new code**; sr status **You can send a new code now.** |
| Commit | **Link Microsoft** (default). Busy **Linking Microsoft…**, the same text written once into the sr `role=status` paragraph (B-28) |
| Way out | **Cancel** (plain, stacked under the commit) → A1; nothing linked |
| Alternatives (one switch line **below the commit**, `.ri-switch.ri-code-alts`, 24 apart; B-39) | **Use a passkey instead** (a plain button drawn as the line’s link text; runs P5's passkey path; hidden without WebAuthn) · **Can’t get the code?** → R1 (`Recover`, WP3.4), which returns here. Wraps to two centred lines at 320, 28 apart so the two 46 px hit areas don’t overlap |

| State (all copy except the default) | Copy and rule |
|---|---|
| Incomplete | Meta line 1 error: **Enter all 8 characters.** |
| Wrong | **That code isn’t right. You have 3 tries left.** → **…2 tries left.** → **…You have 1 try left.** Slots invalid, input focused with its text selected |
| Locked (4th wrong try) | Lede **You’ll need a new code. We’ll send it to jamie.rivera@northwind.coffee.** Error **That was the last try for this code.** (focus moves to it, no ring); input `readonly` + `aria-disabled`; meta line 2 hidden; commit **Send a new code in 0:41** (unavailable; digits `aria-hidden`, name "Send a new code, available at 10:14 AM", B-27) → **Send a new code** at 0:00 with **You can send a new code now.** |
| Expired (5 minutes) | Lede as locked; **This code has expired.**; commit **Send a new code** |
| New code sent | Slots clear; meta line 1 **New code sent. Use the code in the newest email.** (polite), **until the first character is typed** (B-20); timer restarts at 1:00 |
| Resend failed | Meta line 1: **We couldn’t send a code because you’re offline.** / **…Check your connection.** / **…Try again in a moment.** |
| Link failed | Meta line 1 alert: **We couldn’t link Microsoft because you’re offline.** / **We couldn’t link Microsoft. Check your connection.** / **We couldn’t link Microsoft. Try again in a moment.** The code stays |
| Sign-in request expired | E1b (WP3.2); the time-limit warning two minutes before (B-29) |
| Success | Linked (scheme + subject), signed in, remembered with last method Microsoft, **security email** and two **activity entries** (§8.5) → return URL |
| Proof succeeds but the link can't be written | **P8** (§7.5) |

The meta reserves 2 lines, 3 in a code container under 360 px (AS R-16; the `.ri-form` is that container), so every line above fits without moving the commit (measured, §15).

**Rationale for P6.** It is the settings code screen in the identity frame, so every code state is the one people already met in step-up. The lede and the commit both say what the code completes ("…to link Microsoft", **Link Microsoft**), so someone back from their mail app sees the purpose before the button. The alternatives follow the one code-screen grammar (B-39): below the commit, as on Verify, so Tab goes from the code straight to the commit.

### 7.4 P7: Microsoft didn’t share a confirmed email (board `Provider-no-email`)

Route `/signin/external/no-email`. Document title **"Microsoft didn’t share a confirmed email – Richie"**. Reached by §2 rows 10 and 11. **Reveals:** nothing. **Limit:** L6.

| Element | Copy and rule |
|---|---|
| Badge | `exclamation-mark-bold`, neutral (A §4.9) |
| `h1` | **Microsoft didn’t share a confirmed email** |
| Lede | **We need one to find your Richie account. Sign in with your email instead.** |
| Primary | **Sign in with your email instead** → A1 with the same return URL; the email field filled with the provider's email when it sent one (as a hint only) |
| Secondary | **Use a different Microsoft account** (outline **link**: a `GET` challenge with `prompt=select_account`; no busy state and no slot, as P4, COV-10) |
| Note | **After you sign in, you can link Microsoft in Sign-in and security.** (settings linking doesn't depend on the email claim: J4) |
| Switch | **New to Richie?** **Create an account** → A2 |
| Sign-up variant (copy) | Lede **We need one to create your Richie account. Use your email instead.**; primary **Create an account with your email** → A2 with the provider's names filled; switch **Already have an account? Sign in** |
| Google (copy) | **Google didn’t share a confirmed email**, same lede and buttons |

**Focus:** the `h1`. **Rationale.** The person can't fix Microsoft's claim, so the page doesn't explain tenants or domain verification; it says what we needed, gives the one way in that always works, and tells them Microsoft can still be added after. The exclamation badge marks a refusal (A §4.9), and the switch line keeps new people from a detour through "No Richie account uses this email".

### 7.5 P8: signed in, not linked (board `Provider-not-linked`, new in Phase 4, COV-11)

Route `/signin/external/not-linked`. Document title **"You’re signed in, but Microsoft isn’t linked – Richie"**. Reached after a successful proof on P5 or P6 when the link can't be written. **Reveals:** the account's own linked methods, only to someone who has just proved the account. **Limit:** none (already signed in).

**IA row** (for Part A §3.1, after P7):

| ID | Page | Route | Document title | Entry points | Exits | Reveals | Limit |
|---|---|---|---|---|---|---|---|
| P8 | **Signed in, not linked** (`Provider-not-linked`) | `/signin/external/not-linked` | You’re signed in, but Microsoft isn’t linked – Richie | P6 **Link Microsoft** or P5's passkey succeeds, but the account already has another account of that provider, or this provider account was linked to another Richie account in the meantime | **Continue** → return URL (Richie app root when it is missing or no longer valid); **Go to Sign-in and security** → `/account/security#providers` | The account's own linked provider, after proof | — |

| Element | Copy and rule |
|---|---|
| Badge | `exclamation-mark-bold`, neutral: the link was refused (A §4.9) |
| `h1` | **You’re signed in, but Microsoft isn’t linked** |
| Lede, the account already has a different Microsoft account | **Your account already has another Microsoft account. Unlink it to use this one.** (drawn) |
| Lede, this Microsoft account was linked to another Richie account in the meantime (copy) | **This Microsoft account was just linked to another Richie account.** |
| Primary | **Continue** (default) → the validated return URL; when it is missing or invalid (A §3.3), the Richie app root |
| Secondary | **Go to Sign-in and security** (outline) → `/account/security#providers` |
| Google (copy) | "…Google isn’t linked", "…another Google account…", "This Google account was just linked…" |

**Behaviour.** The session is already established when this page renders, so both exits work without signing in again; the external cookie is cleared. No security email and no "Linked" activity entry (nothing was linked); the sign-in itself is logged as **Signed in with an email code** or **Signed in with a passkey**, the method that proved the account. **Focus:** the `h1`. **Journey:** J2b, a variant of A §3.4 J2: A1 → Microsoft → P5 → Send code → P6 → Link Microsoft → **P8** → Continue → return URL, or → Sign-in and security → unlink the old one → **Link** there (J17).

**Rationale.** The person did everything right and is signed in; the page says that first, then the one reason linking stopped and where to fix it. It is `Provider-no-email`'s outcome shape without the note and switch line, which this page doesn't need, so it is drawn as its own board.

---

## 8. Cross-cutting rules

### 8.1 What we read from the provider

| | Google | Microsoft (`common` endpoint: work, school and personal) |
|---|---|---|
| Scopes | `openid email profile` | `openid email profile User.Read` (the photo needs Microsoft Graph) |
| Link key (stored) | `sub` | `tid` + `oid` |
| Email and "confirmed" | `email`, `email_verified` | `email` (optional claim), `xms_edov` (optional claim); consumer tenant `9188040d-6c67-4c5b-b112-36a304b66dad` counts as confirmed |
| Name | `given_name`, `family_name`, `name` | `given_name`, `family_name` (optional claims), `name` |
| Photo | `picture` | Graph `GET /me/photo/$value`, at sign-up only |
| Never used | — | `preferred_username` and `upn` as an email (display at most) |

**Stored:** the link (scheme, subject, linked at, last used, the provider email as a display label for settings, refreshed at each sign-in); at sign-up only, the names and the photo as the profile's starting values. **Never stored:** provider access or refresh tokens (`SaveTokens = false`). This matches the settings description "We only get your name, email address and photo from it." (A §5.2).

### 8.2 Return URL

- The challenge stores `returnUrl`, `mode`, `scheme` and the start page (A1, A2 or A4) in `AuthenticationProperties.Items`; the callback re-validates the return URL (`IsValidReturnUrl` or a non-null authorization context, A §3.3) and drops an invalid one silently (→ Richie app root).
- A3, P4, P5, P6, P7 and P8 keep it server-side with the pending external login; their URLs carry no `returnUrl`. Every way back (**Cancel**, **Sign in another way**, **Sign in with your email instead**) returns to the **start page recorded in the challenge** (A1, A2 or A4, the `start` item; COV-32) with the same return URL. P8's **Continue** uses it, else the Richie app root.
- Carried through: A1 → provider → callback → P5 → P6 → return URL; A2 → provider → A3 → return URL; A1 → P4 → A3 → return URL (A §3.3 rule 3).

### 8.3 Remembered account on this device

- **Set** after any successful sign-in on this device: account ID and last method (email code, passkey, Google, Microsoft) in an HttpOnly, Secure, SameSite=Lax cookie, **90 days, renewed at each sign-in** (OQ-4). One account per device: the latest.
- **Shown:** last method passkey → identity's `Passkey-signin`; Google or Microsoft → A4, only while that provider is still linked (else A1 with the email filled); email code → A1 with the email filled.
- **Skipped** when the authorize request carries a `login_hint` for another address, or `prompt=login` or `prompt=select_account`: A1 with the hinted email.
- **Forgotten** by **Not you?** and when the account is deleted. Kept after sign-out (E13, E14): coming back is its purpose.
- It reveals the name, photo and email to whoever uses this device, as `Passkey-signin` already does; never the linked provider address (that would add a second address).

### 8.4 Focus, live regions and document titles

Titles follow B-13: "{h1} – Richie", or the `h1` alone when it already names Richie; "Error: " in front after a failed submit; never a name or an email.

| Page | Focus on arrival | Live regions | Document title |
|---|---|---|---|
| A1 base and P1 | Email field on a fine pointer only (A §4.7); P1 keeps focus on the pressed button | The slot (one polite + one alert); busy text written once into the polite paragraph, visually hidden (B-28) | Sign in to Richie |
| A1 with P2, P3, L6 | The alert container (`role="group"`, `tabindex="-1"`, labelled by its title, described by its description and, on P3, the reference; no ring, B-38); no live role | The slot (P3's "Reference copied." visually hidden there, A11Y-12; offline line) | Sign in to Richie |
| A4 | `h1` | The slot | Welcome back – Richie |
| A2 | First name on a fine pointer | The slot; field errors via `aria-describedby` with their own IDs, no role (A11Y-08) | Create your Richie account ("Error: Create your Richie account") |
| A3 | First name on a fine pointer | The slot | Finish creating your account – Richie |
| A3 code step, P6 | The code input (all pointers: the next step is typing it) | Meta line 1 (alert when it holds an error), one sr `role=status` | Check your email – Richie ("Error: Check your email – Richie", B-31) |
| P4 | `h1` | none (every control is a navigation, COV-10) | No Richie account uses this Google account |
| P5 | `h1` | The slot | Link Microsoft to your Richie account (sign-up wording: You already have a Richie account) |
| P7 | `h1` | none (COV-10) | Microsoft didn’t share a confirmed email – Richie |
| P8 | `h1` | none | You’re signed in, but Microsoft isn’t linked – Richie |

Busy buttons keep focus and use `aria-disabled` (never `disabled`). Countdowns are never live; they announce once at the start and once at 0:00, and a countdown button's digits are `aria-hidden` behind a fixed "available at {time}" suffix (B-27).

### 8.5 Security email and activity entries

**When a provider is linked from sign-in** (P6, or P5's passkey path), Richie sends **the one "Provider linked" security email owned by spec-settings §5.2** (B-7): the same template as a link from settings. This spec doesn't restate it. Its rules, for reference: subject "Microsoft was linked to your Richie account"; an absolute date and time with the account's time zone, else UTC ("on November 5 at 9:31 AM Pacific Time", never "today", B-8, COV-25); the button **Check Sign-in and security**; the shared fine print "You’re getting this because it’s a security change on your Richie account. We always send these."; the layout of WP3.4's `Email-recovery-alert`. Sign-in adds nothing to it: the device and place lines come from the request that completed the link.

**Activity entries** (spec-settings §5.1 owns the list; the ones a sign-in writes, sample time 9:31 AM as `Security-linked`, COPY-28):

| Event | Title | Line 1 | Line 2 | Tile |
|---|---|---|---|---|
| Link from sign-in | **Linked Microsoft** | jamie.rivera@northwind.coffee | Today at 9:31 AM. Chrome on macOS in Portland, Oregon. | `link-simple` |
| Sign-in with a provider | **Signed in with Microsoft** / **Signed in with Google** | — | Same pattern | `sign-in` (as the other sign-ins) |
| Account created with a provider | **Created your account with Google** | priya.natarajan@gmail.com | Same pattern | `user-circle` |

A link from sign-in writes **Linked Microsoft** then **Signed in with Microsoft** (newest first in the list). Signing in with a linked provider sends no email, like every other sign-in. Each provider sign-in updates the settings row's "Last used {date}." Activity dates are relative only up to "yesterday" (B-16).

---

## 9. CSS

`edges.css`, section `/* ===== WP3.5 SSO in sign-in ===== */`. No tokens, no shared recipe values changed; every colour is an `account.css` token, so no new contrast pair (the chip action is the link colour on Mist: 5.11 light, 7.42 dark, AS §7.1).

| Selector | Rule | suiss mapping | Reason |
|---|---|---|---|
| `[data-slot=alert] > .ri-ref.ri-sso-alert-ref` | `grid-column: 2; justify-content: flex-start; margin-top: 4px` | `Alert` third row | The reference inside P3's alert, aligned with the description |
| `[data-slot=button].ri-method[data-busy] > [data-slot=spinner]` | `margin-inline: -2px 2px` | `Spinner data-icon="inline-start"` | The 16 px spinner centred in the 20 px mark box: the label doesn't move when busy |
| `.ri-sso-names-group`, `.ri-sso-names`, `@container ri-col (min-width: 400px)` | flex column gap 8; grid gap 20; two columns gap 16 from 400 | `FieldGroup` + `Field` | Names side by side on desktop, stacked on phones (§5.1) |
| `.ri-sso-legal`, `.ri-sso-legal a` | 12/16 Slate, centred, balance; links `nowrap` | none (identity `rc-legal`) | how-we-design rule 5; a balanced break split "Terms / of Service" |
| `.ri-body > .ri-form:has(.ri-sso-legal) + *` | `margin-top: 24px` | — | Space after the legal line before "or" (A2) or the actions (A3) |
| `.ra .ra-chip.ri-sso-chip-action` | `padding-inline-end: 4px` | `ra-chip` + `Button plain sm` | **Not you?** ends the chip at the usual 16 px text edge |
| `.ri-sso-chip-action > form.ri-sso-forget` | `display: contents` | `form` + `Button type="submit"` | **Not you?** is a POST, drawn as its form (A11Y-17); the form takes no box, so the button stays a flex item of the chip |
| `.ri-form > .ri-actions.ri-sso-commit` | `margin-top: 4px` | stacked block buttons | P6's commit + Cancel, 24 below the code meta like `.ri-commit` |
| `.ri-commit + .ri-actions.ri-sso-after` | `margin-top: 4px` | — | A4's plain way out under the commit's slot |

**Shared, not here (revision round, edges.css §9):** `.ri-switch.ri-code-alts` and the inline plain button in a switch line (B-39, P6’s alternatives); `.ri-head .ra-chip .ra-email-local` (P-B, A4’s chip wraps after the @). **Shared, not here (B-3, merged by the PM):** the page alert's 24 px gap (`.ri-body > [data-slot=alert]`, my WP3.5-1 request; the scoped copy is removed), the chip's start-aligned text in the centred head, slot icons inline with the first word, the forced-colours badge, and §9's wrapping block buttons (lg side padding 16) with the Highlight primary under forced colours. Every WP3.5 button label, drawn and copy-only, fits one line in the 288 px block button at 320 (24 labels measured, §15).

---

## 10. State matrix

Every how-we-design §4 checklist item, per page. "Board" names the board that draws it; "copy" gives the layout board in brackets; "harness" means tested by the rig or the forced-dark build; "n/a" has a reason.

| Checklist item | A1 states | A4 | A2 | A3 | P4 | P5 | P6 | P7 | P8 |
|---|---|---|---|---|---|---|---|---|---|
| Default, realistic data | `Main` (WP3.1) | **`Signin-remembered-provider`** | **`Signup`** | **`Signup-finish`** | **`Provider-no-account`** | **`Provider-link`** | **`Provider-link-code`** | **`Provider-no-email`** | **`Provider-not-linked`** |
| Long email (A §4.8 test address at 320) | n/a (no address) | harness (chip, start-aligned by the shared rule, §15) | copy (field scrolls) | harness | harness | harness | harness (lede wraps after the @) | n/a | n/a (no address) |
| Prefilled from context | `login_hint` (WP3.1) | the remembered account | `login_hint` → email | **names from Google** | provider email | provider email | — | email to A1 | — |
| Arrival focus | P2/P3 alert container (**drawn** on `Signin-provider-*`: `tabindex="-1"`, labelled and described, B-38; programmatic, no ring) | `h1` | First name, fine pointer | First name, fine pointer; code step: the input (B-31) | `h1` | `h1` | **drawn** on the code | `h1` | `h1` |
| Busy | **`Signin-redirecting`** (Google; busy text in the slot, sr-only, B-28), copy (Microsoft, passkey) | copy [`Signin-redirecting`] | copy (**Sending code…**, provider busy) | copy (**Creating account…**) | n/a (links, COV-10) | copy (**Sending code…**, **Waiting for your device…**) | copy (**Linking Microsoft…**) | n/a (links, COV-10) | n/a (both exits navigate) |
| Success / next page | return URL | return URL | Verify | return URL | A3 | P6 / return URL | return URL + email + activity | A1 | return URL or Sign-in and security |
| Server error | **`Signin-provider-error`** (provider); copy, slot (challenge) | copy [P3]; slot | copy (slot) | copy (slot) | E4 (navigation, COV-10) | copy (slot) | copy (meta) | E4 (navigation) | n/a (already signed in; navigation) |
| Network error | copy, slot (challenge); E8 after the hand-over | same | copy (slot) | copy (slot) | E8 (navigation) | copy (slot) | copy (meta) | E8 (navigation) | E8 |
| Offline | copy [`Signin-offline`] | copy | copy | copy | E8 | copy | copy | E8 | E8 |
| Canceled at the provider | **`Signin-provider-canceled`** | copy [P2] | copy [P2, sign-up wording] | n/a (A3 is after the provider) | copy [P2] (switch account) | — | — | copy [P2] | n/a |
| Rate limit / cooldown | copy L6 [alert + `Signin-limited`] | copy L6 | copy L1, L2, L6 | copy L6 | copy L6 | copy L1, 60 s → P6 | copy (resend countdown drawn) | copy L6 | n/a |
| Sign-in request expired (incl. stale callback, B-1) | E1b | E1b | E1b | E1b + warning (B-29) | E1b | E1b + warning | E1b + warning | E1b | n/a (signed in) |
| Codes: wrong 3/2/1, incomplete, locked, expired, new code (until the first character, B-20), send failed, can't get it, "Error: " title (B-31) | n/a | n/a | Verify (identity) | §6.3 copy [`Provider-link-code`] | n/a | n/a | copy (AS `Stepup-code-error`, `-locked` layouts) | n/a | n/a |
| Passkeys: waiting, failed, other account, no WebAuthn | `Main` | n/a (no passkey button) | n/a | n/a | n/a | copy (slot; button and "or" hidden) | copy (alternative hidden) | n/a | n/a |
| Unknown email / no account | `Signin-unknown` (identity) | n/a | n/a | n/a | **`Provider-no-account`** | n/a | n/a | n/a | n/a |
| Already registered / has an account | P5 | P5 | copy (field error, no link; switch line carries the email, COV-30) | copy (unconfirmed: slot error, second action relabelled) | n/a | **`Provider-link`**; sign-up wording copy | n/a | n/a | n/a |
| Already linked to another Richie account | §2 row 7 (signs in to that account) | same | same | n/a | n/a | n/a | → P8 | n/a | **drawn** (the race lede: copy) |
| Unverified email | P7 | P7 | P7 / A3 code step | copy (lede + §6.3) | n/a | n/a | n/a | **`Provider-no-email`** | n/a |
| Deleted in 30 days / scheduled for deletion | `Signin-deleted` / `Deletion-pending` (AS) | same | copy (field error / new account) | new account | n/a | `Deletion-pending` after proof | `Deletion-pending` after linking | n/a | n/a |
| Remembered account and "Not you?" | → A4 | **drawn** (a form submit, A11Y-17) | n/a | n/a | n/a | n/a | n/a | n/a | n/a |
| What it reveals, rate limit | §3 | §4 | §5 | §6 | §7.1 | §7.2 | §7.3 | §7.4 | §7.5 |
| 320, 375, 390, 768, 1280, 1440, ≤540 tall, 200 % | harness | harness | harness (+ `Signup-mobile`) | harness (+ `-mobile`) | harness | harness (+ `-mobile`) | harness | harness | harness |
| Dark | **`Signin-provider-error-dark`**; forced-dark build for the rest | harness | harness | harness | harness | harness | harness | harness | harness |
| Forced colours | harness (4 boards) | harness | harness | n/a | n/a | n/a | harness | n/a | n/a |
| Reduced motion | harness (spinner slows to 2.4 s, AS §7.6) | harness | — | — | — | — | — | — | — |

---

## 11. Departures

From Part A, the reference boards or the brief, each with its reason.

| # | Departure | Reason |
|---|---|---|
| 1 | P4's `h1` is **"No Richie account uses this Google account"**; A §3.1 gives the document title "No Richie account for this Google account – Richie". The title is the new `h1` alone (B-13: it names Richie) | A full sentence (how-we-design rule 8), and the same sentence as identity's "No Richie account uses this email" |
| 2 | The failure phrase on sign-in surfaces is **"sign you in"** (A2, A3: **"sign you up"**), not A §4.2's "reach Google / reach Microsoft"; the three patterns stay. **Now ruling B-6** for every method on every sign-in surface | "We couldn’t reach Microsoft. Check your connection." measured 2 lines at 358 in SF (A §4.6). The `fetch`-then-`location.assign` challenge (§3.1, COV-14) makes the network and server patterns reachable |
| 3 | A3's failure phrase is **"sign you up"** (how-we-design §3: not "sign up") | "We couldn’t create your account. …" measured 2 lines at 358 in Inter and SF; P-1 already put "Sign up with…" on the page that leads here |
| 4 | **"This email has a Richie account. Sign in instead."** and **"…Sign in to keep it."** (identity: "already has… Sign in with it instead"; AS §4.8: "That email already has…") | The longer strings measured 2 lines at 358; a field error that wraps moves the commit after a failed submit |
| 5 | The legal line sits under the commit's reserved slot (identity: directly under the button) | The slot must stay directly under the primary (A §4.6); the legal line must not be pushed by a status |
| 6 | A4 has no filled button: **Sign in with Google** is the outline method button | A §7.4 ("never filled") and Google's ground rule; one action and one quiet way out |
| 7 | P5's chip uses initials from the provider's name, never the Richie photo or name | Reveal no more than Sign in (A §1.7 principle 5) |
| 8 | P6 adds the AS §4.4 alternatives **Use a passkey instead** and **Can’t get the code?** to Part A's exits, as one switch line below the commit (B-39’s grammar, not AS’s place above the footer) | AS §4.4 code rules apply to every code screen; a lost mailbox needs a way to support from here too |
| 9 | In `signup` mode, an unconfirmed email that has an account stops at A3 with "Sign in instead" (never P5) | Applies A §8 Q5 "unconfirmed emails never match an account" to sign-up, which Part A leaves open |
| 10 | Names are side by side only from a 400 px column (identity: always) | "Last name (optional)" wraps at 171 px on phones |
| 11 | L6 on A3 and A4 is the A1 alert with the page's commit counting down | One L6 presentation everywhere (A §3.5 names the A1 alert) |
| 12 | **Canvas-only links** for controls that are form submits in the app: the provider method buttons on `Main`'s copies and on A2 and A4 (`POST /signin/external/challenge` with the antiforgery token, §3.1). The busy Google control on `Signin-redirecting` is drawn as the `<button>`. **"Not you?" is drawn as its real form** (`POST /signin/forget`, A11Y-17), so it is not on this list | The links keep the canvas walkable; engineering builds the submits |
| 13 | P4's and P7's "Use a different … account", A3's "Use a different Google account" and P4's "Sign up with Google" are **plain navigations** (`GET` challenge or a link to A3): no busy state, no `fetch`, no status slot (COV-10) | They are secondary controls on pages without a form; a reserved slot would add 20 px and a second behaviour for one rare failure, which the navigation already answers (E8, E4, P2, P3) |
| 14 | P4's first action is the outline method button **Sign up with Google** (COPY-17), so P4 has no filled button, like A4 | One name per action (P-1), and Google's G only on its own method button |
| 15 | P3's "Copy" feedback is the button's own label ("Copied") plus a visually hidden line in the page slot, not a second polite region in the alert (A11Y-12's first suggestion) | Keeps one polite region per page (A §4.7) and changes no height; the feedback is where the eye and a magnifier already are |
| 16 | P8 (Signed in, not linked) is a page with its own board, `Provider-not-linked` (COV-11, B-33) | It drops `Provider-no-email`'s note and switch line, so the layout and height differ |

---

## 12. Company SSO later

When companies get their own SAML or OIDC sign-in (not now, D1), it slots in after the email step and changes nothing above it: A1 looks up the email's domain when **Send code** is pressed, and for a domain a company has claimed, the commit's label and action become **Continue with SSO** (A §4.3 reserves "SSO" for this) and the press challenges that company's provider (Duende dynamic providers from `IIdentityProviderStore`, or `acr_values=idp:{scheme}` from a client). Google and Microsoft stay where they are. The callback table in §2 gains one rule before row 8 (a company provider's subject is linked, or provisioned only if the company allows it), the P2 and P3 alerts read "your company’s sign-in" instead of a brand, and P4's wording becomes "No Richie account uses this company account". A company that enforces SSO would hide the email-code commit for its domain, which is the first time a Richie page would depend on a workspace: decide that with the Richie app, not here.

---

## 13. Handoff mapping

### 13.1 `@repo/ui` and identity components

| Board element | Component |
|---|---|
| Frame, head, mascot, badge, footer | Identity `IdentityPage` (`.ri-*`, A §6.2) |
| Method buttons | `Button variant="outline" size="lg" block` + a 20 px mark with `data-icon="inline-start"`; `ProviderMark` = Google's official asset as `<img alt="">` (A P-3), Microsoft as the inline four-square SVG |
| Busy | `Button` with `Spinner data-icon="inline-start"` (`aria-hidden`), `aria-disabled`, `data-busy`; reset on `pageshow` |
| Page alerts | `Alert` / `AlertTitle` / `AlertDescription` with `role="group"` instead of a live role (the suiss `Alert` hard-codes `role="alert"`; the identity wrapper must allow overriding it). The **alert** takes `tabIndex={-1}`, `aria-labelledby` the title’s id and `aria-describedby` the description’s id (plus the reference’s on P3), and is focused on mount, no ring (B-38) |
| Reference row | `ReferenceLine` (A §4.5) inside the alert |
| Status slot | `StatusSlot` (`.ri-slot`, AS §8 row 16) |
| Fields | `Field`, `FieldLabel`, `Input`, `FieldDescription`, `FieldError` |
| Code | `InputOTP maxLength={8}` 4 + 4, settings `CodeEntry` (AS §4.4) |
| Account chip | `AccountChip` (`.ra-chip`, `Avatar`, `EmailText`), optional trailing plain `Button size="sm"` |
| Legal line | Identity `LegalNote` (12/16) |

### 13.2 Duende IdentityServer and User Management

| Concern | Setting or API |
|---|---|
| Hosting (COV-08) | **One registration of each scheme, in the identity host**, used by sign-in (`/signin/external/callback`) and by settings linking (`/account/security/link/callback`). The paragraph on hosting and the redirect URIs registered at Google and Microsoft is **spec-settings §9.2's** (WP3.3); this spec follows it |
| Schemes (Google, Microsoft) | **One registration, the same text in spec-settings §9.2 and spec-sso §13.2** (one handler for both providers, registered once in the identity host; COV-08). Google: `AddOpenIdConnect("google", "Google", …)` with `Authority = "https://accounts.google.com"`, `ResponseType = "code"`, scopes `openid email profile`, `CallbackPath = "/signin-google"`. Microsoft: `AddOpenIdConnect("microsoft", "Microsoft", …)` with `Authority = "https://login.microsoftonline.com/common/v2.0"` (any tenant, Part A §8 Q23) and the issuer validated per tenant (`https://login.microsoftonline.com/{tid}/v2.0`), `ResponseType = "code"`, scopes `openid email profile User.Read`, the optional claims `email`, `xms_edov`, `given_name` and `family_name` set in the app registration’s token configuration, `CallbackPath = "/signin-microsoft"` (two OIDC schemes can’t share `/signin-oidc`). Both: `SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme`, `SaveTokens = false` (Richie keeps no provider tokens, so unlinking has nothing to revoke at the provider), `MapInboundClaims = false` and `GetClaimsFromUserInfoEndpoint = false`, so the callback reads the id_token’s own claim names. **Claim mapping:** link key Google `sub`, Microsoft `tid` + `oid`; email `email`, confirmed by Google’s `email_verified` or Microsoft’s `xms_edov` (the consumer tenant `9188040d-6c67-4c5b-b112-36a304b66dad` counts as confirmed); names `given_name`, `family_name`, `name`; photo Google’s `picture` (Microsoft’s from Graph at sign-up, spec-sso §8.1). `preferred_username` and `upn` are never used as an email. Not `AddGoogle` (OAuth 2.0 plus userinfo: no id_token, and the confirmed flag arrives as `verified_email`) and not `AddMicrosoftAccount` (no id_token, so no `tid`, `oid` or `xms_edov`). |
| Challenge | Method buttons: `POST /signin/external/challenge` → `Challenge(scheme, new AuthenticationProperties { RedirectUri = "/signin/external/callback", Items = { ["scheme"], ["returnUrl"], ["mode"], ["start"] } })`; `prompt=select_account` for "Use a different … account"; `login_hint` from A4 (both via `OnRedirectToIdentityProvider`). For the `fetch` path (§3.1) the same event writes `ProtocolMessage.CreateAuthenticationRequestUrl()` as JSON and calls `HandleResponse()` instead of redirecting; the no-JS form path keeps the `302`. The plain "Use a different … account" links (P4, P7, A3; COV-10): `GET /signin/external/{scheme}?prompt=select_account`, the scheme, mode and return URL taken from the pending external login, never from the query |
| Long state | `AddOidcStateDataFormatterCache("google", "microsoft")` |
| Callback | `HttpContext.AuthenticateAsync(ExternalCookieAuthenticationScheme)` → §2 table → `HttpContext.SignInAsync(new IdentityServerUser(subjectId) { DisplayName, IdentityProvider = scheme, AuthenticationMethods = { "external" } })` → `SignOutAsync(ExternalCookieAuthenticationScheme)` → redirect to the validated return URL |
| Events | `UserLoginSuccessEvent(scheme, providerSubject, subjectId, name, clientId: context?.Client.ClientId)`; `UserLoginFailureEvent` with the reference for P3 |
| Sign-up entry | `UserInteraction.CreateAccountUrl = "/signup"` (advertises `prompt=create`); A2 and A3 call `GetAuthorizationContextAsync(returnUrl)` and treat `null` as "no return URL" |
| Users and links | User Management user store: create the user (names, email confirmed, photo) and add the external login (scheme, subject); find by (scheme, subject) on every callback; email codes through User Management OTP (8 characters, 5 minutes, 4 tries, ≥60 s) for P5, P6 and §6.3 |
| Pending external login | Server-side record keyed to the external cookie: scheme, subject, provider email and confirmed flag, names, photo URL, matched account ID, return URL, mode; 20 minutes |
| Claims in our tokens | `idp` = `google` / `microsoft`, `amr` = `external`; nothing provider-specific beyond that |

---

## 14. Open questions, with defaults

Design proceeds on the default.

| # | Question | Default | Confirm |
|---|---|---|---|
| OQ-1 | Let an *unconfirmed* Microsoft email that matches an account go to P5 (the email code is the proof anyway) instead of P7? | **No** (A §8 Q5 as written). It would save J4's detour and add no takeover risk (the code goes to the account's own mailbox); it would reveal account existence as A1 does. A routing change; P7 stays for "no email" | Security |
| OQ-2 | Does a fresh Google or Microsoft sign-in count as "recent" for settings step-up (AS §4.4, 10 minutes)? | **Resolved by B-18: yes.** Any successful sign-in starts the 10-minute window, Google and Microsoft included; step-up methods stay a passkey or an email code. (My earlier default "no" is withdrawn.) | PM (decided) |
| OQ-3 | Use the provider photo as the profile photo at sign-up? | **Yes**, said in A3's lede; changeable in Profile | Product |
| OQ-4 | Remembered account lifetime | **90 days, renewed at each sign-in**, one account per device | Product |
| OQ-5 | Ask Microsoft for `User.Read` (photo) on every sign-in, or only at sign-up? | **Every time** (one scope set; it is the standard "Sign in and read your profile" consent). If a tenant blocks it, P3's organization variant | Engineering |
| OQ-6 | "Sign up with Microsoft" isn't in Microsoft's named labels (A §8 Q3) | Use it; brand checks before launch; fallback "Sign in with Microsoft" on A2 (a text swap) | PM, brand |
| OQ-7 | Signing up with a provider account that is already linked | **Sign in silently** to that account (§2 row 7); no "you already have an account" page | Product |
| OQ-8 | More than one Google or Microsoft account per Richie account? | **No**, one per provider (settings rows); the "Signed in, not linked" page covers the conflict | Product |
| OQ-9 | Callback timeout talking to the provider | **10 s**, then P3 | Engineering |
| OQ-10 | External cookie and pending-login lifetime | **20 minutes**, the sign-in request's own (A §8 Q1), so both expire together into E1b; both slide with activity (B-29) | Engineering |
| OQ-11 | Where does a stale or mismatched provider `state` go? | **Resolved by B-1: E1b** ("Your sign-in timed out", "Start again and choose Google"; spec-edge D7), not E5 (§2 row 1) | PM (decided) |

Resolved here (from the A §7.4 brief): P2 and P3 copy (§3.2, §3.3); A4 shows no passkey button (§4); A3 with no last name leaves the field empty (§6.1); no passkey offer after a provider sign-up (§6.2).

---

## 15. Test results

Commands as A §5.4, port 8815, out `.design/edges/out-wp35`, results `.design/edges/qa/wp35` (2026-11-05).

**Phase 4 fix round** (all 15 boards, after the PM's shared CSS merge and the WP3.5 fixes; `qa/wp35/p4-fix`, `qa/wp35/p4-fix-dark`):

| Run | Boards | Blockers | Majors | Minors |
|---|---|---|---|---|
| Rig, light and dark schemes, 10 viewports + 200 % | 15 | **0** | **0** | 62 |
| `BOARD_THEME=dark` build (`out-wp35-dark`) | 15 | **0** | **0** | 62 |
| Text spacing (WCAG 1.4.12 style: line-height 1.5, letter-spacing .12em, word-spacing .16em, paragraph spacing 2em) at 320 and 1440, light and dark build (`ts.mjs` on my build dirs) | 15 | **0 findings** (no horizontal scroll, clipped text, spilled button label or ellipsis) | | |
| Button labels in a 288 px block button at 320, 16 px (RIG-02): 14 text-only, 10 with a mark, drawn and copy-only | 24 | **all one line** ("Use a different Microsoft account", "Sign in with Microsoft in 41:12" included) | | |

The 62 minors are the known list below (45 footer links at 24 px on desktop, 16 inline legal links, 1 axe review on the hidden code input). Looked at by eye: `Provider-no-account` 390, `Provider-not-linked` 1440 and 320 dark, `Provider-link-mobile` 390, `Signin-remembered-provider` 390 dark.

**Revision 1 runs:**

| Run | Boards | Blockers | Majors | Minors |
|---|---|---|---|---|
| Rig, light and dark schemes, 10 viewports + 200 % | 14 | **0** | **0** | 59 |
| Forced-dark build (`class="dark"` on every `<body>`, `qa/wp35/forced-dark`) | 14 | **0** | **0** | 59 |
| `--forced-colors` (`Signup`, `Signin-provider-error`, `Signin-redirecting`, `Signin-remembered-provider`; `qa/wp35/forced-colors`) | 4 | 0 | 46 (all discarded, below) | 24 |
| `--forced-colors` baseline on WP3.1's `Main` (`qa/wp35/forced-colors-main-baseline`) | 1 | 0 | 12 (same class) | 5 |

**Discarded false positives** (A §5.4 and how-we-design §5 list, plus one rig limitation):

- 42 + 12 target-size minors on footer links (24 px on fine pointers ≥768, by design; 44 on phones).
- 16 inline legal links under 24 px ("Terms of Service", "Privacy Policy"; WCAG 2.5.8 exempt).
- 1 axe "needs review" on the hidden OTP input `#link-code` (transparent text over drawn slots).
- **46 `axe/color-contrast` majors under `--forced-colors`:** axe measures the authored foreground (`#1d1d1f`, `#6e6e73`, `#0066cc`) against a forced `#000000` canvas. The screenshots render system colours correctly (black text on white, link blue, the alert's outline, button borders, the spinner and both marks intact). WP3.1's tested `Main` shows the same 12 findings, so it is the emulation, not the boards.

**Checked by eye** (screenshots in `qa/wp35/shots/` and `forced-dark/shots/`): 1440 light, 390 light, 390 forced dark, 320, 1280 × 540 (mascot 1×), the `-dark` wrapper, forced colours. The mascot is on whole pixels at every width (689, 164, 129); the G and the four squares render in light and dark; nothing overflows at 320 or at 200 %.

**Layout shift** (y at 1440 / 390): `Main` and `Signin-redirecting` are identical (`h1` 166 / 134, **Send code** 366 / 334, "or" 438 / 406, first method 478 / 446): the busy state moves nothing. The P2 and P3 alerts push the form down by their height (A §4.10 accepts it for news present at first paint).

**Long email** (A §4.8 address injected at 320): no overflow; P6's lede wraps after the @ and never truncates the domain. The chips (A3, A4, P4, P5) wrap the domain inside the pill, centred until the PM's shared chip fix (CSS-REQUESTS #3) lands; with that rule injected the lines start at the avatar and read cleanly (checked on `Signin-remembered-provider` and `Provider-link`).

**Drift:** 0 in WP3.5 boards (the one drift reported is in other WPs' sidebar copies). **Icons:** `check-icons` 0 bad (`info`, `warning-circle`, `key`, `exclamation-mark-bold`). **Status strings:** 31 strings measured with `measure-status.mjs` at 460, 358, 343, 288 (`qa/wp35/slot-strings.json`, `slot-measure.json`): every slot and field-error string is 1 line at 358 and ≤2 at 288 in Inter and SF; meta lines fit the code meta's 3-line reservation under 360. Copy-only ledes and titles measured ≤2 lines at 390.

---

## 16. Links other WPs need

- **WP3.2:** E3 reuses the alert layout of `Signin-provider-canceled` as drawn; its structure (Info icon, the focusable `role="group"` container labelled by `alert-title` and described by the description, B-38; no live role; first child of `.ri-body`) stays fixed (CSS-REQUESTS WP3.5-1 asks to make its 24 px gap shared). **E7 adopts §3.1's mechanism and copy** (COV-14, B-6): provider buttons are form submits, enhanced with `fetch` + `location.assign`, and a press that fails says "We couldn’t sign you in…" in the slot; only a failure after the hand-over lands on E8. Stale callbacks and expired requests go to **E1b** (B-1, §2 rows 1–2). A2 and P5 use B-34’s L1 line with its countdown, "You’ve asked for 5 codes in an hour. Try again in 41:12."; the L2 line and the B-29 time-limit warning (drawn on `Signin-timeout-warning`, used on A3, P5, P6) are yours.
- **WP3.3:** the "Provider linked" email is yours (spec-settings §5.2, B-7); a link from sign-in sends it unchanged. The activity rows a sign-in writes are in §8.5 (sample time 9:31 AM, as `Security-linked`). One scheme registration in the identity host for both callers; your §9.2 paragraph on hosting and redirect URIs is the reference (COV-08), and §13.2 carries the same registration text as your §9.2 (`AddOpenIdConnect` for both providers, the claim mapping and each `CallbackPath`). B-18 replaces my old OQ-2: a fresh provider sign-in starts the 10-minute window. P8 sends people to `/account/security#providers` to unlink the old account.
- **WP3.4:** P6's **Can’t get the code?** opens R1 (`Recover`); R1's **Back to the code** should return to the code page that opened it (Verify or P6), not always Verify. R1's **Sign in with Microsoft** from P6 leads back to P5, which is a loop but not a dead end.
- **PM:** P8 (Signed in, not linked) belongs in Part A §3.1 after P7 (row in §7.5) and in §5.2's WP3.5 table: `Provider-not-linked`, 1440×900, base (CANVAS-CHANGES WP3.5-4). `Main` was not changed by WP3.5 (B-2 was yours).

---

## 17. Changelog

- **Revision 1 (2026-11-05):** the 14 WP3.5 boards, the WP3.5 CSS section, this spec. Rig 0 blockers, 0 majors.
- **Revision 2 (2026-11-05, Phase 4 fix round):** findings in §18; rulings B-1, B-6, B-7, B-8, B-9, B-13, B-18, B-20, B-23, B-27, B-28, B-29, B-31 applied. New board `Provider-not-linked` (P8). Boards changed: `Signin-redirecting` (sr busy text), `Signin-remembered-provider` ("Not you?" form), `Signup`, `Signup-finish` (names hint row), `Provider-no-account` ("Sign up with Google", plain links), `Provider-link` (lede); the mobile and dark wrappers follow. CSS: the page-alert gap rule removed (shared now), the "Not you?" form rule added. Rig 0 blockers, 0 majors on 15 boards, light and dark; text spacing clean.

---

## 18. Fix log (Phase 4)

Findings from `qa/p4-rig.md`, `p4-a11y.md`, `p4-copy.md` and `p4-coverage.md` assigned to WP3.5, and the Part B rulings that land here.

| ID | Severity | Result | What changed |
|---|---|---|---|
| RIG-02 | major | **Fixed** (shared CSS by the PM; verified here) | Block buttons wrap and have 16 px side padding (shared §9). All 24 WP3.5 labels, drawn and copy-only, measure one line in the 288 px button at 320; the text-spacing check finds nothing on the 15 boards (§15) |
| A11Y-08 | minor | **Fixed** | §5.2 and §6.1 specify separate `first-name-error` and `email-error` paragraphs in the hint rows; Last name keeps `name-hint`. The boards draw the names hint in its `.ra-slot` row |
| A11Y-12 | minor | **Fixed** | §3.3: "Copy" shows **Copied** at the button; "Reference copied." goes to the page slot visually hidden. One polite region stays (departure 15) |
| A11Y-17 | minor | **Fixed** | "Not you?" is drawn as its `POST` form (`form.ri-sso-forget` + submit button) on `Signin-remembered-provider`; §4 and §11 #12 say so |
| COPY-03 | major | **Fixed** (by removal) | §8.5 no longer restates the email; it points to spec-settings §5.2 (B-7), whose wording has no serial comma issue to repeat here |
| COPY-14 | minor | **Fixed** | P5's lede: "Confirm it’s yours to link Microsoft." (board and §7.2) |
| COPY-17 | minor | **Fixed** | P4's primary is **Sign up with Google** (method button, straight to A3). The other half of the finding is recorded: `Main`'s switch link keeps "Create an account" (the link names the page) and A2's commit "Create account" (the act) |
| COPY-28 | polish | **Fixed** | §8.5's activity sample time is 9:31 AM, as `Security-linked` |
| COV-08 | major | **Referenced** (WP3.3 writes it) | §13.2 points to spec-settings §9.2 for hosting and redirect URIs; the scheme registration is one text in both specs (`AddOpenIdConnect` for Google and Microsoft; revision round) |
| COV-10 | major | **Fixed, by decision: plain links** | P4's and P7's "Use a different … account" (and A3's) are `GET` challenges with no busy state and no `fetch`; P4's "Sign up with Google" is a link to A3. No slot is needed; failures are the navigation's own (E8, E4, P2, P3). Recorded in §7.1 and departure 13 |
| COV-11 | major | **Fixed** | P8 in §7.5 with its IA row, copy, focus, exits and the invalid-return-URL rule (Richie app root); board `Provider-not-linked` 1440×900 drawn, since its layout differs from `Provider-no-email`; matrix column added |
| COV-14 | major | **Fixed** (the WP3.5 half) | §3.1 is now the one mechanism and copy for provider buttons on A1, A2, A4, R1 and E7: form submit, enhanced with `fetch` + `location.assign`, "We couldn’t sign you in…". WP3.2 aligns E7 and WP3.4 R1 to it |
| COV-15 | minor | **Fixed** | §2 row 1, OQ-11 and §16 say E1b (B-1) |
| COV-25 | minor | **Fixed** (by reference) | The linked email uses spec-settings §5.2's absolute date with the zone (B-7, B-8) |
| COV-30 | minor | **Fixed** | No links inside errors: A2's switch line carries the typed email; A3 relabels its second action **Sign in with your email instead** (§5.2, §6.2) |
| COV-32 | minor | **Fixed** | A3's **Cancel** returns to the start page recorded in the challenge (A1, A2 or A4), §6.1 and §8.2 |
| B-6 | ruling | **Applied** | "We couldn’t sign you in…" for passkey, Google and Microsoft on sign-in surfaces (§3.1, §3.4, §4, §7.2); "sign you up" on A2 and A3 |
| B-7, B-8 | ruling | **Applied** | §8.5 |
| B-9 | ruling | **Applied** | §3.3 organization variant: "Ask your IT admin to allow Richie, or use your email or a passkey." |
| B-13 | ruling | **Applied** | Titles in §3, §5, §7 and §8.4: "Sign in to Richie", "Create your Richie account", "No Richie account uses this Google account", "Link Microsoft to your Richie account", "You already have a Richie account" |
| B-18 | ruling | **Applied** | OQ-2 resolved: any sign-in starts the 10-minute window |
| B-20, B-31 | ruling | **Applied** | P6 and A3's code step: "New code sent…" until the first character; "Error: " title; focus stays in the input |
| B-23 | ruling | **Applied** | L6 counts failed provider sign-ins only (§3.5) |
| B-27 | ruling | **Applied** | Countdown buttons: digits `aria-hidden`, "…, available at {time}" (§3.5, §5.2, §7.2, §7.3) |
| B-28 | ruling | **Applied** | Busy text written once into the polite slot paragraph (drawn on `Signin-redirecting`; §3.1, §5.2, §6.2, §7.2, §7.3) |
| B-29 | ruling | **Applied** (pointer) | The sign-in request slides with activity; WP3.2's time-limit warning appears on A3, P5 and P6 |

New board: `Provider-not-linked`, 1440×900, base (CANVAS-CHANGES WP3.5-4). No size changes to the other 14.

---

## Revision round (Phase 5)

After the grade (`evaluation.md`) and rulings B-34 to B-43. Every change is written into the sections above.

1. No change in this file.
2. No change in this file.
3. No change in this file.
4. **B-34:** the L6 alert says "This network has made 30 attempts in an hour. Try again in 42 minutes, or sign in with a passkey." (§3.5); A2’s and P5’s L1 slot "You’ve asked for 5 codes in an hour. Try again in 41:12."; A2’s L2 slot "This network has made 30 attempts in an hour." with the countdown on the commit (it doesn’t fit the slot with it: flagged).
5. No change in this file.
6. **B-38:** P2, P3 and the L6 alert focus the alert container (`role="group"`, labelled by the title, described by the description and, on P3, the reference); `Signin-provider-canceled`, `Signin-provider-error` (the `-dark` wrapper follows); §3.2, §3.3, §3.5, §8.4, §10, §13.1, §16.
7. **B-39:** `Provider-link-code`’s lede "…to link Microsoft." and its alternatives as one switch line below the commit (Tab: the code, Link Microsoft, Cancel, the alternatives); A3’s code step the same (§6.3); departure 8 and §9 updated.
8. No change in this file.
9. No change in this file.
10. No change in this file.
11. No change in this file.
12. **S-C:** §13.2’s scheme registration is the same text as spec-settings §9.2 (`AddOpenIdConnect` for Google and Microsoft, the claim mapping, `CallbackPath` `/signin-google` and `/signin-microsoft`); the `AddGoogle` row is gone.
13. **P-B:** A4’s chip wraps after the @ at every width (§4; measured one line at 1440, two at 390, 360 and 320).
14. No drawer or action sheet in these pages.

**Boards:** changed `Signin-provider-canceled`, `Signin-provider-error`, `Provider-link-code` (`Signin-provider-error-dark` follows); `Signin-remembered-provider` changes through edges.css §9 only.

**Tests (revision round, 2026-11-05).** Rig on the 52 changed, new and base boards (`Main`, `Security` and every copy), port 8831, 10 viewports: light build (`.design/edges/qa/rev/light`) **0 blockers, 0 majors, 396 minors**; `BOARD_THEME=dark` build with `--schemes dark` (`.design/edges/qa/rev/dark`) **0 / 0 / 396**. Every minor is a known class: 158 targets on inert pages behind overlays, 105 footer links at 24 px on fine pointers, 80 sidebar items at 36 px, 30 axe "needs review" contrast items (scrims, transparent code inputs, **Link** on Mist), 23 inline links. Text spacing (1.4.12, 320 and 1440, both builds): only the known false positives (the skip link; the sr-only ", opens in a new tab" in **Contact support**). Block labels at 320: 90 block buttons, widest 226 px (≤ 256), none taller than 44. Drift: the intended sidebar copies only. Icons: 0 bad. Every `href`, form `action` and `dc-import` target exists.
