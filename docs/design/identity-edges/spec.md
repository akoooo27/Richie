# Richie identity edges: design spec, Part A (foundation)

> **Revision 2 (2026-11-05): synced to the domain specs and Part B after Phase 4 (B-26).** Rows, journeys, limits, grammar examples and the board list below carry the domain specs’ current values. A changed IA row says *(updated: spec-x §n)*; a row Revision 1 lacked says *(added: spec-x §n)*. §8 and §9 still show the Revision 1 questions and decisions, except where Part B overrides them.

WP3.1, Richie Design Program, identity edges workstream. **Revision 2, 2026-11-05.** Written for the four domain designers (WP3.2 to WP3.5), the PM who assembles and publishes the canvas, the testers and the evaluator.

This file is the single source for what the four workstreams share: the design plan, the decisions where the two Richie systems and suiss disagree, the IA for every page in scope, the shared grammar, the canvas plan, the CSS plan, the briefs and the open questions. Each domain designer writes their own pages in their own file (Parts B to E, §7), and those files only add to this one. **Precedence (one order for all five files, ruling B-37):** Part B rulings in `spec.md`, then the domain specs’ page tables and recorded departures, then Part A (Revision 2 synced it to both). A board that disagrees with its spec is a bug in the board.

**Read first, in this order:** [`../how-we-design.md`](../how-we-design.md) (Revision 2: §2 systems, §3 copy, §4 states, §5 canvas, §7 rules), [`inventory.md`](inventory.md) (what exists), [`suiss-addendum.md`](suiss-addendum.md) (suiss blocks for these pages), [`../account-settings/spec.md`](../account-settings/spec.md) (Revision 5, cited as **AS §**), [`../suiss-ui-guide.md`](../suiss-ui-guide.md) §7.

**Decided by the user on 2026-11-05** (binding; called **D1 to D3** below):

- **D1.** Single sign-on means **Google and Microsoft only**: "Sign in with Google" and "Sign in with Microsoft" on sign-in surfaces, "Sign up with Google" and "Sign up with Microsoft" on Create account (labels: §9 PM decisions), linked to the personal Richie account, and the linked accounts listed in settings. No company SAML or OIDC now, but layouts keep room for it.
- **D2.** Lost email: **passkey first, then support.** With a passkey or a linked Google or Microsoft account, sign in with it and change the email in settings. Without one: a recovery request that support reviews, the old address is told and can cancel, a **72-hour hold** before the email changes, then the account is *recently recovered* (step-up for every sensitive action for 7 days, a security-activity entry, every device signed out).
- **D3.** Passkey management and change email shipped in settings (8.7/10 Ship). Settings work here **fills gaps only**, plus the new Google and Microsoft section.

**Sources** (read 2026-11-05):

| Source | Version or location |
|---|---|
| Canvas "Richie Identity Edges" (empty; the PM publishes) | https://claude.ai/artifact/6Xe7bnSdGHE3xvVR5LeMge, version `1793882667-bfa5` |
| `account.css` | `.design/edges/project/account.css`, byte-identical to the settings canvas version `1793877977-4d5d` (checked with `cmp`) |
| `richie.css` and the identity boards | `.design/edges/ref/identity/` (identity canvas `1793882377-cd80`) |
| Settings boards | `.design/edges/ref/settings/` |
| Shiba on this canvas | SVG `/_blob/b4c7243609a18c06cc93f503a2c6c03e` (62 × 78 display, `crispEdges`), PNG `/_blob/6445032e8879407a1ebe015146fd29a2` (31 × 39). Local copies in `.design/edges/blobs/` |
| Google mark | Google "Sign in with Google" branding bundle, `signin-assets.zip` from developers.google.com/identity/branding-guidelines, file `Android + Web/SVG/Light/Theme=Light, Show text=No, Shape=Square, Platform=Android+Web.svg`, logo area only (§6.2) |
| Microsoft mark | `ms-symbollockup_mssymbol_19.svg` from learn.microsoft.com/entra/identity-platform/howto-add-branding-in-apps, unchanged |
| Icons | Phosphor `@phosphor-icons/core@2.1.1`, regular, plus four **bold** badge glyphs. Checked set: `.design/edges/icons/` |

Values marked **(m)** were measured for this revision: the reference boards built with the rig and probed in Chromium on macOS, or ratios computed with `.design/edges/wp31/contrast.mjs` (output in `.design/edges/wp31/contrast-report.md`).

---

## Contents

1. [Design plan and self-review](#1-design-plan-and-self-review)
2. [Decisions where identity, settings and suiss conflict](#2-decisions-where-identity-settings-and-suiss-conflict)
3. [IA: every page, IdentityServer mapping, return URLs, journeys, limits](#3-ia)
4. [Shared grammar for identity pages](#4-shared-grammar-for-identity-pages)
5. [Canvas plan and board list](#5-canvas-plan-and-board-list)
6. [CSS plan for `edges.css`](#6-css-plan-for-edgescss)
7. [Briefs for WP3.2 to WP3.5](#7-briefs-for-wp32-to-wp35)
8. [Open product questions, with defaults](#8-open-product-questions-with-defaults)
9. [For the PM: decide before the domain designers start](#9-for-the-pm-decide-before-the-domain-designers-start)
10. [Changelog](#10-changelog)

---

## 1. Design plan and self-review

### 1.1 The brief in three lines

- **Subject:** the moments around signing in to Richie, a customer messaging product: something went wrong, time ran out, you signed out, you lost your email, or you want to use Google or Microsoft.
- **Audience:** owners and teammates with personal accounts, often on a phone, often arriving from an email link or from another app, sometimes worried (a security email, a lost mailbox).
- **Primary job of every page:** say what happened, keep the person’s place (their email, where they were going, the sign-in they started), and give one next step. Never a dead end.

### 1.2 Concept: the sign-in page with the form taken away

Every page in this area is drawn in the frame of **Sign in**. The column, the mascot, the title and the footer sit in the same places on Sign in, on a timeout, on an error, on a recovery status page and on the signed-out page. Only the words under the title and the one action change. Moving from Sign in to an edge page should feel like the same place reporting back, not like landing on a different site.

**The one bold thing is small and deliberate: the shiba carries the state.** On outcome and status pages a 30 px badge sits on the mascot’s shoulder and says the outcome before you read a word: a check when something you wanted is done, a clock when time ran out or you’re waiting, a crossed-out signal when you’re offline, an exclamation mark when something failed. Nothing else on these pages decorates: no illustrations, no coloured or red pages, no icon discs, no hero art. The settings area spent its boldness on the live messenger preview; identity spends it on the mascot it already has.

### 1.3 Palette (six names; values are the settings tokens, AS §7.1)

| Name | Role here | Light | Dark |
|---|---|---|---|
| **Paper** (with **Mist**, its raised step) | Page; Mist for alerts and grouped summaries | `#ffffff` / `#f5f5f7` | `#000000` / `#161617` |
| **Ink** | Text, the neutral badge disc | `#1d1d1f` | `#f5f5f7` |
| **Slate** | Ledes, hints, notes, the step line, the "or" word; field and button edges | text `#6e6e73`, edges `#86868b` | text `#a1a1a6`, edges `#6e6e73` |
| **Signal** | The one filled primary per page; links and focus | fill `#0071e3`, link `#0066cc` | fill `#0071e3`, link and focus `#2997ff` |
| **Leaf** | The ok badge disc only (never text) | `#16a34a`, white glyph | `#4ade80`, Ink glyph |
| **Alarm** | Field errors and destructive buttons only | `#d0000a` | `#ff5447` |

Google’s and Microsoft’s marks keep their own colours and appear only inside their method buttons and their settings tiles.

### 1.4 Type

One family, the suiss stack (`-apple-system, BlinkMacSystemFont, "Inter Variable", "Inter", sans-serif`: SF Pro on Apple devices, Inter elsewhere). Weights 400 and 600. No monospace anywhere. Every size is fixed: nothing depends on the viewport’s width or height.

| Role | Size / line | Weight | Used for |
|---|---|---|---|
| Title | 30 / 36, −0.025em | 600 | Every page `h1` |
| Body and lede | 16 / 24 | 400 | Ledes (Slate), body sentences, input text, `lg` button labels |
| Label | 16 / 24 | 600 | Field labels |
| Detail | 14 / 20 | 400 | Hints, errors, status lines, notes, the switch line, the reference line, "Step 2 of 3", "or" |
| Footer | 12 / 16 | 400, "Richie" 600 | Footer |
| Code glyph | 20 / 28, tabular, uppercase | 600 | The 8 code slots (AS §7.2 row 20) |

### 1.5 Layout concept

Three page shapes share one frame (wireframes with measured positions in §4.1):

```
 form page                    outcome page                 status page
 ┌──────── 460 ────────┐      ┌──────── 460 ────────┐      ┌──────── 460 ────────┐
 │       [shiba]       │      │      [shiba]✓       │      │      [shiba]◷       │
 │                     │      │                     │      │    Step 2 of 3 *    │
 │  Sign in to Richie  │      │  You’re signed out  │      │ Your email changes  │
 │   one-line lede     │      │  two-line lede max  │      │   on November 8     │
 │                     │      │                     │      │   lede              │
 │ Label               │      │ [ Primary action  ] │      │ ┌ Mist summary ───┐ │
 │ [field            ] │      │                     │      │ │ row             │ │
 │ [ Primary         ] │      │   centred note      │      │ │ row             │ │
 │   (status slot)     │      │                     │      │ └─────────────────┘ │
 │ ──────── or ─────── │      │                     │      │ [ Secondary       ] │
 │ [ Method          ] │      │                     │      │   centred note      │
 │ [ Method          ] │      │                     │      │                     │
 │   switch line       │      │                     │      │                     │
 └─────────────────────┘      └─────────────────────┘      └─────────────────────┘
        Richie  Help  Privacy  Terms   (12 px, at the bottom of the window)
 * multi-step flows only
```

Rejected layouts, so nobody re-proposes them:

- **Split screen** (illustration or brand panel on one side): a second visual system, and on phones the panel becomes a banner that pushes the form down.
- **Settings’ start-aligned solo frame** (AS §4.8): the mascot and title jump from the centre of Sign in to the left edge on the next page ((m) mark x 164 → 16 at 390, how-we-design §2).
- **Vertically centred card**: the title and button move whenever content length changes; top-anchored pages keep them still.

### 1.6 Alignment rules

1. **Centred:** the mascot, the step line, the `h1`, the lede, the account chip, the status slot, notes under actions, the switch line, the reference line and the footer.
2. **Start-aligned:** field labels, inputs, hints and field errors, bulleted lists, grouped summaries and alert content.
3. **Full column width:** every button (`lg`, 44 px, block, radius 10). No pill buttons and no side-by-side buttons in the identity frame.
4. **Whole pixels:** the column and the mascot are placed on whole pixels at every width, so the pixel art stays sharp on 1× screens ((m) mascot x 156 at 375, where plain centring gives 156.5).
5. **Top-anchored:** the mascot’s top is at 64 px (≥768 wide), 32 px (<768) and 16 px (≤540 tall). The `h1` top is at 166 at 1440 × 900 and 134 at 390 × 844 (m) on every page without a step line. Nothing centres vertically.

### 1.7 Principles for this area

1. **Keep their place.** An edge page keeps the email, the return URL and, where it still exists, the sign-in that was started. Restarting never makes someone retype what we already know.
2. **One next step, always a way out.** One filled primary per page. Every page also has a way back (Sign in, Go to Richie or Help).
3. **The badge says the outcome; colour never says it alone.** The `h1` states the fact in words. The badge is `aria-hidden`.
4. **Say what we know, with real numbers.** "Your sign-in timed out after 20 minutes", "Try again in 12 minutes", "We’ll change it on November 8 at 4:10 PM". Never "invalid", "oops" or an exclamation mark.
5. **Reveal no more than Sign in already does.** Sign in says whether an email has an account (accepted, rate-limited). Nothing in this area reveals more: not which sign-in methods an account has, not whether a passkey exists, not who asked for a recovery.
6. **No passwords.** Our copy says "password" only in "There’s no password to forget." and in provider names ("Google Password Manager").
7. **Nothing workspace-related.** No workspace names, logos, invites or counts on any page in this area.

### 1.8 Self-review: where the plan read like a default, and what changed

| First draft (reads generic) | Change | Why |
|---|---|---|
| Error and 404 pages with a large illustration and a friendly "Oops" | No illustration. The same 62 × 78 shiba with a badge, and a headline that states the fact ("We couldn’t sign you in") | A 404 illustration is template decoration and shifts the frame. The shiba is already the brand; the badge is the only new idea |
| suiss auth-frame icon disc (`size-16 rounded-full bg-muted`) above every edge-page title | Dropped. The badge sits on the shiba instead | Two marks (disc plus mascot) compete for the same spot, and the disc is the generic suiss look |
| "Sign in with Google" first, above the email field (the common SaaS order) | Email field first, then "or", then passkey, Google, Microsoft | D1 asks for email-first so domain routing can slot in after the email step. The passkey is Richie’s own phishing-resistant method. Google asks only for equal prominence with other third-party options |
| suiss "Or continue with" divider | "or" | Shorter, and two of the three labels below already start with "Continue" |
| A dot progress indicator for recovery (suiss `signup-steps`) | "Step 2 of 3" as text above the title | Three steps; the dots add no information (suiss addendum §4 agrees). It is a real sequence, so a step count is honest |
| Red full-page errors, or a destructive alert for every failure | Neutral badge and a Slate lede. Alarm red only on field errors and destructive buttons | Settings principle 4: red marks the point of no return. A timeout is not one |
| A 48 px hero title at desktop (suiss `md:text-5xl`) | Fixed 30 / 36 | Compared on the rendered 1440 board at 30 and 36 (m, §2 row 3): 36 adds no presence the mascot doesn’t already give, and wraps more on phones |
| Passkey key icon at 16 px beside 20 px Google and Microsoft marks | One 20 px mark box for all three method buttons (`.ri-method`) | (m) On the first render the key looked thinner and smaller than the marks, so the passkey read as the lesser option |
| Status slot reserving 2 to 3 lines on phones (the settings section rule) | 1 line from a 350 px column, 2 below | (m) The first render left a 60 px hole under "Send code" at 320. Every identity slot string measured one line at 358 px (§4.6) |
| Recovery status as a numbered timeline (01, 02, 03) | Plain sentences with a real date and time; an ordered list only where the content is a sequence (received, hold, change) | Numbered markers on non-sequences are a template tell |
| Monospace reference ID | Tabular figures in the UI face, grouped 4-4-4 | No monospace anywhere (AS §1.3) |
| Template tells: ALL-CAPS eyebrows, "A · B · C" meta, "→" on links, one accented headline word | None used | Brief rule; the step line is sentence case and the meta lines are sentences |

---

## 2. Decisions where identity, settings and suiss conflict

"Identity" is `richie.css` and the identity boards. "Settings" is `account.css` and AS. Where how-we-design §2 already recommends, this table confirms or departs, with the reason. **(m)** is measured on this canvas’s reference boards.

| # | Topic | Identity | Settings | suiss | Our choice | Reason |
|---|---|---|---|---|---|---|
| 1 | **Solo-frame alignment** | Centred column; centred mark, `h1`, lede; forms start-aligned (`rc-col`) | Everything start-aligned, mark at the start (`ra-solo`) | Centred column, centred text | **Identity’s centred frame** on settings tokens: `.ri-*` (§6.2) | how-we-design §2. These are identity-app pages; Sign in → any edge page keeps the mascot and title in place ((m) mascot 689,64 and `h1` top 166 on `Main` and `Signedout` at 1440; 164,32 and 134 at 390) |
| 2 | **Mascot asset and size** | SVG at 62 × 78; 93 × 117 at ≥768 w and ≥640 h; 31 × 39 at ≤540 h; `alt="Richie"` | PNG 31 × 39 at every width, `alt=""` | Icon disc | **SVG `/_blob/b4c7…` at 62 × 78 on every identity page; 31 × 39 at ≤540 px tall; 31 × 39 on deletion-lifecycle pages (DL-5)**, `alt=""`, `image-rendering: pixelated`, snapped to whole pixels | The SVG stays crisp at any zoom. 3× existed only for the 48 px title. `alt=""`: the footer names Richie, so the brand isn’t announced before every `h1` (how-we-design §6 item 4) |
| 3 | **Title scale** | 34 / 40 / 48, by viewport height | 30 / 36 fixed | `text-4xl md:text-5xl` (36 / 48), by width | **30 / 36 / 600 at every size** | (m) `Main` at 1440 × 900 rendered at 30 and at 36: 36 moves the form down 4 px and adds no presence the 62 × 78 mascot doesn’t give. how-we-design §2.1: 30 wraps no title at 390 and fewest at 320; 36 wraps one at 390 and six at 320. One size across identity, settings and the shell |
| 4 | **Lede** | 17 / 19 px, max 400, centred, `--2l` reservations | 16, Slate, max 560 | `text-lg` | **16 / 24 Slate, max 400, centred, `text-wrap: balance`, at most 2 lines at 390** | Fixed scale (row 3). 400 keeps a centred lede to a readable two-line block; the `--2l` reservations go with the height-based scale |
| 5 | **Fields** | 52 outlined, radius 12, 17 px | 44 filled + 1 px `--input` edge, radius 10, 16 px | 44 filled, no edge (1.09:1) | **Settings** (`[data-slot=input]`) | how-we-design §2.1: confirmed. (m) `Main` draws a 460 × 44 field over a 460 × 44 button |
| 6 | **Buttons** | Pills ≥240 (280 on passkey pages) above 480 px; full width, 50 tall below | `lg` 44, block, radius 10 in forms | Pills, block radius 8 | **Settings `lg` block at every width**, full column width | One shape at every width, the same width and height as the field above; ends the 240 / 280 split (review C5) |
| 7 | **Status line under the primary** | `rc-status` 1.4 em, mixed roles (two polite regions on Verify) | `.ra-slot` grid cell (`role=status` + `role=alert`), reserved 20 / 40 / 60 by column | none | **`.ri-slot`: the settings slot, centred, reserved 20 from a 350 px column and 40 below** | (m) every identity slot string fits one line at 358 and two at 288 in Inter and SF (§4.6), so the third tier only made a hole |
| 8 | **Where the sign-in methods sit, and their order** | One outline "Sign in with a passkey" under "or" | n/a | `field-separator-with-text`: provider first, then "Or continue with", then email | **Email field → Send code → "or" → Sign in with a passkey → Sign in with Google → Sign in with Microsoft**, stacked, never side by side | D1 (email first; future domain routing replaces the primary’s action after the email step without moving anything). Passkey first among the alternatives: Richie’s own phishing-resistant method, and conditional UI already offers it in the field. Google then Microsoft: alphabetical; neither brand asks for an order. Same on Create account |
| 9 | **Method button style** | Outline pill | Outline `lg` block | Outline block | **Outline `lg` block (`.ri-method`), 20 px mark box, 10 px gap, mark and label centred together**; provider marks are the official multicolour SVGs | Google: "at least as prominently as other third-party sign-in options", the G unaltered on a white, dark or neutral ground (our outline button is `#fff` light and transparent on `#000` dark). Microsoft: "DON’T alter the Microsoft logo"; light or dark scheme allowed. Departures from Google’s own button (Google Sans Medium 14, stroke `#747775`) are listed in §8 Q4 |
| 10 | **"or" divider** | `rc-or`, 13 px | none | `FieldSeparator` text "Or continue with" on `bg-background` | **`.ri-or`: "or", 14 / 20 Slate, `--border` hairlines** | Reads as a word (it tells a screen reader these are alternatives). The suiss label masks the line with `bg-background`, which fails on Mist; ours draws two hairlines |
| 11 | **Dark mode mechanism** | `.dark` wrapper; `.rc--system` media query, used by no board | Tokens on `:root`/`.dark`; `[data-appearance="system"]` under the media query | `.dark` on `<html>` | **Settings mechanism.** Boards: base boards light, `-dark` wrappers. Production identity: always "System" (`data-appearance="system"` on `<html>`, `color-scheme` and `theme-color` metas before first paint) | Before sign-in the OS is the only signal. Same tokens reach imported parts |
| 12 | **Code input: auto-submit** | Review §5 asks to auto-submit on the 8th valid character | AS §4.4: no auto-submit; Enter submits | `verify-code` checks on complete | **No auto-submit anywhere.** Paste or autofill fills all 8 slots; the person presses the commit or Enter | Each code has **4 tries**: an auto-submitted typo spends a quarter of them unseen. Submitting on input is a change of context that WCAG 3.2.2 asks us to warn about. One behaviour for both apps; the cost is one tap |
| 13 | **Code entry geometry and states** | `rc-otp`, slots up to 60 tall, `disabled` when locked, countdown on its own line | `.ra-otp`: 52 (44 below 360 cqi), `readonly` + `aria-disabled` when locked, countdown in the button | 8 × 44 doesn’t fit 360 | **Settings** (AS §7.2 row 20, §4.4), centred in the column | how-we-design rule 11; fixes identity’s 2.09:1 dimmed button and the dropped focus |
| 14 | **Busy** | CSS ring spinner 18 px | suiss 8-spoke 16 px, `aria-disabled`, `data-busy` | 8-spoke `Spinner` | **Settings.** A busy method button swaps its mark for the spinner and reads "Taking you to Google…" | It is the suiss `Spinner` |
| 15 | **Focus on arrival** | `autofocus` on error boards | Outcome `h1` `tabindex=-1`; field in dialogs | none | **Outcome and status pages: the `h1`** (`tabindex="-1"`, no ring). **Form pages: the first field on fine pointers only** (no keyboard pops up on phones). **Pages loaded with an alert: the alert container, labelled by its title and described by its description (B-38)** | how-we-design rule 13; §4.7 |
| 16 | **Footer** | "Richie" (600) + Help, Privacy, Terms, 12 px | Help, Privacy, Terms only | none | **Identity’s** (`.ri-foot`), 24 px targets on fine pointers, 44 on phones and coarse pointers | Names the product where the `h1` doesn’t (review C4) |
| 17 | **Passkey glyph** | Hand-drawn key | Phosphor `key` | Phosphor | **Phosphor `key` (regular) in both apps**, 20 px in method buttons | how-we-design rule 9 asks to decide once. The FIDO "passkey" mark is a filled person-and-key drawing that matches neither the line icons nor the 20 px brand marks, and settings already ships `key` in its nav and rows. Revisit only with a brand review |
| 18 | **Badges** | 30 px disc, ok (`#1e8e3e`), alert (red), neutral (Ink); hand-drawn glyphs | none (DL-5 dropped them) | Green success disc in `verify-*` | **30 px disc, two tones: ok (Leaf) and neutral (Ink); four Phosphor bold glyphs** (§4.9). No red badge | Bold, not regular: a 16 px regular glyph draws 1 px strokes, too thin on a 30 px disc. No red: nothing on these pages is a point of no return |
| 19 | **Alert on an identity page** | none | `.ra-page-alert` between lede and first section (offline) | `Alert` hard-codes `role="alert"` | **suiss `Alert` as the first child of `.ri-body`, only for news present at first paint** (a provider sent you back, your session ended). Focus moves to its title | The page was just loaded, so nothing a person was about to tap moves. Live changes on a page use the status slot instead |
| 20 | **Steps in a flow** | none | `.ra-steps` (one height for dialog steps) | `signup-steps` dots + "Step 1 of 3" | **"Step 2 of 3" text (`.ri-step`) between the mascot and the `h1`**; each step is its own page and URL | Steps are pages here (Back works, links work); dots add nothing for three steps |

### 2.1 Copy conflicts that touch these pages

From how-we-design §3. One decision each; WP3.2 to WP3.5 use these strings.

| Topic | Identity | Settings | **Use** |
|---|---|---|---|
| First send | "Send sign-in code" | "Email me a code" | **"Send code"** (drawn on `Main`) |
| Code instead of a passkey | "Get a code by email instead" | "Email me a code instead" | **"Get a code by email instead"** |
| Code sr label | "Code from your email, 8 letters and numbers" | "Email code, 8 letters and numbers" | **"Email code, 8 letters and numbers"** |
| Code lede | "Enter the code sent to…" | "Enter the code we sent to…" | **"Enter the code we sent to {email}."** |
| Countdown start | 0:59 | 1:00 | **1:00**, from the server’s next-allowed time |
| Locked | "No tries left for this code." + separate timer line + dimmed button | "That was the last try for this code." + countdown in the button | **Settings’** |
| Expired | "This code has expired. Send a new one to continue." | "This code has expired." + **Send a new code** | **Settings’** |
| New code sent | "New code sent. Use the code in the newest email." | "We sent a new code." | **Identity’s**: it says which code to use |
| Send failed | "We couldn’t send a code. Try again in a minute." | The three patterns (AS §4.0) | **The three patterns** |
| Passkey failed | "…or you cancelled it." | "The passkey prompt closed. Try again." | **"The request timed out or you canceled it."** |
| Spelling | "cancelled" | "canceled" | **US: "canceled"** |
| Example address | placeholder "you@example.com" | "name@example.com" | **"name@example.com"** in placeholder and error (drawn on `Main`) |
| Email label and error | Label "Email"; "Enter a full email, like…" | Label "Email address"; "Enter a full email address, like…" | **Label "Email"**; error **"Enter a full email address, like name@example.com."** |
| Headlines | Noun labels ("Passkey created") | Sentences and imperatives | **Imperative for a task page, full sentence for an outcome** ("Sign in to Richie", "You’re signed out") |
| First person in controls | — | "Keep my account" | **"Keep account"**; never "me" or "my" in a control |
| Document title | App title mixed with state | Board title on boards | **App: "{h1} – Richie"**, or the `h1` alone when it already contains "Richie" (B-13); "Error: " prefix after a failed submit. **Boards: the board title** (§5.1) |
| Code commit label (new pages) | "Verify code" | "Confirm", "Change email", "Request download" | **Name what the code completes**: "Link Microsoft" (P6), "Continue" (R4). Verify keeps "Verify code": it lives on the identity canvas and isn’t redrawn here |

---

## 3. IA

### 3.1 Every page in scope

IDs: **A** sign-in and sign-up pages that gain methods, **E** edge pages (WP3.2), **S** settings states (WP3.3), **R** recovery (WP3.4), **P** pages after Google or Microsoft (WP3.5). "Board" names the board that draws it (§5.2); "copy" means the domain spec specifies it without a board (§5.3). Document titles are "{h1} – Richie", except when the `h1` already contains "Richie": then the `h1` alone (B-13); they never contain a person’s name or email (titles stay in history and tab lists).

**Revision 2 sync (B-26).** Every row was compared with its domain spec. A row whose values changed shows the current value and *(updated: spec-x §n)* in its first cell; a row that Revision 1 lacked shows *(added: spec-x §n)*. Where this table and a domain spec disagree, the domain spec’s recorded departure wins (Part B). Unchanged rows were checked and left as they were.

**Reveals** says what the page tells someone who doesn’t own the account. **Limit** names the rate limit, with the numbers in §3.5.

#### Sign in and Create account (base WP3.1, states WP3.5)

| ID | Page or state | Route | Document title | WP | Entry points | Exits (every button and link) | Reveals | Limit |
|---|---|---|---|---|---|---|---|---|
| A1 | **Sign in** (board `Main`) *(updated: spec-sso §3, §3.1; spec-edge §3.5, E3)* | `/signin?returnUrl=…` (IdentityServer `LoginUrl`) | Sign in to Richie (B-13) | 3.1 / 3.5 | Any client’s authorize redirect; **Sign in again** on E1, E13, E14; **Sign in** on E5, E6, R8, R13; **Sign in another way** on A4, P4, P7; **Back to sign in** on R2; E2 (adds `signin_reason=session_ended`, which shows E3); this device’s remembered account shows A4 instead (§8.3 of spec-sso) | **Send code** → Verify (identity canvas), or a field error, or `Signin-deleted`; **Sign in with a passkey** → WebAuthn (discoverable) → return URL; **Sign in with Google / Microsoft** → P1 (a form POST to `/signin/external/challenge`) → provider → callback (return URL, or P2–P8; on the canvas `Main`’s Microsoft button leads to `Provider-link`, B-2); **Create an account** → A2; footer Help, Privacy, Terms | Whether an email has a Richie account (accepted product decision, review "Decided by the product owner") | L1, L2, L6 |
| A2 | **Create account** *(updated: spec-sso §5, §2)* | `/signup?returnUrl=…` (also IdentityServer `CreateAccountUrl`, so `prompt=create` lands here) | Create your Richie account (B-13); "Error: Create your Richie account" after a failed submit | 3.5 | A1 switch line; P7 and its sign-up variant (**Create an account**); A3 **Cancel** when the person began here; P5 sign-up wording **Cancel**; E1 restart of a sign-up (names and email filled) | **Create account** → Verify (sign-up) → passkey offer → return URL; **Sign up with Google / Microsoft** → provider → A3, or P5 (sign-up wording: the email has an account), or signed in silently (the provider account is already linked), or P7 (sign-up variant), or A3 with a code step (unconfirmed email); **Sign in** → A1 (the typed address goes along as `login_hint` after an error) | Whether an email has an account ("This email has a Richie account. Sign in instead.") | L1, L2, L6 |
| A3 | **Finish creating your account** *(updated: spec-sso §6, §6.3)* | `/signup/finish` (needs the external sign-in cookie; without it → A2); code step `/signup/finish/code` | Finish creating your account – Richie; code step: Check your email – Richie | 3.5 | Provider callback in sign-up mode when no Richie account uses the provider’s email (or only one deleted in the last 30 days does); P4 **Sign up with Google** | **Create account** → (code step if the Microsoft email isn’t confirmed, §8 Q5) → return URL, no passkey offer (Q19); **Use a different Google / Microsoft account** (a plain link, `prompt=select_account`; when the unconfirmed email already has an account its label becomes **Sign in with your email instead** → A1); **Cancel** → the start page recorded in the challenge (A2, A1 or A4; external cookie cleared) | Nothing new (the person sees their own provider email) | L6 |
| A4 | **Welcome back, last used Google** (drawn: `Signin-remembered-provider`) *(updated: spec-sso §4, §8.3)* | `/signin` with this device’s remembered-account cookie | Welcome back – Richie | 3.5 | A1 when this device remembers an account whose last method was Google or Microsoft and that provider is still linked (cookie: 90 days, renewed at each sign-in) | **Sign in with Google** → P1 → provider (`login_hint` = the linked address); **Sign in another way** → A1 (account still remembered, email filled); **Not you?** (a form POST to `/signin/forget`) → forgets the account on this device → A1, empty | The remembered name, photo and email, to this device only (as the passkey welcome-back page already does) | L6 |
| A5 | Passkey sign-in, "Can’t use your passkey?" link (delta to the identity canvas’s `Passkey-signin`) *(updated: spec-recovery §7.2, D-R2)* | `/signin` (remembered, passkey) | unchanged | 3.4, copy | Identity `Passkey-signin`, `-failed` | Adds **Can’t use your passkey?** → R1b (`/signin/help?from=passkey&returnUrl=…`) | Nothing | — |
| A6 | Verify, "Can’t get the code?" link (delta to the identity canvas’s `Verify` family; drawn as `Verify-help`, a handoff reference) *(updated: spec-recovery §7.1, D-R13; B-33)* | `/signin/code` | unchanged | 3.4 | Identity `Verify*` (sign-in only; not the sign-up Verify); P6 carries the same link | Adds **Can’t get the code?** → R1 (first of two centred lines, with "Not your email? **Use a different one**."), beside **Use a passkey instead** where shown | Nothing | — |

#### Edge pages (WP3.2)

| ID | Page or state | Route | Document title | WP | Entry points | Exits | Reveals | Limit |
|---|---|---|---|---|---|---|---|---|
| E1 | **Sign-in timed out** (`Timeout`) *(updated: spec-edge E1, §3.6)* | `/signin/expired?returnUrl=…` | Your sign-in timed out – Richie | 3.2 | 20 minutes passed with no activity (the clock slides, B-29): the page navigates here at 0:00 of the time-limit warning, or the next submit on Verify, passkey prompt, A1–A3 or P5–P6 finds the request expired (§8 Q1) | **Start again** → the client’s login start with `login_hint` (A1 with the email filled; a sign-up restart lands on A2 with email and names filled); footer Help | Nothing | — |
| E1a | Timed out while waiting for a passkey | same | same | 3.2, copy | The passkey prompt resolves after expiry | as E1 (lede: "Start again and use your passkey.") | Nothing | — |
| E1b | **Timed out at the provider** (copy; ruled by B-1) *(added: spec-edge E1, D7; spec-sso §2 rows 1–2)* | same | same | 3.2, copy | A provider callback after the request expired, or whose correlation cookie expired or `state` doesn’t match (not E5) | as E1 (lede: "Start again and choose Google." / "…Microsoft.") | Nothing | — |
| E2 | **Signed out while in settings** (`Session-expired`, dialog) *(updated: spec-edge E2)* | dialog over `/account/*` | unchanged | 3.2 | Any settings request returns 401: the device’s sign-in ended (14 days without use, 90 at most, §8 Q2), another device signed this one out (E14), or recovery or deletion ended every session; the app also checks when a tab becomes visible; a settings link callback that finds an ended session | **Sign in again** → `/bff/login?returnUrl={this page}` with `login_hint` and `signin_reason=session_ended` → A1 with E3, then back here with unsaved Profile name fields restored and "We put back what you typed. Save to keep it."; **Go to Richie** → Richie app root (draft kept 30 minutes). Escape and outside clicks do nothing (spec-edge D3) | Nothing | — |
| E3 | Sign in after the session ended (state of A1 with an info alert) *(updated: spec-edge E3, D8)* | `/signin` with `signin_reason=session_ended` | Sign in to Richie (B-13) | 3.2, copy | E2 only; a first visit or a plain deep link while signed out gets plain A1 | as A1 | as A1 | L1, L2 |
| E4 | **Something went wrong, with a reference** (`Error`) *(updated: spec-edge E4, D6)* | `/error?errorId=…` (IdentityServer `ErrorUrl`) and unhandled 500s on identity routes | {h1} – Richie (drawn: "We couldn’t sign you in – Richie"; other `h1`s: "We couldn’t sign you in to Tallybird", "We couldn’t sign you out", "We couldn’t open this page") | 3.2 | Protocol errors (unknown client, redirect URI mismatch, bad request), server errors; the variant follows the route and the client | **Go to Richie** → Richie app root; **Back to {client}** → the client’s `ClientUri` for a third-party client; on other routes **Try again** (reload) and **Go to Richie**; **Copy** (reference); the note’s **contact support** link (new tab, topic `error`, `ref`) and the footer Help carry the reference | Nothing beyond one plain sentence; `error_description` goes to logs only | — |
| E5 | **This link doesn’t work anymore** (`Error-link`) *(updated: spec-edge E5, D7)* | The link’s own route, HTTP 410 for every cause | This link doesn’t work anymore – Richie | 3.2 | Expired, used or tampered email links (recovery status and cancel links, any one-time link). A stale provider callback is E1b, not E5 | Recovery-request links: **Start a recovery request** → R2, **Sign in** → A1, note about status emails. Owner’s cancel link: **Contact support** (new tab, topic `recovery-not-me`), **Sign in**, note about the 30-day undo. Any other one-time link: **Sign in** only. Footer Help | Nothing: one page for expired, used and invalid tokens | L7 (past it, E10) |
| E6 | **Page not found** (`Error-notfound`) | Any unknown identity route, HTTP 404 | Page not found – Richie | 3.2 | Typos, old bookmarks | **Go to Richie**; **Sign in**; Help | Nothing | — |
| E7 | **Offline, inline** (`Signin-offline`) *(updated: spec-edge E7, B-6)* | state of every identity form page (A1, A2, A3, Verify, R2–R5, R12, P5, P6, E11) | unchanged | 3.2 | An action fails while `navigator.onLine` is false, or the browser goes offline on the page | The same button, which tries again; values kept. The line reads "We couldn’t send a code…" for **Send code** and "We couldn’t sign you in…" for the passkey, Google and Microsoft buttons alike ("We couldn’t sign you up…" on A2 and A3) | Nothing | — |
| E8 | **You’re offline, page** (`Offline`) | Served by the identity service worker for any failed navigation (§8 Q16) | You’re offline – Richie | 3.2 | Navigating to an identity page with no connection | **Try again** → reloads the requested URL (also automatically on the `online` event, visibly: "You’re back online. Trying again…") | Nothing | — |
| E9 | **Too many codes, inline** (`Signin-limited`) *(updated: spec-edge E9, B-12, B-15, B-27)* | state of A1, A2, R2, R3, P5 (first sends and lookups) and Verify, R4, P6 (later sends) | unchanged | 3.2 | L1 or L2 reached | Countdown button **Send code in 41:12** (each page’s own commit label; accessible name "Send code, available at 4:53 PM") → **Send code** at 0:00; editing the email clears an L1 countdown; the other ways in stay enabled | Nothing | L1, L2 |
| E10 | **Too many attempts, page** (`Limited`) *(updated: spec-edge E10, B-11, B-15)* | Any identity route, HTTP 429, for navigations; `fetch` submits get E9’s inline pattern | Too many attempts – Richie | 3.2 | L7 or L8 (network-level throttling) | Countdown button from `Retry-After` ("Try again in 12:00", or "Try again at 2:30 PM" from an hour) → **Try again** (reload); Help | Nothing | L7, L8 |
| E11 | **Sign out, confirm** (`Signedout-prompt`) *(updated: spec-edge E11, D2, B-13)* | `/signout?logoutId=…` (IdentityServer `LogoutUrl`) when the prompt is required (no valid `id_token_hint`) | Sign out of Richie? (B-13) | 3.2 | A logout request from an unknown or untrusted client, a typed `/signout` | **Sign out** → E13; **Sign out of every device** → E14; **Stay signed in** → the validated post-logout URI, else the Richie app root | The account chip, to whoever holds this signed-in device | — |
| E12 | Signing out (busy) | state of E11 | unchanged | 3.2, copy | **Sign out** pressed | → E13 or E14 | Nothing | — |
| E13 | **You’re signed out** (`Signedout`; third-party variant `Signedout-client`) *(updated: spec-edge E13, B-33)* | `/signout/done?logoutId=…` (Duende "logged out" page; renders the front-channel sign-out iframes) | You’re signed out – Richie | 3.1 | A trusted sign-out (settings sidebar and mobile index, the Richie app menu: no prompt); E11 | **Sign in again** → A1 (no return URL: lands on the Richie app root); **Back to {client name}** (drawn on `Signedout-client`: only with a validated post-logout URI from another app); **Sign-in and security** (note link) → A1 with `returnUrl=/account/security` | Nothing | — |
| E14 | **Signed out of every device** (`Signedout-everywhere`) *(updated: spec-edge E14)* | `/signout/done?scope=all&logoutId=…` | You’re signed out on every device – Richie | 3.2 | E11 **Sign out of every device** | **Sign in again** → A1; **Back to {client name}** (third-party sign-out, same layout as `Signedout-client`); note link **Connected apps** → A1 with `returnUrl=/account/connected-apps` | Nothing | — |

Time limit warning (B-29, spec-edge §3.6): a state of every timed page (A1–A4, Verify, the passkey sign-in pages, R1–R5, P4–P7). Two minutes before a sliding timer ends, the page’s own status slot says "Still there? This page times out in 2:00." with **Keep going**. At 0:00 the page goes to its "ran out" page (E1 for a sign-in request; R2 or R3 for a recovery draft). It adds no page and no ID.

**Delta to AS §3.3:** "Sign out … lands on identity sign-in" becomes "lands on **You’re signed out** (E13)", whose primary is **Sign in again**. Reason: the Duende signed-out page must render the front-channel sign-out iframes, and on a shared computer the confirmation matters.

#### Settings states (WP3.3; routes are AS’s)

| ID | Page or state | Route | Document title | WP | Entry points | Exits | Reveals | Limit |
|---|---|---|---|---|---|---|---|---|
| S1 | Rename passkey: default, error, saving (`Security-rename`, `Security-rename-error-mobile`) | dialog on `/account/security` | "Error: Sign-in and security – Richie" after a failed submit | 3.3 | **Rename** on a passkey row (no step-up, no email) | **Save** → row updates, "Passkey renamed."; **Cancel** or Escape → focus to **Rename** | — | — |
| S2 | Passkey added, already registered, not supported, at the limit (`Security-passkey-added`, `Security-passkey-limit`) | `/account/security#passkeys` | unchanged | 3.3 | **Add a passkey** outcomes (AS §4.3) | Rename (added; the 10th added moves focus to the new row’s **Rename**); Remove (limit) | — | 10 passkeys |
| S3 | Passkey list didn’t load (`Security-load-error`, `-dark`) | same | unchanged | 3.3 | Load fails | **Try again** | — | — |
| S4 | Remove: last passkey (copy), failed (`Security-remove-failed`) | confirm on `/account/security` | unchanged | 3.3 | **Remove** (step-up follows the confirm) | **Remove passkey** retries; **Cancel** | — | — |
| S5 | Change email: empty, malformed, same address, belongs to another account (`Profile-email-taken`), send failed; new code sent (`Profile-email-code-mobile`); done (`Profile-email-changed`); mobile drawers *(updated: spec-settings §3.1, §10 Q-S4; B-18)* | dialog on `/account/profile#email` | "Error: Profile – Richie" after a failed submit | 3.3 | **Change email** (after step-up); R1’s three methods sign the person in to `/account/profile#email` with Change email open (B-18) | as AS §4.2 | Whether the new address has an account (AS §9 Q4) | L1 for the new address; 10 address checks per account per hour (Q-S4) |
| S6 | Security email to the old address *(updated: spec-settings §3.4, departure 1; B-10)* | email | — | 3.3, copy | Email changed | **This wasn’t me** → the support form (new tab, topic `email-changed-not-me`, signed token good for 30 days). No **Sign in** button: the old address can no longer sign in to the account | — | — |
| S7 | **Google and Microsoft section** (`Security`, drawn by WP3.1) | `/account/security#providers` | unchanged | 3.1 / 3.3 | Settings nav | **Link** → step-up (required, Q20) → busy "Taking you to Microsoft…" → provider (account picker) → `/account/security/link/callback` → back to `#providers` with a status (focus to the section `h2`); **Unlink** → S9 | — | L9 |
| S8 | Linked (`Security-linked`); linked to another Richie account (`Security-link-error`); canceled at the provider; organization hasn’t approved Richie; timed out (older than 20 minutes); provider error; signed out while away; at the limit (copy) *(updated: spec-settings §4.2, B-9)* | `/account/security#providers` | unchanged | 3.3 | Return from the provider | Status in the section slot; **Link** stays available; "signed out while away" goes through Sign in with `signin_reason=session_ended` (E3) and back | Whether that provider account is linked to some other Richie account (accepted: the person proved they own the provider account) | L9 |
| S9 | Unlink confirm (`Security-unlink`) + step-up (copy, AS §4.4 per-action description) | confirm on `/account/security` | unchanged | 3.3 | **Unlink** | **Unlink** → step-up → row shows "Not linked", status "Google unlinked.", focus to that row’s **Link**; **Cancel** | — | — |
| S10 | Step-up before linking (copy) *(updated: spec-settings §4.4; B-25)* | dialog | unchanged | 3.3 | **Link** while step-up is due | AS §4.4 screens A to C; screen C now ends in S11 | — | — |
| S11 | **Step-up screen C, "Can’t get the code?"** (settings canvas `Stepup-cant-get-code`) *(added: spec-recovery §15.6; B-25)* | step-up dialog, then `/recover?from=settings` | unchanged | 3.4, handoff | Screen B’s **Can’t get the code?** during any step-up | **Back**; **Request account recovery** → R2 in the same tab (the action is canceled; the account’s email is prefilled; **Back to Sign-in and security**) | Nothing | as R2 |
| S12 | Security emails for settings changes: passkey added, passkey removed, provider linked (also from sign-in, P6), provider unlinked *(added: spec-settings §5.2; B-7, B-8)* | email | — | 3.3, copy | The change happens | One button each, **Check Sign-in and security** → `/signin?returnUrl=/account/security%23{section}`; "contact Richie support" → the support form (spec-settings §5.2 names the topic "Someone changed how I sign in", which is not in spec-recovery §15.1’s fixed topic list; not settled) | The change, to the account’s own address | — |

#### Recovery (WP3.4)

| ID | Page or state | Route | Document title | WP | Entry points | Exits | Reveals | Limit |
|---|---|---|---|---|---|---|---|---|
| R1 | **Can’t get the code?** (`Recover`) *(updated: spec-recovery §3.1, D-R16; B-18)* | `/signin/help?returnUrl=…` | Can’t get the code? – Richie | 3.4 | A6, P6, A5 (as R1b), footer Help and the Help article (as R1c) | **Back to the code** → Verify as left (code, tries, countdown); **Sign in with a passkey** / **Google** / **Microsoft** → signed in to `/account/profile#email` with Change email open (any sign-in starts the 10-minute step-up window); **Start a recovery request** → R2 (the sign-in’s email carried server-side); "Questions? **Contact support**" (new tab, topic `cant-sign-in`) | **Nothing**: every visitor sees every way in, whatever the account has | L6 on the provider buttons |
| R1a | R1 from linking Microsoft (`?from=link`) *(added: spec-recovery §3.1; COV-03)* | `/signin/help?from=link` | Can’t get the code? – Richie | 3.4, copy | P6 **Can’t get the code?** | **Back to the code** → P6 as left; **Sign in with Microsoft** is hidden (it would loop to P5); passkey and Google as R1 | Nothing | L6 |
| R1b | R1 from passkey sign-in (`?from=passkey`) *(added: spec-recovery §3.1, D-R2)* | `/signin/help?from=passkey` | Can’t use your passkey? – Richie | 3.4, copy | A5 | Primary **Get a code by email instead** → Verify; the rest as R1 | Nothing | L6 |
| R1c | R1 with no sign-in in progress *(added: spec-recovery §3.1)* | `/signin/help` | Can’t get the code? – Richie | 3.4, copy | The Help article, a typed URL | Primary **Back to sign in** → A1; the rest as R1 | Nothing | L6 |
| R2 | Recovery request, step 1: which account (`Recover-account`) *(updated: spec-recovery §3.2, A11Y-09)* | `/recover` (`?from=settings`) | Recover your account, step 1 of 3 – Richie | 3.4 | R1; E5 for recovery links; S11; R9, R10a, R10b **Start a new request** | **Continue** → R3; **Back to sign in** → A1 (from settings: **Back to Sign-in and security**) | Whether the email has an account (as A1), and whether that account has an open request or a wait | L2 (same bucket as A1), L5 per IP |
| R3 | Step 2: a new email you can get (`Recover-email`) *(updated: spec-recovery §3.3, D-R1)* | `/recover/email` | Choose a new email, step 2 of 3 – Richie | 3.4 | R2 | **Send code** → R4; **Back** → R2 (values kept) | Whether the new address already has an account (as AS §9 Q4) | L1 for the new address, L5 per new address |
| R4 | Step 2, the code (`Recover-code`) *(updated: spec-recovery §3.4)* | `/recover/code` | Check your new email, step 2 of 3 – Richie | 3.4 | R3 | **Continue** → R5; **Use a different email** → R3 | — | L3, L4, L1 for sends |
| R5 | Step 3: details for support (`Recover-details`); limited (`Recover-limited`) *(updated: spec-recovery §3.5, D-R1)* | `/recover/details` | Tell us about your account, step 3 of 3 – Richie | 3.4 | R4 | **Send request** → R6; **Back** → R3 (the new email stays confirmed for 30 minutes without activity) | — | L5 |
| R6 | **Under review** (`Recover-review`) *(updated: spec-recovery §3.6, A11Y-10)* | `/recover/status?token=…` (also the page after submit) | We’re reviewing your request – Richie | 3.4 | R5; the "received" email | **Cancel request** (a link to R6a); "Questions? **Contact support**" (new tab, topic `recovery-request`) | This request’s status and both addresses, to whoever has the link | L7 |
| R6a | **Cancel your request?** (requester’s confirm, `Recover-review-cancel`) *(updated: spec-recovery §3.6, revision round)* | `/recover/status/cancel?token=…` | Cancel your request? – Richie | 3.4 | R6, R7 **Cancel request** | **Cancel request** → R10b; **Keep the request** → R6 | as R6 | L7 |
| R6b | **Under review, owner’s view** (`Recover-owner-view`) *(updated: spec-recovery §3.6, D-R10, revision round)* | `/recover/cancel/status?token=…` | We’re reviewing this request – Richie | 3.4 | R12 **Keep the request** | New address masked, no Account row; **Cancel this request** → R12 | The masked new address, to the holder of the old mailbox | L7 |
| R7 | **Approved, on hold** (`Recover-approved`) *(updated: spec-recovery §3.7)* | same as R6 | {h1} – Richie, for example "Your email changes on November 8 – Richie" ("Your email changes today" on the day) | 3.4 | "approved" email | **Cancel request** (→ R6a); "Questions? **Contact support**"; reloads itself to R8 at the change time | as R6 | L7 |
| R7a | **Your email is changing now / We need to check something** (copy, layout `Recover-review`) *(added: spec-recovery §15.5; COV-23)* | same as R6 | {h1} – Richie | 3.4, copy | The change time passed and the job isn’t done (reloads every 30 s for up to 10 minutes), or the final re-check failed | Changing now: none (the page updates itself). Blocked: **Contact support** (topic `recovery-request`); email #15 | as R6 | L7 |
| R8 | **Completed** (`Recover-completed`) *(updated: spec-recovery §3.8, D-R5)* | same as R6 | Your email was changed – Richie | 3.4 | "completed" email; R7 after the hold | **Sign in** → A1 with the new email as `login_hint` and `returnUrl=/account/security` | as R6 | L7 |
| R9 | **Declined** (`Recover-declined`) *(updated: spec-recovery §3.9, D-R7, RIG-01)* | same as R6 | We couldn’t approve your request – Richie | 3.4 | "declined" email (sent to the new and the old address) | **Contact support** (new tab, topic `recovery-request`); **New request on November 12** (unavailable until the 7-day wait ends) then **Start a new request** → R2; note: sign in with a passkey, Google or Microsoft | as R6 | L7 |
| R10a | Canceled by the old address, requester’s view (copy, layout `Recover-declined`) *(updated: spec-recovery §3.9, D-R8)* | same as R6 | This request was canceled – Richie | 3.4, copy | "canceled" email (R12 **Cancel the request** was used) | **Contact support** (topic `recovery-request`); **New request on November 13** (7-day wait) then **Start a new request** → R2 | as R6 | L7 |
| R10b | Canceled by the requester (copy, layout `Recover-declined`, check badge) *(added: spec-recovery §3.9, D-R6)* | same as R6 | You canceled your request – Richie | 3.4, copy | R6a **Cancel request**; "canceled" email | **Start a new request** → R2 (no wait); Contact support | as R6 | L7 |
| R10c | **Expired** (`Recover-expired`) *(added: spec-recovery §3.9, B-36, revision round)* | same as R6 | We couldn’t finish your request in time – Richie | 3.4 | No decision 7 days after submit (B-36); email #16 | **Start a new request** → R2 (no wait); **Contact support** (new tab, topic `recovery-request`); note: sign in with a passkey, Google or Microsoft | as R6 | L7 |
| R11 | **Email to the old address** (`Email-recovery-alert`) *(updated: spec-recovery §4.1 #2 and #4; B-10)* | email | — | 3.4 | Sent at submit and again at approval | **Cancel this request** → R12 ("If this wasn’t you, cancel the request." sits above it). No "This wasn’t me" button here | The new address, masked ("j•••@rivera.studio") | — |
| R12 | **Cancel this recovery request?** (`Recover-cancel`) *(updated: spec-recovery §3.10, D-R10)* | `/recover/cancel?token=…` | Cancel this recovery request? – Richie | 3.4 | R11 | **Cancel the request** → R13; **Keep the request** → R6b. Variants: approved (adds the change time); already canceled or already closed → R13; already completed (up to 30 days) → "We already changed the email" with **Contact support** (topic `recovery-not-me`) | as R11 | L7 |
| R13 | **Canceled, owner’s view** (`Recover-canceled`) *(updated: spec-recovery §3.10, §3.11)* | `/recover/canceled` | We canceled the request – Richie | 3.4 | R12; a used cancel link again; a request that was already closed ("Nothing changed") | **Sign in** → A1 with `returnUrl=/account/security`; Help | — | — |
| R14 | **Recently recovered** in settings (`Security-recovered`) *(updated: spec-recovery §3.12)* | `/account/security` | unchanged | 3.4 | First sign-ins in the 7 days after R8 | Alert links to the passkeys (`#passkeys`) and devices (`#sessions`) sections and to **contact Richie support** (new tab, topic `recovery-not-me`); step-up note changes (copy) | — | — |
| R15 | Recovery emails, 15 in all: received (new), alert (old, R11), approved (new, old), completed (new, old), declined (new, old), canceled by the owner (new, old), canceled by the requester (new, old), undone (old, recovered address), couldn’t finish (new) *(updated: spec-recovery §4.1, §15.5; D-R7)* | email | — | 3.4, copy (R11 drawn) | lifecycle | one button each | as R11 | — |

#### After Google or Microsoft (WP3.5)

| ID | Page or state | Route | Document title | WP | Entry points | Exits | Reveals | Limit |
|---|---|---|---|---|---|---|---|---|
| P1 | **Taking you to Google** (busy; `Signin-redirecting`) *(updated: spec-sso §3.1, COV-14)* | state of A1, A2, A4 and R1 | unchanged | 3.5 | **Sign in with Google / Microsoft** (**Sign up with…** on A2) | A form `POST /signin/external/challenge` (scheme, `returnUrl`, antiforgery), sent with `fetch` when scripts run → provider. A press that fails says "We couldn’t sign you in…" in the slot and the button returns to idle. Plain links "Use a different … account" are a `GET /signin/external/{scheme}?prompt=select_account` | — | L6 (the press doesn’t count; a failed callback does, B-23) |
| P2 | **Canceled at the provider** (`Signin-provider-canceled`) *(updated: spec-sso §3.2, B-13)* | A1 / A2 / A4 with an alert (callback `error=access_denied`; shown once) | Sign in to Richie | 3.5 | Provider returns without an account | A1’s actions; the alert says nothing was shared ("You canceled signing in with Google") | — | L6 |
| P3 | **Provider error** (`Signin-provider-error`, `-dark`) *(updated: spec-sso §3.3, B-9, B-13)* | A1 / A2 / A4 with a destructive alert and a reference | Sign in to Richie | 3.5 | Provider error, timeout (10 s) or invalid response; Microsoft organization approval (title "Your organization hasn’t approved Richie") | A1’s actions; the alert names the other ways in; **Copy** (reference) | — | L6 |
| P4 | **No Richie account uses this Google account** (`Provider-no-account`) *(updated: spec-sso §7.1, COPY-17)* | `/signin/external/no-account` | No Richie account uses this Google account (B-13) | 3.5 | Callback in sign-in mode: no linked account and no account with the provider’s confirmed email | **Sign up with Google** (a link to A3, no second trip to the provider); **Use a different Google account** (plain link, `prompt=select_account`); **Sign in another way** → A1 | Whether the provider’s email has a Richie account (as A1) | L6 |
| P5 | **Link Microsoft to your Richie account** (`Provider-link`); sign-up variant "You already have a Richie account" (copy) *(updated: spec-sso §7.2, B-13)* | `/signin/external/link` | Link Microsoft to your Richie account (B-13); sign-up variant: You already have a Richie account | 3.5 | Callback: no link exists, and the provider’s confirmed email matches an account (sign-in or sign-up) | **Send code** → P6; **Use a passkey instead** → WebAuthn → link → return URL; **Cancel** → the start page (A1, or A2 for the sign-up wording; nothing linked) | Whether the provider’s email has an account (as A1); not its name, photo or methods | L1, L6 |
| P6 | **Link Microsoft, enter the code** (`Provider-link-code`) *(updated: spec-sso §7.3, B-31)* | `/signin/external/link/code` | Check your email – Richie ("Error: " prefix after a failed submit) | 3.5 | P5 | **Link Microsoft** → linked, signed in → return URL (the "provider linked" security email owned by spec-settings §5.2, and two activity entries); **Use a passkey instead**; **Can’t get the code?** → R1a; **Cancel** → A1; proof succeeds but the link can’t be written → P8 | — | L3, L4, L1 |
| P7 | **Microsoft didn’t share a confirmed email** (`Provider-no-email`) *(updated: spec-sso §7.4)* | `/signin/external/no-email` | Microsoft didn’t share a confirmed email – Richie | 3.5 | Callback with no email claim, or one that isn’t confirmed (§8 Q5, sign-in mode), and no linked account | **Sign in with your email instead** → A1; **Use a different Microsoft account** (plain link); switch line **Create an account** → A2. Sign-up variant: **Create an account with your email** → A2 | — | L6 |
| P8 | **Signed in, not linked** (`Provider-not-linked`) *(added: spec-sso §7.5; COV-11, B-33)* | `/signin/external/not-linked` | You’re signed in, but Microsoft isn’t linked – Richie | 3.5 | P6 **Link Microsoft** or P5’s passkey succeeds, but the account already has another account of that provider, or this provider account was just linked to another Richie account | **Continue** → the return URL (Richie app root when it is missing or no longer valid); **Go to Sign-in and security** → `/account/security#providers` | The account’s own linked provider, after proof | — |

### 3.2 IdentityServer mapping

| Duende piece | Setting | Our route | Pages |
|---|---|---|---|
| Login | `UserInteraction.LoginUrl = "/signin"`, `LoginReturnUrlParameter = "returnUrl"` | `/signin` | A1, A4, E3; Verify and passkey sign-in on the identity canvas |
| Interaction context | `IIdentityServerInteractionService.GetAuthorizationContextAsync(returnUrl)` on every identity POST | — | `null` for a return URL that was valid when the page loaded → E1 |
| Logout | `LogoutUrl = "/signout"`, `LogoutIdParameter = "logoutId"`; `GetLogoutContextAsync` → `ShowSignoutPrompt` | `/signout`, `/signout/done` | E11 when the prompt is required; E13, E14. `SignOutIFrameUrl` renders invisibly on E13/E14; `PostLogoutRedirectUri` + `ClientName` give "Back to {client}" (drawn: `Signedout-client`). No automatic redirect after sign-out |
| Error | `ErrorUrl = "/error"`, `ErrorIdParameter = "errorId"`; `GetErrorContextAsync(errorId)` | `/error` | E4. Show one plain sentence; log `Error`, `ErrorDescription`, `RequestId`; the reference comes from the trace ID (§4.5) |
| External login *(updated: spec-sso §2, §3.1, §13.2)* | Challenge: method buttons `POST /signin/external/challenge` (scheme, `returnUrl`, antiforgery; sent with `fetch` and then `location.assign` when scripts run, a `302` without them); plain links `GET /signin/external/{scheme}?prompt=select_account`. `returnUrl`, mode and the start page (A1, A2 or A4) travel in `AuthenticationProperties.Items`. Callback `/signin/external/callback` (server endpoint, no UI) | as listed | Callback outcomes: linked → return URL; E1b; P2, P3, P4, P5, P7, P8; A3 (sign-up mode). Settings linking uses its own callback `/account/security/link/callback`; both schemes are registered once, in the identity host (spec-settings §9.2) |
| Provider identity | Google: `sub` + `email_verified`. Microsoft (`common` endpoint): `oid` + `tid` as the key; `email` is not verified on its own (§8 Q5) | — | Links are stored by provider subject, never by email |
| Consent, device flow, CIBA, native redirect | not used (first-party clients skip consent) | — | out of scope |
| User Management OTP | 8-character Crockford Base32 code, 5 minutes, 4 tries, ≥60 s between sends | — | Verify, R4, P6, S5, A3 code step |

### 3.3 Return-URL handling

1. Every identity page carries `returnUrl` and validates it server-side (`IsValidReturnUrl` or a non-null authorization context) before it redirects. An invalid one is dropped silently and the page behaves as if there was none.
2. **No return URL** means: after sign-in, land on the Richie app root.
3. **Carried through:** A1 → Verify → passkey offer → `Deletion-pending` → return URL; A1 → provider → callback → P5 → P6 → return URL (or P8 → return URL); A2 → A3 → return URL; A1 → P4 → A3 → return URL; R1 → back to Verify (or P6, for `from=link`), the same sign-in. R1’s three sign-in methods use the return URL `/account/profile#email` instead, so Change email opens at once (B-18). A3, P4–P8 keep the return URL server-side with the pending external login; every way back returns to the start page recorded in the challenge (spec-sso §8.2).
4. **Restarted:** E1 re-enters the client’s login start (Richie: the app root, which issues a fresh authorize request) with the email as `login_hint` (a sign-up restart adds `prompt=create` and fills the names). The old return URL can’t be resumed once its request expired.
5. **Dropped:** recovery (R2 onwards) and every email-link page: they are out of band and can outlive any sign-in. R8 goes to A1 with the new email as `login_hint` and `returnUrl=/account/security`; R13 goes with `returnUrl=/account/security` only (spec-recovery D-R5).
6. **Settings session end (E2):** `returnUrl` = the settings page (`/account/security`), plus `login_hint`. Unsaved field values are kept in `sessionStorage` under the page’s key for 30 minutes and restored after sign-in; open dialogs aren’t reopened.
7. A return URL is never shown in copy. Where a page names the destination it uses the client’s display name ("Back to Tallybird").

### 3.4 Journeys walked for dead ends

Revision 2: journeys whose steps changed show *(updated: source)*; new ones show *(added: source)*.

| # | Journey | Path | Way out at each step |
|---|---|---|---|
| J1 | New person signs up with Google *(updated: spec-sso §7.1, §6.1)* | A1 → Sign in with Google → P1 → Google → P4 → **Sign up with Google** → A3 → Create account → return URL | P4: Use a different Google account, Sign in another way; A3: Use a different Google account, Cancel (back to the start page) |
| J2 | Existing person, first time with Microsoft *(updated: spec-sso §7.2, §7.3, §8.5; spec-settings §5.2)* | A1 → Sign in with Microsoft → P1 → Microsoft → P5 → Send code → P6 → Link Microsoft → return URL; the "provider linked" security email; activity entries "Linked Microsoft" and "Signed in with Microsoft" | P5: Use a passkey instead, Cancel; P6: Send a new code, Use a passkey instead, Can’t get the code? (→ R1a), Cancel |
| J2b | Same, but the link can’t be written *(added: spec-sso §7.5)* | … → P6 → Link Microsoft → P8 (signed in, not linked) → Continue → return URL, or Go to Sign-in and security → unlink the old account → Link (J17) | P8: both buttons |
| J3 | Create account with a provider whose email has an account | A2 → Google → P5 (sign-up wording) → J2 | as J2 |
| J4 | Microsoft gives no confirmed email *(updated: spec-sso §7.4)* | A1 → Microsoft → P7 → Sign in with your email instead → A1 → Verify → in; link Microsoft later in settings (S7: signed in, so the email claim doesn’t matter) | P7: Use a different Microsoft account, Create an account |
| J5 | Sign-in times out on Verify *(updated: spec-edge §3.6, E1; B-29)* | Verify, idle → "Still there? This page times out in 2:00." → **Keep going** renews it (up to 10 times); otherwise → E1 → Start again → A1 with the email → Send code → Verify | E1: Help |
| J5b | Provider round trip takes too long *(added: B-1; spec-edge E1b)* | Provider callback after the request expired or its cookie expired → E1b → Start again → A1 → choose the provider again | E1b: Help |
| J6 | Lost mailbox, has a passkey *(updated: spec-recovery §2.2, D-R16; B-18)* | Verify → Can’t get the code? → R1 → Sign in with a passkey → signed in to Profile with Change email open (the sign-in starts the 10-minute step-up window) → new address → code → done | R1: Back to the code |
| J7 | Lost mailbox, Google linked *(updated: B-18)* | R1 → Sign in with Google → in, returned to `/account/profile#email` with Change email open; the sign-in starts the 10-minute window, so no step-up is asked. After the window, sign in with Google again | as J6 |
| J8 | Lost mailbox, nothing else *(updated: spec-recovery §2.1, §2.3, B-10)* | R1 → Start a recovery request → R2 → R3 → R4 → R5 → R6; old address emailed (R11, **Cancel this request**) → approved → R7 (hold) → R8, every device signed out → Sign in with the new email (returns to Security) → R14 | Every step: Back; R6/R7: Cancel request (J23), Contact support; R9: Contact support, New request on the date shown |
| J9 | The owner gets the alert *(updated: spec-recovery §3.10, D-R10; B-10)* | R11 → Cancel this request → R12 → Cancel the request → R13 → Sign in → Security | R12: Keep the request (→ R6b, the owner’s view, where Cancel request returns to R12) |
| J10 | The owner misses the alert *(updated: spec-recovery §4.1 #6, §3.10; B-10)* | Hold ends → email changed. The "completed" email to the **old** address says what happened and links **Contact support** (topic `recovery-not-me`, the dispute token attached, good for 30 days) | **Dead end found and closed:** without that link a real owner who missed the hold had no route back. Support can undo it for 30 days (§8 Q13); R12 "already changed" offers the same |
| J11 | Signed out while saving in settings *(updated: spec-edge E2, E3)* | Profile → Save → 401 → E2 → Sign in again → A1 with E3 ("You were signed out", email filled) → Verify → back to Profile, values restored unsaved with "We put back what you typed. Save to keep it." | E2: Go to Richie |
| J12 | Sign out from settings | Sidebar Sign out → E13 → Sign in again; or the note link → A1 → Security | — |
| J13 | Untrusted logout link *(updated: spec-edge E11, D2)* | `/signout` → E11 → **Stay signed in** → client or Richie root | — |
| J14 | Bad client configuration *(updated: spec-edge E4, D6)* | authorize → E4 → Go to Richie (Back to {client} for a third-party client), or contact support with the reference | — |
| J15 | Offline | A1 Send code offline → E7 line, values kept → back online → Send code. Page load offline → E8 → Try again (automatic when back online, visibly) | — |
| J16 | Throttled *(updated: spec-edge E9, E10)* | A navigation → E10 → countdown → Try again; a form submit gets E9’s inline countdown instead | Help (E10); the other ways in stay enabled (E9) |
| J17 | Link Microsoft in settings *(updated: spec-settings §4.2)* | Security → Link → step-up → busy → Microsoft (account picker) → back → "Microsoft linked. You can now sign in with it." Already linked elsewhere → S8 error with what to do. Canceled → "You canceled linking Microsoft. Nothing changed." | Cancel at every step |
| J18 | Unlink Google | Unlink → confirm → step-up → "Google unlinked." | Cancel |
| J19 | Remembered Google account *(updated: spec-sso §4)* | A4 → Sign in with Google → in; Sign in another way → A1; Not you? (form POST) → forgotten → A1 | — |
| J20 | Expired recovery link *(updated: spec-edge E5)* | E5 → Start a recovery request → R2, or Sign in. The owner’s expired cancel link: E5 → Contact support | Help |
| J21 | Deleted account (days 30 to 60) signs in with a provider | callback → the provider email matches an account deleted in the last 30 days → `Signin-deleted` (AS §4.8) | as `Signin-deleted` |
| J22 | Lost mailbox, signed in (settings step-up) *(added: spec-recovery §15.6; B-25)* | Step-up screen B → Can’t get the code? → screen C → Request account recovery → R2 with the account’s email filled (the action is canceled) → J8 | Screen C: Back; R2: Back to Sign-in and security |
| J23 | Requester cancels *(added: spec-recovery §3.6, D-R6)* | R6 or R7 → Cancel request → R6a → Cancel request → R10b → Start a new request → R2 | R6a: Keep the request; no wait after your own cancel |

Other gaps closed in the plan: E2 needs an exit that isn’t sign-in (**Go to Richie**); E13 needs "Back to {client}" for third-party sign-outs (drawn on `Signedout-client`); a Microsoft email that isn’t confirmed needs a code step on A3 rather than a refusal (§8 Q5); a passkey on another device works from R1 through the browser’s cross-device prompt (R1 says "Passkey on your phone? Choose it when your browser asks."); a sign-in request that runs out is warned about two minutes ahead and can be renewed (B-29).

### 3.5 Rate limits and lifetimes (numbers are defaults engineering confirms)

#### Limits L1 to L9

| # | Limit | Default | Shown as |
|---|---|---|---|
| L1 | Code sends per address | 1 per 60 s (Duende), **5 per hour** | **Send a new code in 1:00**; after the 5th: **Send code in 41:12** (m:ss from the server’s next-allowed time, B-15; accessible name "Send code, available at 4:53 PM", B-27) and a status line that says the count and the wait once: "You’ve asked for 5 codes in an hour. Try again in 41:12." (B-34; spec-edge E9) |
| L2 | Email lookups per IP (A1, A2, R2) | **30 per hour** | Same countdown in the commit button (E9); slot "This network has made 30 attempts in an hour." (B-34; with its countdown it is 2 lines at 358, so the countdown stays on the commit, spec-edge E9) |
| L3 | Tries per code | 4 | AS §4.4 locked state |
| L4 | Code lifetime | 5 minutes | AS §4.4 expired state |
| L5 | Recovery requests | **1 open per account; 3 per new address per 24 h; 5 per IP per 24 h** | R5 limited state (`Recover-limited`): "This network has made 5 recovery requests today."; on R3 "This email has been used for 3 recovery requests today." (B-12) |
| L6 | Provider sign-ins and account creations per IP | **30 per hour**, shared with L2; **only failed provider sign-ins count** (B-23) | An A1 alert "Too many attempts" with the count and the wait, and countdown method buttons ("Sign in with Google in 41:12") |
| L7 | Token lookups (status, cancel, one-time links) per IP | **30 per hour** | E10 |
| L8 | Gateway throttling | engineering’s threshold; `Retry-After` always sent | E10 countdown |
| L9 | Linking attempts per account | **5 per hour** | S8 status line and **Link in 41:12** |

#### Limits and lifetimes (B-26: every number in the four domain specs and Part B, with its source)

Written 2026-11-05 from `spec-edge.md`, `spec-settings.md`, `spec-recovery.md`, `spec-sso.md` and Part B. A number appears here once; the pages say it in their own copy. All are defaults engineering and support confirm (Part A §8; spec-recovery Q-R11). Changing one is a text swap. "Sliding" means any request that carries the timed state renews it (B-29).

| Group | Thing | Number | Notes | Source |
|---|---|---|---|---|
| Sign-in request | Sign-in request (the pending sign-in: email, method, client) | **20 minutes, sliding** | Any GET, POST or the typing keep-alive renews it. Runs out into E1 (E1b at a provider callback). The external sign-in cookie and the server-side pending external login (used by A3, P4–P8) live and slide the same 20 minutes | Part A §8 Q1, P-4; B-29; spec-edge §3.6, §4 E1; spec-sso OQ-10, §13.2 |
| Sign-in request | Time limit warning | **2 minutes** before the end; **Keep going** renews up to **10 times**; minimum 20 s to respond | The 5-minute email code is not covered (essential) | B-29; spec-edge §3.6 |
| Sign-in request | Keep-alive request | at most **once a minute** while a person types | Also used by the recovery pages | spec-edge §3.6, QE-13 |
| Sign-in request | Settings link request (Link Microsoft / Google) | **20 minutes** | Older than that: "Linking Microsoft timed out. Nothing changed." | spec-settings §4.2, §9.2 |
| Sign-in request | Provider callback wait | **10 seconds** | Then P3 | spec-sso §2 row 5, OQ-9 |
| Codes | Email code life | **5 minutes** | The authentication factor’s own lifetime | spec-recovery §2.4; spec-settings §3.2; spec-sso §7.3; Part A §3.2 |
| Codes | Tries per code | **4** | "3 tries left", "2", "1 try left", then locked | spec-settings §3.2; spec-recovery §3.4; spec-sso §7.3 |
| Codes | Sends | **1 per 60 s**, **5 per hour per address** (L1) | The resend countdown is the only 1:00 start (B-15). A send within 60 s of the last goes straight to the code page | Part A §3.5; spec-edge §4 E9; spec-recovery §3.3; spec-settings §3.1 |
| Codes | Code shape | 8 characters, Crockford Base32 | No auto-submit | Part A §2 row 12; spec-sso §7.3 |
| Network and address limits | Email lookups per network (L2) | **30 per hour** | Shared bucket with A1, A2, R2 | Part A §3.5; spec-edge §4 E9; spec-recovery §3.2 |
| Network and address limits | Provider sign-ins and account creations per network (L6) | **30 per hour**, **failures only** | A successful provider sign-in never counts, so an office at a sign-in rush isn’t throttled (B-23). Shares L2’s bucket. The press doesn’t count; arriving at the callback with a failure does | B-23; spec-sso §2 row 6, §3.1, §3.5 |
| Network and address limits | Token lookups per network (L7) | **30 per hour** | Status, cancel and one-time links; past it, E10 | Part A §3.5; spec-edge §4 E5; spec-recovery §2.4 |
| Network and address limits | Gateway throttling (L8) | engineering’s threshold | `Retry-After` always sent | Part A §8 Q15; spec-edge §4 E10 |
| Network and address limits | Linking attempts per account (L9) | **5 per hour** | The 6th shows **Link in 41:12** | spec-settings §4.2 |
| Network and address limits | Change-email address checks per account | **10 per hour** | "That email already belongs to another Richie account" is an existence check; the commit counts down; slot "You’ve tried 10 addresses in an hour." | spec-settings §10 Q-S4 |
| Network and address limits | Countdown display | m:ss up to **59:59**; from an hour, a clock time ("Try again at 2:30 PM"); "tomorrow at 9:10 AM" when not today | Starts at `Retry-After` or the server’s next-allowed time, never at 1:00 except the resend | B-15; spec-edge §3.4, §4 E10 |
| Recovery | Recovery draft (R2–R5) | **30 minutes, sliding** | Warning 2 minutes before; runs out into R2 with "This took too long" | B-29; spec-recovery §2.4, §3.2, §15.3 |
| Recovery | New-address confirmation (after R4) | **30 minutes, sliding** | Runs out into R3 with "Confirm your new email again" | B-29; spec-recovery §2.4, §3.5, §15.3 |
| Recovery | Recovery requests (L5) | **1 open per account; 3 per new address per 24 h; 5 per IP per 24 h** | The second open request is refused at R2 | Part A §3.5; spec-recovery §2.4, §3.2, §3.3, §3.5 |
| Recovery | Request open time | "open for up to **7 days**" | Appears only in R2’s open-request error; spec-recovery §2.4 has no lifetime row for an open request | spec-recovery §3.2 (COV-05) |
| Recovery | Reply time | **2 business days** | "Richie support usually replies within 2 business days." verbatim on R1, R2, R6, email #1 and the support confirmation | Part A §8 Q9, P-5; B-19; spec-recovery §1.1, §15.1, Q-R11 |
| Recovery | The hold | **72 hours from approval** | Server time; shown as a date and time, said "3 days" in copy. The page reloads once at the change time + 5 s | Part A §8 Q8, P-5; D2; spec-recovery §1.1, §2.4, §2.5 |
| Recovery | Completion running late | page reloads every **30 s** for up to **10 minutes**, then the blocked page | | spec-recovery §15.5 |
| Recovery | Wait after a decline or an owner cancel | **7 days** | Support can lift it. No wait after the requester cancels | Part A §8 Q12; spec-recovery §1.1, D-R8, §2.4 |
| Recovery | Recently recovered | **7 days** from completion | Step-up for every sensitive action; R14 alert | D2; spec-recovery §2.4, §3.12 |
| Recovery | Support undo window | **30 days** from completion | The completed email’s **Contact support** and R12 "already changed" work for the same 30 days | Part A §8 Q13; spec-recovery §1.1, §2.4; spec-edge §4 E5; spec-settings §3.4 |
| Tokens | Status and cancel tokens | until **30 days** after the request closes | 32 random bytes, stored hashed; not used up, they expire; wrong or expired gives E5 (HTTP 410) | spec-recovery §2.4, §15.4 |
| Tokens | Dispute token | **30 days** after completion | Opens a `recovery-not-me` case | spec-recovery §2.4, §15.4 |
| Tokens | "This wasn’t me" token on the changed-email message (S6) | **30 days** | Signed token naming the account and the change | spec-settings §3.4, §9.2 |
| Tokens | Recovery draft cookie | **30 minutes, sliding** | `HttpOnly`, `SameSite=Lax` | spec-recovery §15.4 |
| Tokens | Recovery answers (R5) | deleted **30 days** after the request closes | | spec-recovery Q-R2 |
| Devices | Remembered account on this device | **90 days, renewed at each sign-in** | One account per device; shown as A4 or the passkey welcome back | spec-sso §8.3, OQ-4 |
| Devices | Signed-in device | **14 days without use**, 90 days at most | Ends into E2; recovery and deletion end every session at once | Part A §8 Q2; spec-edge §3.6, §4 E2 |
| Devices | Unsaved settings draft (E2) | **30 minutes** in `sessionStorage` | Page-level fields only | Part A §3.3 rule 6; spec-edge §4 E2 |
| Step-up | Step-up window | **10 minutes** after the last sign-in or step-up | Any successful sign-in starts it, Google and Microsoft included (B-18); always required for 7 days after a recovery | B-18; spec-settings §1.1, §9.2, Q-S1; spec-sso OQ-2 |
| Passkeys | Passkeys per account | **10** | Names up to 40 characters | spec-settings §2.1, §2.2; Part A §3.1 S2 |
| Deleted accounts | Windows the sign-in and recovery copy depend on | days **0 to 30** after a deletion request (scheduled: sign-in works with `Deletion-pending`, recovery allowed); days **30 to 60** (deleted: `Signin-deleted`, data downloadable); after day **60** "No Richie account uses this email" | Cited from AS §4.8; not set here | spec-recovery §3.2; spec-sso §2 rows 7a, 8a |
| Field lengths | Recovery details | name **100**, devices **200**, last sign-in **100**, created **100**, anything else **1,000** characters; the support form’s message **2,000** | | spec-recovery §3.5; §15.1 |

**Wording note.** The inline limit lines are B-34’s: "You’ve asked for 5 codes in an hour." (L1) and "This network has made 30 attempts in an hour." (L2, L6), each followed by the countdown or clock time where it fits (spec-edge E9, §7).

---

## 4. Shared grammar for identity pages

### 4.1 Frame anatomy

Measured on `Main` and `Signedout` (m). The page is `<div class="ra ri">` → `<main class="ri-main">` → `<div class="ri-col">` (head, body, switch) and `<footer class="ri-foot">`.

**Form page at 1440 × 900** (`Main`):

```
y   0 ┌──────────────────────────────── 1440 ───────────────────────────────┐
   64 │                         [shiba 62×78 @ x 689]                       │
  166 │                          Sign in to Richie              (h1 30/36)  │
  210 │                   Enter your email to get a code.     (lede, Slate) │
  266 │             Email                                  (label 16/600)   │
  298 │             [ name@example.com                         ] 460 × 44   │
  366 │             [               Send code                  ] 460 × 44   │
  418 │                       (status slot, 20 reserved)                    │
  438 │             ───────────────────── or ─────────────────────          │
  478 │             [ (key)   Sign in with a passkey           ]            │
  534 │             [ (G)     Sign in with Google             ]            │
  590 │             [ (MS)    Sign in with Microsoft          ]            │
  666 │                   New to Richie?  Create an account     (14 Slate)  │
  852 │                     Richie   Help   Privacy   Terms         (12)    │
  900 └─────────────────────────────────────────────────────────────────────┘
         column 460 at x 490 (whole pixel); every control is full column width;
         footer box 804–900
```

**Form page at 390 × 844** (`Main-mobile`):

```
┌──────────── 390 ────────────┐
│ 32   [shiba]   x 164        │
│ 134  Sign in to Richie      │  h1
│ 178  Enter your email to…   │  lede, 1 line
│ 234  Email                  │
│ 266  [name@example.com     ]│  358 × 44
│ 334  [      Send code      ]│
│ 386    (slot, 20; 40 <350)  │
│ 406  ─────── or ───────     │
│ 446  [ Sign in with a passk]│  three method buttons, 12 apart
│ 502  [ Sign in with Google]│
│ 558  [ Continue with Micros]│
│ 634  New to Richie? Create… │
│ 776  Richie Help Privacy …  │  footer box 728–844
└─────────────────────────────┘
```

**Outcome page at 1440 × 900** (`Signedout`): the same column and head as the form page (mascot 689,64; `h1` 166; lede 210, two lines), then the primary at **290** and the note at 358. Only the body changes between the two shapes.

**Outcome page at 390 × 844** (`Signedout-mobile`):

```
┌──────────── 390 ────────────┐
│ 32   [shiba]✓  x 164        │  badge 30 px on the shoulder, 3 px Paper ring
│ 134  You’re signed out      │  h1, focused on arrival (no ring)
│ 178  We signed you out of   │  lede, 2 lines (≤ 2 allowed), max 400
│      Richie on this device… │
│ 258  [    Sign in again    ]│  the one primary, 358 × 44
│ 326  Lost a device? Sign in,│  note, 14 Slate, centred, link underlined
│      then sign it out in …  │
│                             │
│ 776  Richie Help Privacy …  │  footer box 728–844, 44 px targets on phones
└─────────────────────────────┘
   16 px gutters
```

**Status page** (recovery status, R6 to R9; drawn by WP3.4):

```
┌──────────────── 460 column ────────────────┐
│               [shiba]◷                     │  clock badge: waiting
│    Your email changes on November 8        │  h1 carries the date
│   We’ll change it to j…@rivera.studio at   │  lede: the time, and what you can do
│   4:10 PM unless you cancel.               │
│ ┌─ Mist inset group (AS §7.2 row 7) ─────┐ │  facts as rows, start-aligned:
│ │ Requested   November 5 at 10:42 AM     │ │  title + description, no actions
│ │ Approved    November 5 at 4:10 PM      │ │
│ │ Changes     November 8 at 4:10 PM      │ │
│ └────────────────────────────────────────┘ │
│ [             Cancel request             ] │  outline (a commit here would be
│        Questions? Contact support.         │  destructive in tone, not filled)
└────────────────────────────────────────────┘
```

**Spacing (one scale, in `edges.css`):** main top padding 64 (≥768) / 32 / 16 (≤540 tall); mascot → step line or `h1` 24; step line → `h1` 4; `h1` → lede 8; head → body 32; fields 20 apart; field → commit 24; primary → slot 8; slot → "or" 0, "or" → methods 20; methods 12 apart; body → switch 32; note 24 below actions; footer 48 above, 24 below. **Short screens (≤540 tall):** mascot 31 × 39, badge 20 with a 12 px glyph, head gaps 12, body 24. Type and controls never change size.

**Why the reserved space looks empty.** The 20 px under **Send code** (40 on 375 and 320 phones) is the status slot. In a clean state it is empty on purpose: it is the room "Sending code…", a failure or a countdown note needs, so a message never pushes the alternatives down while someone is about to tap one (AS §1.5 rule 8). Don’t collapse it.

### 4.2 Delta to the settings grammar (AS §4.0)

**Reused unchanged** (cite AS, don’t restate): the buttons table and the busy and unavailable rules; copy rules; the three failure patterns and the phrase table (new phrases below); screen-reader rules; the long-email rule; dialogs, confirms, toasts and drawers on settings pages; code-entry rules (AS §4.4) and the step-up screens; loading skeletons on settings pages.

**Different on identity pages:**

| Settings (AS §4.0) | Identity pages (this area) |
|---|---|
| Page header: `h1` + lede start-aligned, 32 to the first section | `.ri-head`: mascot, optional step line, `h1`, lede, centred; 32 to the body |
| Sections with `h2`, inset groups and section foots | No sections. One body: a form, an action stack, or a Mist summary group (status pages) |
| Status slot in a section foot, start-aligned, 20 / 40 / 60 | `.ri-slot` under the one primary, centred, 20 / 40 (§4.6) |
| Offline: `Alert` between lede and first section | Offline: the status slot on the page in use (E7); a full page only when a page can’t load (E8) |
| Toasts for things that left the screen | **No toasts on identity pages.** Every outcome is the page itself or the status slot |
| Skip link on shell pages | None: one `main`, content first |
| Shiba 1× on solo pages (DL-5) | Shiba 62 × 78 (1× on deletion-lifecycle pages and ≤540 tall) |

**New failure phrases** (for the AS §4.0 patterns; Revision 2: B-6, spec-recovery D-R4, spec-settings §1.3):

| Page | Action | {phrase} | Where |
|---|---|---|---|
| A1, A2, R3, P5 | Send a code | send a code | `.ri-slot` |
| A1, A4, R1, E7, P5 | Sign in with the passkey, Google or Microsoft button | sign you in (one phrase for every method on a sign-in surface; "reach" is retired, B-6) | `.ri-slot` |
| A2, A3 | Sign up with a provider, or Create account | sign you up | `.ri-slot` |
| R2 | Continue | continue | `.ri-slot` |
| R5 | Send the request | send it (screen readers hear "your request") | `.ri-slot` |
| R6a | Cancel the request (requester) | cancel it (sr "your request") | `.ri-slot` of the confirm |
| R12 | Cancel the request (owner) | cancel it (sr "the request") | `.ri-slot` |
| E11 | Sign out | sign you out | `.ri-slot` |
| Timed pages | Keep going | add more time | `.ri-slot` |
| S7 | Link (before the redirect, and after the return) | link Microsoft / link Google (the button’s verb, B-6) | section status slot |
| S9 | Unlink | unlink Google | inside the confirm, or inside step-up when it ran |

### 4.3 One name per thing: additions to AS §4.0 and how-we-design §3

| Thing | Use in copy | Not |
|---|---|---|
| Signing in with Google or Microsoft | Buttons **"Sign in with Google"**, **"Sign in with Microsoft"** (on Create account: **"Sign up with Google"**, **"Sign up with Microsoft"**). Busy: **"Taking you to Google…"**. Failure: **"We couldn’t sign you in…"** for the passkey, Google and Microsoft buttons alike, **"We couldn’t sign you up…"** on Create account (B-6). In settings: the section **"Google and Microsoft"**, the verbs **link** / **unlink**, the state **"Linked"** / **"Not linked"**, the thing **"your Google account"**, **"your Microsoft account"** (Microsoft: "a work, school or personal Microsoft account") | connect, disconnect, connected (taken: a **connected app** is an OAuth grant, AS §4.6); SSO; social login; federated; external provider; identity provider; "reach Google" (retired, B-6); "Microsoft 365 admin" (B-9: "your organization", "your IT admin") |
| Company SSO (future) | **SSO** is reserved for it ("Continue with SSO" after the email step) | SSO for Google or Microsoft |
| The IdentityServer login request | In copy: **"your sign-in"** ("Your sign-in timed out"). In specs: **sign-in request** | login request, session, flow, authorization, transaction |
| A signed-in browser ending | Device (AS); its end: **"signed out"** ("You were signed out", "We signed you out on every device") | session expired, logged out, timed out (for a signed-in device) |
| Signing out everywhere | Action **"Sign out of every device"**; outcome **"We signed you out on every device."** (as `Data-deleted`) | sign out everywhere, log out all sessions |
| Lost-email help | **"Can’t get the code?"** (link, both apps); **recovery request** (noun); **"Start a recovery request"**; support **reviews**, **approves**, **declines**; the requester’s button is **"Cancel request"**, the old address’s email button **"Cancel this request"** (with "If this wasn’t you, cancel the request." above it) and its page **"Cancel the request"**; **"This wasn’t me"** stays only on the changed-email message (S6), where it goes to support (B-10); the wait is said as a date and time ("We’ll change your email on November 8 at 4:10 PM"); reply time "Richie support usually replies within 2 business days." | ticket, case, appeal, claim, reset, "hold" in copy |
| Recently recovered | **"Your email was changed by account recovery on November 8."** | recovered account, restored |
| An error’s reference | **reference** ("Reference 9F2C-41AB-7D03", button **Copy**, sr "Copy reference") | error code, trace ID, correlation ID, request ID |
| Too many attempts | Say the count in B-34’s lines: **"You’ve asked for 5 codes in an hour."** (L1) and **"This network has made 30 attempts in an hour."** (L2, L6), then the countdown or clock time; other counts keep the B-12 pattern **"This network has made {n} {things} in {window}."** / **"This email has been used for {n} recovery requests today."**; the page title is **"Too many attempts"** (B-11). **"Request"** means only the recovery request in this area; a sign-in is a **sign-in request** in specs and **your sign-in** in copy | rate limit, throttled, 429, blocked; "Too many requests" as a title |
| Go to / Back to Richie | **"Back to Richie"** where the person came from the app (settings sidebar, mobile index); **"Go to Richie"** on edge and signed-out pages (B-14); **"Back to {client name}"** for a third-party client | "Return to Richie", "Home" |
| Security emails | One fine print: "You’re getting this because it’s a security change on your Richie account. We always send these."; the "provider linked" button is **"Check Sign-in and security"**; every email names the time zone (the account’s, else UTC) and uses an absolute date, never "today" (B-7, B-8) | "Go to settings", "today" in an email |
| Offline | **"You’re offline"** | no connection, network error, disconnected |
| Help destinations | **Help** (footer link), **Contact support** (button), **Richie support** (prose) | help desk, customer service, support team |

### 4.4 Signed-out copy patterns

| Situation | Pattern | Where |
|---|---|---|
| An action fails | The AS §4.0 three patterns with the §4.2 phrase: "We couldn’t send a code because you’re offline." / "… Check your connection." / "… Try again in a moment." On a sign-in surface the passkey, Google and Microsoft buttons all say "We couldn’t sign you in…" (B-6) | `.ri-slot`, `p[role=alert]` (focus stays on the button) |
| The browser goes offline on a form page | Quiet Slate line, once: "You’re offline. We’ll keep what you typed." Buttons stay enabled; pressing one gives the offline failure line | `.ri-slot`, `role=status` |
| Back online | "You’re back online." for 5 s, then the slot empties | `.ri-slot`, `role=status` |
| A page can’t load (E8) | `h1` "You’re offline"; lede says what to check and that we’ll retry; **Try again** (busy "Trying again…"); still offline: "You’re still offline." | E8 |
| Too many codes or lookups (E9) | Commit becomes unavailable **"Send code in 41:12"** (`aria-disabled`, outline look, Slate, tabular; the digits are `aria-hidden` and the accessible name is "Send code, available at 4:53 PM", B-27); the slot says why once, in B-34’s lines: "You’ve asked for 5 codes in an hour. Try again in 41:12." / "This network has made 30 attempts in an hour." (its countdown stays on the commit: with it the line is 2 lines at 358); at 0:00 the button returns and the slot says "You can send a code now." The countdown starts at the server’s next-allowed time (B-15) | E9 |
| Throttled page (E10) | The `h1` is **"Too many attempts"**; the lede says when to try again in whole minutes rounded up ("We’ve paused activity from your network for now. Try again in 12 minutes."); the button counts down in m:ss from `Retry-After` (up to 59:59; from an hour, "Try again at 2:30 PM") and becomes **Try again** | E10 |
| Something expired | Say what expired, after how long, and what is kept: lede "Start again with jamie.rivera@northwind.coffee to get a new code." and note "A sign-in times out after 20 minutes without activity, so the code we sent earlier won’t work anymore." | E1, E5 |
| A timed state is about to end | Two minutes before a sliding timer ends: "Still there? This page times out in 2:00." with a plain **Keep going** in the slot (up to 10 times); after it, "You have 20 more minutes." (B-29) | spec-edge §3.6 |
| A limit in a sentence | "This network has made 5 recovery requests today." (R5), "This email has been used for 3 recovery requests today." (R3 field error only), with the clock time after it (B-12, B-15) | R3, R5 |
| Microsoft needs organization approval | Title "Your organization hasn’t approved Richie"; next step "Ask your IT admin to allow Richie, or use your email or a passkey." on sign-in, "…or link a personal Microsoft account." in settings (B-9) | P3, S8 |
| A busy state | The button reads its gerund; the same text is written once into the slot’s polite paragraph, sr-only (B-28) | every page |
| Something failed that we can’t explain | One plain sentence, then the reference (§4.5) and **Go to Richie** | E4 |

### 4.5 Error reference ID

- **What:** the first 12 hex characters of the request’s W3C trace ID, uppercase, grouped 4-4-4: **9F2C-41AB-7D03**. Support finds the log entry by it. Duende’s `errorId` stays in the URL and is never shown.
- **Why this shape:** it can’t be mistaken for an email code (codes are 8 Crockford characters drawn in slots); the hyphens make it easy to read aloud.
- **Markup** (`.ri-ref`, centred, 24 below the actions):
  `<p class="ri-ref">Reference <span class="ri-ref-code">9F2C-41AB-7D03</span></p>` followed by a plain `sm` button **Copy** with `aria-label="Copy reference"`. Tabular figures, not monospace.
- **Copy feedback:** the page’s `.ri-slot` (polite): "Reference copied." Clipboard unavailable: select the code text and say "Press Command-C to copy." (Ctrl-C off Apple).
- **Help:** **Contact support** and the footer Help link carry the reference into the support form’s topic.
- **Where:** E4 always; P3 (provider error) inside the alert; never on pages whose cause we can explain (timeouts, limits, offline).
- **Copy feedback by page (Revision 2):** on E4 the slot under the reference line says "Reference copied." (spec-edge D5). On P3 the button’s label reads **Copied** for 2 s and "Reference copied." is written once, sr-only, into the page slot, so there is still one polite region (spec-sso §3.3, A11Y-12).
- **Support link:** **Contact support** and the reference go through the support handoff contract (B-19, spec-recovery §15.1): topic `error`, `ref` = the reference, always a new tab with the accessible-name suffix ", opens in a new tab".

### 4.6 Status slot rule (measured)

`.ri-slot` holds an always-present `p.ra-status[role=status]` and `p.ra-status.is-error[role=alert]` in one grid cell. Reserved **20 px from a 350 px column, 40 px below** (375 and 320 px phones).

**The rule:** every slot string fits **one line at a 358 px column** (390 phones) and **two at 288** (320), in Inter and SF, icon included. Check new and translated strings with `node .design/wp3-1/measure-status.mjs <strings.json> 460,358,343,288`. (m) Results for the shared strings (`.design/edges/wp31/slot-strings.json`), lines at 460 / 358 / 343 / 288, Inter then SF:

| String | Inter | SF |
|---|---|---|
| We couldn’t send a code because you’re offline. | 1 / 1 / 1 / 2 | 1 / 1 / 1 / 2 |
| We couldn’t send a code. Check your connection. | 1 / 1 / 1 / 2 | 1 / 1 / 1 / 2 |
| We couldn’t send a code. Try again in a moment. | 1 / 1 / 1 / 2 | 1 / 1 / 1 / 2 |
| You’ve asked for 5 codes in an hour. Try again later. | 1 / 1 / 1 / 2 | 1 / 1 / **2** / 2 |
| The request timed out or you canceled it. | 1 / 1 / 1 / 1 | 1 / 1 / 1 / 1 |
| We couldn’t sign you out. Try again in a moment. | 1 / 1 / 1 / 2 | 1 / 1 / 1 / 2 |

A 343 px column (375 phones) is under 350, so it reserves two lines, and the long limit string is the one that needs them. The shipped limit lines are B-34’s: "You’ve asked for 5 codes in an hour. Try again in 41:12." (no icon: one line at 358, drawn on `Signin-limited`) and "This network has made 30 attempts in an hour." (with its countdown it measures two lines at 358, so the countdown stays on the commit; spec-edge §7). A string that can’t meet the rule is rewritten, never given a taller slot on one page. Revision 2 added slot strings measured one line at 358: the three "We couldn’t sign you in…" lines, "This network has made 5 recovery requests today.", "You can try again now." and "Still there? This page times out in 2:00." (spec-recovery §15.7).

### 4.7 Focus and live regions

| Situation | Rule |
|---|---|
| Form page arrival | Focus the first field **only on a fine pointer** (`matchMedia('(pointer: fine)')`); on phones nothing is focused, so no keyboard opens over the page. Boards draw no focus on bases |
| Outcome and status page arrival | The `h1` (`id="page-title"`, `tabindex="-1"`), no ring |
| Page loaded with an alert (P2, P3, E3, L6, the recovery expiry alerts, R14) | The **alert container** (B-38): `tabindex="-1"`, `aria-labelledby` its title, `aria-describedby` its description (and the reference row on P3), no ring. No live `role` (identity alerts are `role="group"`, R14 a named `section`): focus announces title and description once |
| Failed submit | Focus the first invalid field; its error is read through `aria-describedby` and has no `role`; the document title gets "Error: " |
| Action failure with focus on the button | `p[role=alert]` in `.ri-slot` |
| Busy | The button keeps focus, `aria-disabled="true"`, gerund label ("Sending code…", "Taking you to Google…", "Signing out…"); the spinner is `aria-hidden`, no role. Never `disabled`. The gerund is also written once into the page’s polite slot paragraph, sr-only when the slot shows nothing visible (B-28) |
| Countdowns | Never live. In a countdown button the digits are `aria-hidden` and a fixed sr suffix carries the clock time ("Send code, available at 4:53 PM"; still starts with the visible label, B-27). Announce once at the start (the slot line) and once at 0:00 ("You can send a code now.") |
| Code screens | Every code screen (Verify, S5 step 2, R4, P6, A3 code step, step-up B) behaves the same: focus on the code input on arrival; "New code sent. Use the code in the newest email." until the first character is typed (B-20); after a failed submit the document title gets "Error: " and focus stays in the input with its text selected (B-31). Layout and Tab order: the code-screen grammar below (B-39) |
| Time limit warning | Written once into the slot’s polite paragraph two minutes before a sliding timer ends; focus doesn’t move; **Keep going** is the next Tab stop (B-29, spec-edge §3.6; drawn on `Signin-timeout-warning`) |
| Locked code | AS §4.4: input `readonly` + `aria-disabled`; focus to the error message |
| Changing text in a region | Change the text of the existing `p`; never remount the region. One polite region per page (the slot) |
| Landmarks | One `main`, one `footer`; no skip link on solo pages |
| Links that open elsewhere | Contact support opens the support form in a new tab: name "Contact support, opens in a new tab" |

**Code-screen grammar (B-39; Verify and `Verify-help`, R4, P6, A3’s code step; written once here, the domain specs point to it).** In order:

1. **Head:** the mascot, the `h1` ("Check your email", "Check your new email"), and a lede that names the purpose: "Enter the code we sent to **{email}** to sign in." (Verify), "…to confirm your new email." (R4), "…to link Microsoft." (P6), "…to create your account." (A3). The address uses the long-email markup (§4.8); every lede is 2 lines at 390 (measured on `Verify-help`, `Recover-code`, `Provider-link-code`).
2. **The code input** (8 slots, 4 + 4), focused on arrival.
3. **The code meta**, reserved (2 lines; 3 in a code container under 360): "Codes expire after 5 minutes." / "Send a new code in 1:00".
4. **The commit** (`lg` block, default), named for what the code completes ("Verify code", "Continue", "Link Microsoft", "Create account"). Where the page has a plain **Cancel** (P6, A3), it is stacked directly under the commit.
5. **The alternatives, as one switch line below the commit:** "Can’t get the code?" and, where available, "Use a passkey instead" (passkey first, 24 apart, `.ri-switch.ri-code-alts`; a plain button in the line is drawn as its link text, edges.css §9). A page whose only alternative is a different address says that instead ("Use a different email" on R4 and A3). A help note (R4’s "No code after a few minutes?…") sits between the commit and the line; Verify’s "Not your email? Use a different one" is a second switch line under it.

**Tab order:** the code, the commit (then its Cancel), the alternatives. Nothing sits between the code and the commit. **Settings dialogs keep the AS layout** (S5 step 2, step-up screen B): the alternatives sit in the body under the meta, above the dialog footer, because the footer holds Cancel and the commit.

### 4.8 Long emails

The AS §4.0 rule, with two identity placements:

- **In a lede or body sentence:** `<strong class="ri-strong ra-email-wrap"><span class="ra-sr">full address</span><span aria-hidden="true">local@</span><wbr><span aria-hidden="true">domain</span></strong>`, inside `translate="no"`. It wraps after the @ and the domain never truncates.
- **In the account chip** (E11, A4, A3, P4, P5): `.ra-chip` with `.ra-email`, centred under the lede. In an identity head the address is never truncated: it wraps after the @ when it can’t share one line with the chip’s other parts, at every width (edges.css §9, P-B; the sidebar and index keep account.css’s truncation).
- **Masked addresses** (R11, R12): first character of the local part, three bullets, full domain: "j•••@rivera.studio". Screen readers get "j, hidden, at rivera.studio" through an sr-only string.
- **Test data:** `maximiliana.featherstonehaugh-okonkwo@northwind-specialty-coffee-roasters.co.uk` (AS §10.1). Each WP checks its ledes with it at 320 (copy-only is enough when the markup is the pattern above).

### 4.9 Badges

`.ri-badge` inside `.ri-mark`, `aria-hidden="true"`, Phosphor **bold** glyph 16 (12 at ≤540 tall).

| Glyph | Tone | Means | Used on |
|---|---|---|---|
| `check-bold` | ok (Leaf disc; white glyph light, Ink glyph dark) | Something you wanted is done | E13, E14, R8, R10b, R13 |
| `clock-bold` | neutral (Ink disc, Paper glyph) | Time ran out, or you’re waiting | E1, E10, R6, R6b, R7, R7a |
| `wifi-slash-bold` | neutral | You’re offline | E8 |
| `exclamation-mark-bold` | neutral | Something failed or was refused | E4, E5, P7, P8, R9, R10a, R10c, R12 "already changed" |
| none | — | Form pages, questions and plain information | A1–A4, E6, E11, P4, P5, P6, R1–R5, R6a, R12 |

No other glyphs, no red badge, no badge on settings pages.

### 4.10 Alert on an identity page

A suiss `Alert` (AS §7.2 row 17) as the first child of `.ri-body`, 24 above the form, **only for news present when the page loads** (P2, P3, E3, the L6 alert, and the expiry alerts "This took too long" on R2 and "Confirm your new email again" on R3, spec-recovery §3.2, §3.5). Default variant with `Info` for neutral news (you canceled, you were signed out); `destructive` with `WarningCircle` when an action failed (P3). The alert container is the focus target (B-38): `role="group"`, `tabindex="-1"`, `aria-labelledby` its `alert-title`, `aria-describedby` its description (and P3’s reference), no ring; one-sentence description, no action button (the page’s own buttons are the actions). The alert pushes the form down; that is accepted because nothing has been tapped yet on a fresh page.

### 4.11 Document and board titles

- App: "{h1} – Richie" with an en dash, except when the `h1` already contains "Richie": then the `h1` alone (B-13: "Sign in to Richie", "Create your Richie account", "Sign out of Richie?", "Link Microsoft to your Richie account", "You already have a Richie account", "No Richie account uses this Google account"). "Error: " prefix after a failed submit; no names or emails. Recovery steps name the step: "Recover your account, step 1 of 3 – Richie" (spec-recovery A11Y-09).
- Boards: the `<title>` is the board title from §5.2, never the app title.

### 4.12 Sample data (every board in this area)

| What | Value |
|---|---|
| Today | **Thursday, November 5, 2026**, Portland, Oregon (Pacific) |
| The person | **Jamie Rivera**, `jamie.rivera@northwind.coffee` (the settings sample; this canvas doesn’t use the identity canvas’s `larkspur.io` so Security and Sign in agree) |
| Passkeys, devices, activity | AS §10.1 |
| Google | Linked: `jamie.rivera@gmail.com`, linked October 2, last used October 20 (A4, P1, P2, Security) |
| Microsoft | Jamie’s work account `jamie.rivera@northwind.coffee`, **not linked** on `Security`; the link flow (P5, P6, `Security-linked`) links it; P3 and P7 use Microsoft |
| A new person (P4, A3) | **Priya Natarajan**, `priya.natarajan@gmail.com` (Google), no Richie account |
| Recovery | Jamie lost `jamie.rivera@northwind.coffee`; new address `jamie@rivera.studio` (AS’s change-email target). Requested **November 5 at 10:42 AM**, approved **November 5 at 4:10 PM**, changes **November 8 at 4:10 PM**, recently recovered **until November 15** |
| Time zone in emails and status facts | The account’s time zone, named: "November 5 at 10:42 AM Pacific Time" (else UTC, B-8) |
| Reference | `9F2C-41AB-7D03` |
| A third-party client | **Tallybird** (AS §4.6, invented) |

---

## 5. Canvas plan and board list

### 5.1 Conventions

- **Canvas:** "Richie Identity Edges", `canvas.json` v3 with `createdOnFiles`, `launch` canvas, `pages: []`, `designSystems: []`. The file is `.design/edges/project/canvas.json`: **80 boards** in 16 rows (Revision 1 planned 73, under a cap of 75; the Phase 4 boards from B-21 and B-33 took it to 80). **Only the lead and the PM edit `canvas.json`.** Domain designers report a size or position change in `.design/edges/CANVAS-CHANGES.md` (file, new size, why).
- **Entry board:** `Main.dc.html` = Sign in.
- **Files:** `<Area>-<screen>[-<state>][-mobile|-dark]`. Areas: `Main` (Sign in base), `Signin-*` (states of Sign in), `Signup*`, `Provider-*`, `Timeout*`, `Session-*`, `Error*`, `Offline`, `Limited`, `Signedout*`, `Security*`, `Profile*`, `Recover*`, `Email-*`, `Account-sidebar`.
- **Rows** are flows; columns are desktop base, mobile, dark, then states, **80 px apart**; row pitch is the tallest board + 400; a `title1` note sits **260 above** each row with `maxW` = the row’s span (≤8000).
- **Sizes:** desktop boards 1440 wide, height measured and rounded up to the next 100 (minimum 900), `expand: "fill"`, `$preview` = w × h. Mobile wrappers 390 × 844. Dark wrappers 1440 with `fill`, no width, helmet `body{margin:0;background:#000}`. Overlays 1440 × 900; mobile overlays 390 × 844. The one email board is 720 wide, fixed.
- **Kinds:** **B** base (full markup), **F** full copy of a base with local edits (re-copy after base edits; never hand-patch), **W** wrapper (one import), **O** overlay (`<div inert>` around the import, then a sibling `.ra-layer`: AS §6.1), **P** part.
- **`is_interactive`:** every board whose links go to other boards (bases, full copies and their wrappers). Overlays are not.
- **Board template:** AS §6.1’s, with two stylesheet links, in this order:
  ```html
  <script src="./support.js"></script>
  <link rel="stylesheet" href="./account.css">
  <link rel="stylesheet" href="./edges.css">
  ```
  Copy `Main.dc.html` (form pages), `Signedout.dc.html` (outcome and status pages) or `Security.dc.html` (settings) and change only what the state changes.
- **Static markup only:** no `{{holes}}`, `<sc-if>`, `<sc-for>` or Tweaks.
- **Assets:** mascot `<div class="ri-mark"><img src="/_blob/b4c7243609a18c06cc93f503a2c6c03e" alt="" width="62" height="78"></div>`; deletion-lifecycle pages use the PNG `/_blob/6445032e8879407a1ebe015146fd29a2` at 31 × 39. Google mark: copy the whole `<svg class="ri-provider-mark" …>` from `Main.dc.html` (it uses ids `ri-g-*`; a board that shows two G marks renames the second set `ri-g2-*`). Microsoft mark: the 21 × 21 `<svg class="ri-provider-mark is-ms" …>` from `Main.dc.html`.
- **Icons:** Phosphor regular, inline, `viewBox="0 0 256 256"`, `fill="currentColor"`, `aria-hidden="true"`, `focusable="false"`; badge glyphs bold. Check with `python3 -I .design/canvas-tools/check-icons.py .design/edges/icons <boards>` (add any new icon’s SVG from unpkg to `.design/edges/icons/` first).
- **Links:** board files (`href="Timeout.dc.html"`); out-of-area `#help`, `#privacy`, `#terms`, `#richie`, `#support`. On this canvas the settings sidebar’s Preferences, Connected apps and Data links are `#preferences`, `#connected-apps`, `#data` (those boards aren’t here).
- **Canvas-only class:** `.is-focused` draws a ring where focus would be on arrival; never on bases.

### 5.2 Board list

Regenerated in Revision 2 from `.design/edges/project/canvas.json` (file, title, size, row and x are the canvas’s): **80 boards** in 16 rows. Rows are y = 0, 1400, 2800, 4100, 5400, 6700, 8000, 9300, 10600, 11900, 14300, 17400, 18800, 20300, 22100, 23500; the row names are the `row-…` notes. The 73 boards of Revision 1 became 80 with the Phase 4 boards from B-21 and B-33 (`Signedout-client`, `Signin-limited-dark`, `Limited-dark`, `Security-load-error-dark`, `Recover-limited-dark`, `Verify-help`, `Provider-not-linked`). Sizes and titles changed in Phase 4 follow CANVAS-CHANGES (B-4, B-11, COPY-26); a size here is the board’s `$preview`, which may be larger than the measured page.

**WP3.1 (foundation, drawn and tested: 9 boards)**

| File | Board title | Size | Kind | State drawn | Row, x |
|---|---|---|---|---|---|
| `Main` | Sign in, desktop | 1440×900 | B | Sign in: email, Send code, "or", passkey, Google, Microsoft (default, empty) | signin, 0 |
| `Main-mobile` | Sign in, mobile | 390×844 | W | `Main` at 390 | signin, 1520 |
| `Main-dark` | Sign in, dark | 1440×900 | W | `Main` dark | signin, 1990 |
| `Signedout` | Signed out, desktop | 1440×900 | B | E13, ok badge, **Sign in again**, lost-device note | signout, 0 |
| `Signedout-mobile` | Signed out, mobile | 390×844 | W | E13 at 390 | signout, 1520 |
| `Signedout-dark` | Signed out, dark | 1440×900 | W | E13 dark | signout, 1990 |
| `Security` | Sign-in and security, desktop | 1440×1900 | B | Shipped Security + the **Google and Microsoft** section (Google linked, Microsoft not linked) | security, 0 |
| `Account-sidebar` | Account sidebar, shared part | 256×900 | P | Shipped part (link deltas only) | security, 8070 |
| `Profile` | Profile, desktop | 1440×1000 | B | Shipped Profile (`Main` on the settings canvas), base for the email and session overlays | email, 0 |

**Delta from the shipped settings boards** (everything else is byte-identical to `.design/edges/ref/settings/`):

| Board | Change | Why |
|---|---|---|
| `Security` | New section **Google and Microsoft** (`id="providers"`) between Passkeys and Where you’re signed in | D1 |
| | Lede: "Sign in with an email code, a passkey, Google or Microsoft. There’s no password to forget." | The old lede named two methods |
| | Change email link → `Profile.dc.html#email`; mobile back link → `#account` | `Main` is Sign in on this canvas; the index board isn’t here |
| | Links `edges.css`; `$preview` 1440 × 1900 ((m) page height 1856) | The section uses `.ri-tile-brand` and `.ri-provider-mark` |
| `Account-sidebar` | Profile → `Profile.dc.html`; Preferences, Connected apps, Data → `#preferences`, `#connected-apps`, `#data`; links `edges.css` | Boards not on this canvas |
| `Profile` | Mobile back link → `#account`; links `edges.css`; file renamed from `Main` | Same |

**The Google and Microsoft section** (copy and behaviour that WP3.3 builds on):

| Element | Copy and rule |
|---|---|
| `h2` | Google and Microsoft (`id="providers-title"`, `tabindex="-1"` for focus after unlink) |
| Description | Link an account to sign in with it. We only get your name, email address and photo from it. |
| Google row | Tile `.ra-tile.ri-tile-brand` with the G (Paper tile, `--border` edge); title **Google**; description 1 `jamie.rivera@gmail.com` (`.ra-email-wrap`); description 2 "Linked October 2. Last used October 20."; action **Unlink** (outline `sm`, `aria-label="Unlink Google"`, `aria-haspopup="dialog"`) |
| Microsoft row | Tile with the four squares; title **Microsoft**; description "Not linked. Works with a work, school or personal account."; action **Link** (outline `sm`, `aria-label="Link Microsoft"`) |
| Foot | The section status slot only (no section action) |
| Order of rows | Google, then Microsoft, always both; one account per provider |
| Not shown | Rows never show "Connected" or "SSO"; the activity list stays as shipped |

**WP3.2 edge pages (20 boards)**

| File | Board title | Size | Kind | State drawn | Row, x |
|---|---|---|---|---|---|
| `Timeout` | Sign-in timed out, desktop | 1440×900 | B | E1 on Verify, email kept, **Start again** | timeout, 0 |
| `Timeout-mobile` | Sign-in timed out, mobile | 390×844 | W | E1 at 390 | timeout, 1520 |
| `Timeout-dark` | Sign-in timed out, dark | 1440×900 | W | E1 dark (an error, drawn dark) | timeout, 1990 |
| `Session-expired` | Profile, signed out while you were away | 1440×900 | O (imports `Profile`) | E2 dialog | session, 0 |
| `Session-expired-mobile` | Profile, signed out while you were away, mobile | 390×844 | W | E2 drawer | session, 1520 |
| `Error` | Something went wrong, with a reference | 1440×900 | B | E4 with the reference line | error, 0 |
| `Error-mobile` | Something went wrong, mobile | 390×844 | W | E4 at 390 | error, 1520 |
| `Error-dark` | Something went wrong, dark | 1440×900 | W | E4 dark | error, 1990 |
| `Error-link` | Link doesn’t work anymore | 1440×900 | B | E5 | error, 3510 |
| `Error-notfound` | Page not found | 1440×900 | B | E6 | error, 5030 |
| `Signin-offline` | Sign in, offline | 1440×900 | F (of `Main`) | E7: email filled, offline failure in the slot | offline, 0 |
| `Signin-offline-dark` | Sign in, offline, dark | 1440×900 | W | E7 dark | offline, 1520 |
| `Offline` | You’re offline, page | 1440×900 | B | E8 | offline, 3040 |
| `Signin-limited` | Sign in, too many codes | 1440×900 | F (of `Main`) | E9 (L1): **Send code in 41:12** unavailable and focused, slot line (B-12, B-27) | limits, 0 |
| `Limited` | Too many attempts, page | 1440×900 | B | E10 on arrival: **Try again in 12:00** (title: B-11) | limits, 1520 |
| `Signin-limited-dark` | Sign in, too many codes, dark | 1440×900 | W | E9 dark (B-21) | limits, 3040 |
| `Limited-dark` | Too many attempts, dark | 1440×900 | W | E10 dark (B-21) | limits, 4560 |
| `Signedout-prompt` | Sign out, confirm | 1440×900 | B | E11 with the account chip | signout, 3510 |
| `Signedout-everywhere` | Signed out of every device | 1440×900 | B | E14 | signout, 5030 |
| `Signedout-client` | Signed out, back to Tallybird | 1440×900 | F (of `Signedout`) | E13 after a third-party sign-out: **Back to Tallybird** (outline) under **Sign in again** (COV-13, B-33) | signout, 6550 |

**WP3.3 settings gaps (16 boards)**

| File | Board title | Size | Kind | State drawn | Row, x |
|---|---|---|---|---|---|
| `Security-mobile` | Sign-in and security, mobile | 390×844 | W | `Security` at 390 (new rows’ actions wrap) | security, 1520 |
| `Security-dark` | Sign-in and security, dark | 1440×1900 | W | `Security` dark (brand marks on black) | security, 1990 |
| `Security-linked` | Sign-in and security, Microsoft linked | 1440×2000 | F | S8 linked: Microsoft row linked, status, activity entry | security, 3510 |
| `Security-link-error` | Sign-in and security, Microsoft linked to another account | 1440×2000 | F | S8 error in the section slot | security, 5030 |
| `Security-unlink` | Sign-in and security, unlinking Google | 1440×900 | O | S9 confirm | security, 6550 |
| `Security-passkey-added` | Sign-in and security, passkey added | 1440×2100 | F | S2 added: new row first, status | passkeys, 0 |
| `Security-passkey-limit` | Sign-in and security, 10 passkeys, the limit | 1440×2700 | F | S2 limit: 10 rows (two long names), button hidden, line | passkeys, 1520 |
| `Security-load-error` | Sign-in and security, passkeys didn’t load | 1440×1900 | F | S3 destructive alert + **Try again** | passkeys, 3040 |
| `Security-rename` | Sign-in and security, renaming a passkey | 1440×900 | O | S1 default, field focused | passkeys, 4560 |
| `Security-rename-error-mobile` | Sign-in and security, passkey name too long, mobile | 390×844 | O (at 390) | S1 error in the drawer | passkeys, 6080 |
| `Security-remove-failed` | Sign-in and security, passkey not removed | 1440×900 | O | S4 failure alert inside the confirm | passkeys, 6550 |
| `Security-load-error-dark` | Sign-in and security, passkeys didn’t load, dark | 1440×1900 | W | S3 dark (B-21) | passkeys, 8070 |
| `Profile-email-taken` | Profile, new email already has an account | 1440×900 | O | S5 step 1 error | email, 1520 |
| `Profile-email-taken-mobile` | Profile, new email already has an account, mobile | 390×844 | W | S5 step 1 drawer | email, 3040 |
| `Profile-email-code-mobile` | Profile, new code sent, mobile | 390×844 | O (at 390) | S5 step 2 drawer, "New code sent…" | email, 3510 |
| `Profile-email-changed` | Profile, email changed | 1440×1000 | F (of `Profile`) | S5 done: new address, status | email, 3980 |

**WP3.4 recovery (20 boards)**

| File | Board title | Size | Kind | State drawn | Row, x |
|---|---|---|---|---|---|
| `Recover` | Can’t get the code, signed out | 1440×1100 | B | R1 | recover, 0 |
| `Recover-mobile` | Can’t get the code, mobile | 390×844 | W | R1 at 390 | recover, 1520 |
| `Recover-dark` | Can’t get the code, dark | 1440×1100 | W | R1 dark | recover, 1990 |
| `Verify-help` | Check your email, with Can’t get the code | 1440×900 | B | The sign-in Verify page in this frame with the A6 entry **Can’t get the code?**, a handoff reference for the identity canvas (B-33, COV-12) | recover, 3510 |
| `Recover-account` | Recovery request, which account | 1440×900 | B | R2, email prefilled | request, 0 |
| `Recover-email` | Recovery request, new email | 1440×900 | B | R3 | request, 1520 |
| `Recover-code` | Recovery request, enter the code | 1440×900 | B | R4, focus drawn on the active slot | request, 3040 |
| `Recover-details` | Recovery request, details for support | 1440×1400 | B | R5 | request, 4560 |
| `Recover-details-mobile` | Recovery request, details for support, mobile | 390×844 | W | R5 at 390 | request, 6080 |
| `Recover-limited` | Recovery request, too many requests | 1440×1300 | F | R5 filled, limited (L5 per network) | request, 6550 |
| `Recover-limited-dark` | Recovery request, too many requests, dark | 1440×1300 | W | R5 limited dark (B-21) | request, 8070 |
| `Recover-review` | Recovery request, under review | 1440×900 | B | R6 | status, 0 |
| `Recover-approved` | Recovery request, approved, waiting 3 days | 1440×1000 | B | R7 with the summary group (title: "waiting 3 days", COPY-26) | status, 1520 |
| `Recover-approved-mobile` | Recovery request, approved, waiting 3 days, mobile | 390×844 | W | R7 at 390 | status, 3040 |
| `Recover-completed` | Recovery request, email changed | 1440×900 | B | R8 | status, 3510 |
| `Recover-declined` | Recovery request, declined | 1440×900 | B | R9 | status, 5030 |
| `Email-recovery-alert` | Email to the old address, someone asked to change it | 720×1100 | B (email) | R11 | owner, 0 |
| `Recover-cancel` | Cancel the recovery request, from the old address | 1440×900 | B | R12 | owner, 800 |
| `Recover-canceled` | Recovery request canceled by the account owner | 1440×900 | B | R13 | owner, 2320 |
| `Security-recovered` | Sign-in and security, recently recovered | 1440×1800 | F (of `Security`) | R14 alert + activity entry | owner, 3840 |

**WP3.5 SSO in sign-in (15 boards)**

| File | Board title | Size | Kind | State drawn | Row, x |
|---|---|---|---|---|---|
| `Signin-redirecting` | Sign in, taking you to Google | 1440×900 | F (of `Main`) | P1 busy Google button | signin, 3510 |
| `Signin-provider-canceled` | Sign in, Google sign-in canceled | 1440×1000 | F | P2 alert | signin, 5030 |
| `Signin-provider-error` | Sign in, Microsoft sign-in failed | 1440×1000 | F | P3 destructive alert with the reference | signin, 6550 |
| `Signin-provider-error-dark` | Sign in, Microsoft sign-in failed, dark | 1440×1000 | W | P3 dark | signin, 8070 |
| `Signin-remembered-provider` | Sign in, welcome back with Google | 1440×900 | B | A4 | signin, 9590 |
| `Signup` | Create account, desktop | 1440×1000 | B | A2 with the methods | signup, 0 |
| `Signup-mobile` | Create account, mobile | 390×844 | W | A2 at 390 | signup, 1520 |
| `Signup-finish` | Create account, finish with Google | 1440×900 | B | A3, Priya’s names prefilled | signup, 1990 |
| `Signup-finish-mobile` | Create account, finish with Google, mobile | 390×844 | W | A3 at 390 | signup, 3510 |
| `Provider-no-account` | Google account with no Richie account | 1440×900 | B | P4 | provider, 0 |
| `Provider-link` | Link Microsoft to your Richie account | 1440×900 | B | P5 | provider, 1520 |
| `Provider-link-mobile` | Link Microsoft to your Richie account, mobile | 390×844 | W | P5 at 390 | provider, 3040 |
| `Provider-link-code` | Link Microsoft, enter the code | 1440×900 | B | P6 | provider, 3510 |
| `Provider-no-email` | Microsoft didn’t share a confirmed email | 1440×900 | B | P7 | provider, 5030 |
| `Provider-not-linked` | Signed in, Microsoft not linked | 1440×900 | B | P8: exclamation badge, **Continue**, **Go to Sign-in and security** (B-33, COV-11) | provider, 6550 |

**Totals:** WP3.1 9, WP3.2 20, WP3.3 16, WP3.4 20, WP3.5 15: **80** (Revision 1: 9, 17, 15, 18, 14 = 73; added 3 + 1 + 2 + 1 = 7).

### 5.3 Copy-only states (specified in the domain spec, no board) and why that is enough

A state is copy-only when it is a text or label swap on a layout already drawn on this canvas or on the settings canvas, with no new element, height or focus target. The domain spec gives the exact strings and says which board shows the layout.

Revision 2: this list was rebuilt from the domain specs’ current state matrices (spec-edge §5, spec-settings §6.3, spec-recovery §6, spec-sso §10). States that gained a board in Phase 4 left the list (E13 "Back to {client}": `Signedout-client`; A6 "Can’t get the code?": `Verify-help`; P8: `Provider-not-linked`).

| WP | Copy-only state | Layout drawn on |
|---|---|---|
| 3.2 | E1 sign-up timed out; E1a passkey; E1b provider callback; no email known, no code sent | `Timeout` |
| 3.2 | After **Keep going**, last warning, **Keep going** failed (B-29) on any timed page; the warning itself is drawn once as `Signin-timeout-warning` | the page’s own slot, as on `Signin-timeout-warning` |
| 3.2 | E2 nothing unsaved; a setting that saves on change; after sign-in: values restored | `Session-expired`; the AS `Profile-name-saved` slot position |
| 3.2 | E3 sign in after the session ended (three descriptions) | the alert pattern on `Signin-provider-canceled` |
| 3.2 | E4 third-party client; sign-out route; other identity routes (two actions, **Trying again…**); reference copied, clipboard unavailable | `Error` (the two-button stack of `Error-notfound`) |
| 3.2 | E5 owner’s cancel link; any other one-time link | `Error-link` |
| 3.2 | E7 network failure and server error; passkey, Google or Microsoft pressed offline; offline while idle and back online | `Signin-offline` slot |
| 3.2 | E8 **Trying again…**, still offline, load failed, back online | `Offline` |
| 3.2 | E9 L2; an hour or more; 0:00; email edited; other pages’ commit labels | `Signin-limited` |
| 3.2 | E10 wait over an hour; 0:00 | `Limited` |
| 3.2 | E11 failures; E12 signing out (busy **Signing out…**) | `Signedout-prompt` slot; busy pattern, AS `Security-passkey-waiting` |
| 3.2 | E14 **Back to {client}** | `Signedout-client` |
| 3.3 | Rename saving (**Saving…**); "Passkey renamed."; empty name; server failure | `Security-rename`, `Security-rename-error-mobile`; slot on `Security-passkey-added` |
| 3.3 | Passkey already registered, save failed, prompt failed; not supported; the 10th added | status slot (`Security-passkey-added`; AS `Security-passkey-failed`); button hidden and quiet line (`Security-passkey-limit`) |
| 3.3 | Load error offline and server; other lists | `Security-load-error` |
| 3.3 | Remove the last passkey (description); removing; failure after step-up | AS `Security-remove-passkey`; `Security-remove-failed`; AS `Stepup-code` |
| 3.3 | Change email: empty, malformed, same address, send failed, L1 limit | `Profile-email-taken` (same field error slot) |
| 3.3 | Change email: wrong code, locked, expired, send failed, change failed | AS `Stepup-code-error`, `Stepup-code-locked`; `Profile-email-code-mobile` |
| 3.3 | Link: busy **Taking you to Microsoft…**; failure before the redirect; L9 limit; step-up before linking or unlinking | `Security` (row action); slot on `Security-link-error`; AS `Stepup`, `Stepup-code` |
| 3.3 | Canceled at the provider; organization hasn’t approved; timed out; provider error; signed out while away | slot and note on `Security-link-error` |
| 3.3 | Unlinking; unlinked (**Google unlinked.**); unlink failed | `Security-unlink`; slot on `Security-linked`; `Security-remove-failed` |
| 3.3 | Security emails (changed email, passkey added or removed, provider linked or unlinked); activity entries | `Email-recovery-alert` layout; `Security-linked`, `Security-passkey-added` |
| 3.4 | R1 from P6 (`from=link`), from passkey sign-in (`from=passkey`), no sign-in in progress; passkey waiting, canceled, failure; provider busy and offline; no WebAuthn | `Recover` |
| 3.4 | R2 account cases (no account, open request, waiting, deleted), L2 and L5 countdowns, offline, draft expired ("This took too long"), from settings | `Recover-account` |
| 3.4 | R3 field errors, L5 per address, L1 | `Recover-email` |
| 3.4 | R4 wrong, locked, expired, new code sent, send and check failures | AS `Stepup-code-error`, `Stepup-code-locked`; `Recover-code` |
| 3.4 | Time limit warning on R2–R5 (drawn once as `Signin-timeout-warning`); R5 "Confirm your new email again", L5 race | the page’s own slot; `Recover-details` |
| 3.4 | R6a and R6b when the request is approved; R6a’s cancel failures | `Recover-review-cancel`, `Recover-owner-view` |
| 3.4 | R7a "Your email is changing now", "We need to check something"; email #15 | `Recover-review` |
| 3.4 | R10a canceled by the old address, R10b canceled by the requester | `Recover-declined` |
| 3.4 | R12 approved, already canceled, already closed, already completed | `Recover-cancel`; `Recover-canceled`; `Recover-declined` |
| 3.4 | Step-up note while recently recovered; account undone by support | AS `Stepup` note line |
| 3.4 | Emails other than R11 (#1, #3 to #17; #16 and #17 are the expired notices, B-36) | `Email-recovery-alert` |
| 3.4 | A5 "Can’t use your passkey?" link | identity canvas `Passkey-signin` |
| 3.4 | Settings step-up screen C **Request account recovery** (S11) | AS `Stepup-cant-get-code` (settings canvas) |
| 3.5 | P1 busy Microsoft button and passkey button; A2 provider busy | `Signin-redirecting` |
| 3.5 | P2, P3 for Microsoft and for A2 and A4; P3 organization variant; L6 alert and countdown buttons on A1, A2, A3, A4 | `Signin-provider-canceled`, `Signin-provider-error`; the countdown buttons of `Signin-limited` |
| 3.5 | A2 field errors, send failed, L1 and L2, offline | `Signup` |
| 3.5 | A3 create failed; unconfirmed email that has an account; L6 | `Signup-finish` |
| 3.5 | A3 code step for an unconfirmed Microsoft email (`/signup/finish/code`) | `Provider-link-code` |
| 3.5 | P4 Microsoft wording | `Provider-no-account` |
| 3.5 | P5 sign-up wording ("You already have a Richie account"); Google wording; send failed; L1; passkey states | `Provider-link` |
| 3.5 | P6 wrong, locked, expired, new code, resend and link failures | AS `Stepup-code-error`, `Stepup-code-locked`; `Provider-link-code` |
| 3.5 | P7 sign-up variant; Google wording | `Provider-no-email` |
| 3.5 | P8 "This Microsoft account was just linked to another Richie account"; Google wording | `Provider-not-linked` |
| all | Dark for states without a dark board | the forced-dark build (§5.4) |

**Dark boards** are drawn for one error per flow (B-5: `Timeout-dark`, `Error-dark`, `Signin-offline-dark`, `Signin-provider-error-dark`), for every base that introduces a new element in dark (`Main-dark`, `Signedout-dark`, `Security-dark`, `Recover-dark`) and, since B-21, for the limit and load-error states (`Signin-limited-dark`, `Limited-dark`, `Recover-limited-dark`, `Security-load-error-dark`). **Departure from how-we-design §4 ("draw dark for at least every error"):** drawing every error dark would add about 20 wrapper boards on top of the 80 here. Every colour here is a token, and the forced-dark build tests every board dark with its screenshots in the QA report; the drawn ones cover each new element in dark. Mobile (B-22): states without a 390 board are harness-tested, and the QA report carries a 390 render of every board.

### 5.4 Testing (every WP, before handoff)

```
node .design/qa-rig/build.mjs .design/edges/project .design/edges/out-<wp> .design/edges/blobs
lsof -i :<port>        # must print nothing. WP3.2 8812, WP3.3 8813, WP3.4 8814, WP3.5 8815
node .design/qa-rig/run.mjs --out .design/edges/out-<wp> --port <port> --boards <your boards> --results .design/edges/qa/<wp>
python3 -I .design/canvas-tools/check-icons.py .design/edges/icons .design/edges/project/<your boards>.dc.html
node .design/wp3-1/measure-status.mjs <your strings.json> 460,358,343,288
```

Also: the forced-dark build (a copy of the out dir with `class="dark"` on every `<body>`), `--forced-colors`, layout shift (record the `h1` and primary y across a page’s states), and look at 390 light, 390 dark and 1440 light yourself. **Known false positives** are how-we-design §5’s, plus these found on the WP3.1 boards: footer links at 24 px on fine pointers ≥768 (by design, 44 below 768 and on coarse pointers); inline note links under 44 (inline, exempt); axe "needs review" colour contrast on the outline **Link Microsoft** button over Mist (computed 16.83:1).

**WP3.1 results** (`.design/edges/qa/wp31/report.md`, 2026-11-05): 7 boards × 10 viewports × 2 schemes: **0 blockers, 0 majors, 30 minors**, all in the false-positive list above or how-we-design §5 (18 footer links on six boards, 3 inline note links, 8 sidebar items at 36 px, 1 axe review). `check-icons`: 0 bad. `drift`: 0. Screenshots checked by eye at 390 light, 390 dark, 1440 light, 320 and 1280 × 540: the mascot renders (SVG), the Google mark renders its gradient in light and dark, nothing overflows.

---

## 6. CSS plan for `edges.css`

`.design/edges/project/edges.css` extends `account.css`. Boards link `account.css` first, then `edges.css`. It **never** redefines an `account.css` token or recipe value: new tokens are `--ri-*`, new recipes are `.ri-*`, and the few rules that touch a suiss part are additive modifiers (`[data-slot=button].ri-method` adds a gap and a mark size; colours, height and radius stay `account.css`’s). `account.css` stays byte-identical to the shipped canvas.

### 6.1 From `account.css`, unchanged

| `account.css` section | Used here for |
|---|---|
| 0 Tokens | Every colour (§1.3) |
| 1 Base (`.ra`, `.ra-sr`, `.ra--part`, `.ra-layer`, the inert pin) | The page root (`.ra.ri`), sr-only text, overlays |
| 2–9 Shell, sidebar, bars, page header, sections, inset group, rows, tiles | Settings boards only; the inset group (7–8) also for status-page summaries |
| 10–11 Buttons, busy and unavailable | Every button, countdown buttons |
| 12 Fields | Every field and field error |
| 15, 15b Emails, account chip | Long emails, the chip |
| 16 Status line, `.ra-slot` | Inside `.ri-slot` |
| 17 Alert | Page alerts (§4.10), load errors |
| 18–19 Dialog, confirm | Settings overlays (S1, S4, S5, S9, E2) |
| 20 Code entry | R4, P6 and the step-up code (centred in the column) |
| 21 Toast | Settings only (S9 doesn’t toast: the row stays) |
| 29–33 Utilities, focus, responsive, motion, forced colours | Everywhere |
| 28 Solo page (`.ra-solo`) | **Not used for new pages** (superseded by `.ri-*`); kept for the shipped settings solo pages |

### 6.2 New recipes in `edges.css`

| # | Recipe | Selectors | suiss mapping | Reason |
|---|---|---|---|---|
| 0 | Badge glyph token | `--ri-badge-ok-fg` on `:root/.light`, `.dark`, `[data-appearance=system]` | — | White on `#4ade80` is 1.74:1; dark uses Ink (9.66) |
| 1 | Identity frame | `.ri`, `.ri-main`, `.ri-col` (container `ri-col`) | auth frame (`section.flex.min-h-150…`), `w-115` | Centred 460 column, top-anchored; whole-pixel placement with `round()` (fallback `margin: auto`) |
| 2 | Head | `.ri-head`, `.ri-mark`, `.ri-title`, `.ri-lede`, `.ri-strong`, `.ri-head .ra-chip` | `h1 text-3xl font-semibold tracking-tight`, `p text-muted-foreground` | §1.4 type; the mascot snapped to whole pixels; 31 × 39 at ≤540 tall |
| 3 | Badge | `.ri-badge` + `.is-ok`, `.is-neutral` | `verify-*` success disc (`size-19 rounded-full`), smaller | §4.9 |
| 4 | Step line | `.ri-step` | `signup-steps` "Step 1 of 3" (`text-sm font-semibold text-muted-foreground`), weight 400 here | §2 row 20 |
| 5 | Body | `.ri-body`, `.ri-form`, `.ri-form:has(> .ra-otp)` (code container) | `Field`/`FieldGroup` | Start-aligned body; the code meta reserves 3 lines on phones (AS R-16) |
| 6 | Commit row and slot | `.ri-commit`, `.ri-slot` (20; 40 under a 350 px column) | none (own `StatusSlot`, AS §8 row 16) | §4.6 |
| 7 | Action stack | `.ri-actions` | `login-sheet` stack (`flex flex-col gap-2.5`), 12 px | Full-width block buttons |
| 8 | "or" | `.ri-or` | `field-separator-with-text` | §2 row 10 |
| 9 | Sign-in method buttons | `[data-slot=button].ri-method`, `svg.ri-provider-mark`, `.is-ms` | `Button variant="outline" size="lg" block` + `data-icon="inline-start"` | §2 rows 8, 9; one 20 px mark box (21 for Microsoft’s pixel grid) |
| 10 | Notes and switch | `.ri-note`, `.ri-switch` (44 px link hit areas) | `login-passkey` text link | Centred Detail lines |
| 11 | Reference | `.ri-ref`, `.ri-ref-code` | none | §4.5 |
| 12 | Footer | `.ri-foot`, `.ri-foot-brand` | none | §2 row 16 |
| 13 | Brand tile (settings) | `.ra-tile.ri-tile-brand` | `ItemMedia` | Google asks for the G on a white, dark or neutral ground; Paper with a hairline keeps both marks true |
| 14 | Short screens, phones, coarse pointers | `@media (max-height: 540px)`, `(max-width: 767px), (pointer: coarse)` | — | Mascot 1×, tighter head; 44 px footer targets |
| 15 | Forced colours | `.ri-badge` (CanvasText/Canvas), `.ri-provider-mark` (`forced-color-adjust: none`), `.ri-or` lines (GrayText), brand tile outline | — | Shapes survive; logos keep their colours like the mascot image |

**The Google mark.** Google’s bundle draws the 2025 gradient G with a mask, blurred shapes and an HTML `conic-gradient` inside a `foreignObject` (a Figma export). `Main` inlines the logo area only (`viewBox="10 10 20 20"`), with the Figma metadata removed and the root `fill="none"` kept (without it the base path paints black: found and fixed on the first render). It renders correctly in Chromium in light and dark (m). **Risk for the PM:** the live canvas must render inline `foreignObject`; check `Main` after the first publish. Fallback: upload the bundle’s `Android + Web/PNG @4x` light and dark logo crops as blobs and use `<img>` (§9).

### 6.3 Contrast of new pairs (computed, `.design/edges/wp31/contrast.mjs`)

| Pair | Light | Ratio | Dark | Ratio | Needs |
|---|---|---|---|---|---|
| Ok badge glyph on its disc | `#ffffff` on `#16a34a` | 3.30 | `#1d1d1f` on `#4ade80` | 9.66 | 3:1 |
| (rejected) white glyph on the dark disc | | | `#ffffff` on `#4ade80` | 1.74 | fails |
| Ok badge disc vs page | `#16a34a` on `#ffffff` | 3.30 | `#4ade80` on `#000000` | 12.05 | 3:1 |
| Neutral badge glyph on its disc | `#ffffff` on `#1d1d1f` | 16.83 | `#000000` on `#f5f5f7` | 19.29 | 3:1 |
| Neutral badge disc vs page | `#1d1d1f` on `#ffffff` | 16.83 | `#f5f5f7` on `#000000` | 19.29 | 3:1 |
| Method button label | `#1d1d1f` on `#ffffff` | 16.83 | `#f5f5f7` on `#000000` | 19.29 | 4.5:1 |
| Method button edge vs page | `#86868b` | 3.62 | `#6e6e73` | 4.14 | 3:1 |
| "or", step line, notes (Slate on page) | `#6e6e73` | 5.07 | `#a1a1a6` | 8.16 | 4.5:1 |
| Reference code, footer "Richie" (Ink on page) | `#1d1d1f` | 16.83 | `#f5f5f7` | 19.29 | 4.5:1 |
| *Reported, not required* | | | | | |
| Brand tile edge vs grouped list (decorative) | `#d2d2d7` on `#f5f5f7` | 1.38 | `#424245` on `#161617` | 1.81 | — |
| Brand tile vs grouped list (surface) | | 1.09 | | 1.16 | — |
| Google’s own button stroke (reference) | `#747775` on `#ffffff` | 4.53 | `#8e918f` on `#131314` | 5.83 | — |
| Logo colours (exempt: brand marks, the label carries the meaning) | Microsoft `#ffb900` on white | 1.72 | Google yellow on white | 1.51 | — |

All other pairs are AS §7.1’s, unchanged.

### 6.4 The four WP sections

`edges.css` ends with four empty, delimited sections: `/* ===== WP3.2 edge pages ===== */ … /* ===== end WP3.2 ===== */`, and the same for WP3.3, WP3.4 and WP3.5.

- Write **only inside your own section**. Prefix new classes `.ri-edge-`, `.ri-set-`, `.ri-rec-`, `.ri-sso-`.
- No new tokens and no overrides of shared recipes. If you need a shared change, write it in `.design/edges/CSS-REQUESTS.md` (selector, value, why, boards affected); the lead merges.
- Each rule gets a one-line comment with its reason; list every recipe in your spec’s CSS table with its suiss mapping and any new colour pair with its ratio.

---

## 7. Briefs for WP3.2 to WP3.5

> Historical: these are the Revision 1 briefs the domain designers started from. The domain specs and Part B supersede them where they differ (for example "Cancel this request", B-10).


**Common to every brief**

- **Inputs:** this file (binding), how-we-design Revision 2, AS Revision 5, the suiss addendum, the inventory, and the reference boards `Main`, `Signedout`, `Security` (copy their markup; don’t invent new structure).
- **Write:** your boards in `.design/edges/project/`, your CSS section in `edges.css`, and your spec file in `docs/design/identity-edges/`. Don’t edit other WPs’ boards, `canvas.json`, `account.css`, the shared `edges.css` sections, or `docs/engineering/`. No app code, no commits, no publishing.
- **Your spec file** has: a one-paragraph summary; **per page**: route and document title (from §3.1), a copy table with final strings, a state table (every how-we-design §4 checklist item → board, copy, harness or n/a), behaviour (what each button does, focus, live regions, timers), reveals and limits, and **a short rationale** (why this page looks and reads the way it does); your emails (subject, body, one button); your CSS table; open questions you resolved, and any you couldn’t, with defaults; your rig results and the false positives you discarded.
- **Sample data** is §4.12. **Copy** follows §2.1, §4.3 and §4.4. **Badges** follow §4.9.
- **Done when:** every board in your §5.2 table exists and matches its title and size, every copy-only state in §5.3 has final copy in your spec, the rig shows 0 blockers and no unexplained majors on your boards, and you’ve looked at 390 light, 390 dark and 1440 light yourself.

### 7.1 WP3.2 Edge pages (`spec-edge.md`)

- **Boards (17):** §5.2 WP3.2 table. Port **8812**, out `.design/edges/out-wp32`, results `.design/edges/qa/wp32`.
- **Draw:**
  - **Timeout (E1):** copy `Signedout`. Clock badge. Say what timed out, after how long (§8 Q1: 20 minutes), and that the email is kept. **Start again** (primary). Show the kept email in the lede with the long-email markup (§4.8). E1a (passkey) as copy.
  - **Session-expired (E2):** an overlay over `Profile` (AS dialog rules; no ×; **Sign in again** primary, **Go to Richie** outline; Escape = Go to Richie? decide and say why). Say that unsaved changes come back after sign-in (§3.3 rule 6). Mobile is the drawer wrapper. E3 as copy (the info alert on Sign in).
  - **Error (E4):** exclamation badge; one-sentence `h1` and lede that don’t blame the person; **Go to Richie**; the reference line (§4.5) with **Copy**; Help. `Error-link` (E5) and `Error-notfound` (E6, no badge) are variants with their own copy and exits.
  - **Offline:** `Signin-offline` is a full copy of `Main` with the email filled and the offline failure line in the slot (focus on **Send code**). `Offline` (E8) is a page with the wifi-slash badge and **Try again**. Copy-only: going offline while idle, back online.
  - **Limits:** `Signin-limited` (E9) is a full copy of `Main` with **Send code in 41:12** (unavailable, `aria-disabled`, focusable; `min-width` of the widest label so it doesn’t jump) and the slot line; the method buttons stay enabled. `Limited` (E10) is a page with the clock badge and the countdown button.
  - **Sign out:** `Signedout-prompt` (E11): `h1` "Sign out of Richie?", the account chip, **Sign out** (primary), **Sign out of every device** (outline), **Cancel** (plain). E12 busy as copy. `Signedout-everywhere` (E14): ok badge, "You’re signed out on every device".
- **Cross-links:** E1 and E13 → `Main`; E2 → `Main`; E5 → `Recover-account`; E11 → `Signedout`, `Signedout-everywhere`.
- **Resolve:** E2’s Escape and what "Go to Richie" does with unsaved values; whether E8 auto-retries silently or says "Trying again…"; E10 copy when `Retry-After` is over an hour (default: "Try again at 2:30 PM."); whether E5 says why a link stopped working (default: no, one page for all causes).

### 7.2 WP3.3 Settings gaps (`spec-settings.md`)

- **Boards (15):** §5.2 WP3.3 table. Port **8813**, out `.design/edges/out-wp33`, results `.design/edges/qa/wp33`.
- **Base:** `Security` and `Profile` from WP3.1. **Re-copy, never hand-patch.** Settings rules apply in full (AS §4.0 dialogs, confirms, toasts, slots, step-up); this area adds nothing to them.
- **Draw:** S1 rename (default with the name selected; the error drawer at 390 "Use 40 characters or fewer."; reserve the error line so the dialog keeps one height); S2 added (new row first, "Added just now", status) and the limit (10 rows, two names at the 40-character limit, the button hidden, the quiet line "You have 10 passkeys, the limit. Remove one to add another."); S3 load error (AS §4.3); S4 remove failure inside the confirm; S5 change email (step 1 error "That email already belongs to another Richie account. Use a different one.", the drawers, step 2 "New code sent. Use the code in the newest email." per §2.1, done); S7 to S9 Google and Microsoft (linked with the activity entry "Linked Microsoft", already linked to another account, unlink confirm).
- **Specify as copy:** every other S-state in §5.3; the security email to the old address after a change (subject, body, button, "If this wasn’t you" → Help); the step-up descriptions "To link Microsoft, use a passkey or an email code." and "To unlink Google, use a passkey or an email code."; activity entries for rename, remove, link and unlink; the provider callback statuses (linked, canceled "You canceled linking Microsoft. Nothing changed.", provider error with the reference).
- **Cross-links:** `Security-linked` and `Security-link-error` are the returns from the provider; `Security-recovered` (WP3.4) is a full copy of your `Security` base after your edits land; tell WP3.4 when `Security` is final.
- **Resolve:** unlink confirm copy (what stops working, what still works: "You won’t be able to sign in with Google anymore. You can still use your passkeys or an email code."); whether unlinking needs step-up (default yes, §8 Q20); what "already linked to another Richie account" tells the person to do; the passkey rows at the limit with long names (truncation per AS row 8, `wrap-2`).

### 7.3 WP3.4 Recovery (`spec-recovery.md`)

- **Boards (18):** §5.2 WP3.4 table. Port **8814**, out `.design/edges/out-wp34`, results `.design/edges/qa/wp34`.
- **Draw:** R1 (all ways in, for every visitor, §1.7 principle 5: back to the code, passkey, Google, Microsoft, then "Lost access to your email?" with **Start a recovery request**; no badge); R2 to R5 (three steps with "Step n of 3", each its own page; R4 uses the code entry with focus drawn on the active slot; R5 asks what support needs: name on the account, roughly when you created it, the devices you use Richie on, when you last signed in, anything else; **Send request**); R5 limited; R6 to R9 status pages (§4.1 status shape, clock badge for review and hold, check for completed, exclamation for declined; the summary group with real dates); the old-address email R11 (720 wide: shiba 31 × 39, subject as the first line, body, **This wasn’t me** button, masked new address, why they got it); R12 and R13; R14 (`Security` full copy: a default `Alert` above the first section, "Your email was changed by account recovery on November 8. Until November 15 we’ll ask you to confirm it’s you for every sensitive change. Check your passkeys and devices.", and the activity entry "Email changed by account recovery").
- **Specify as copy:** R10; every R15 email (received to the new address, approved to both, completed to both with **This wasn’t me** on the old one, declined, canceled); the A5 and A6 links on the identity canvas’s Verify and passkey sign-in; the step-up note during the 7 days ("Your account was recovered recently, so we’ll ask every time until November 15.").
- **Rules to keep:** R1 reveals nothing (no "your account has a passkey"); R2 reveals no more than Sign in; status pages are token links that show one request and expire (§3.1); the hold is shown as a date and time, never "72 hours from now"; completing signs out every device; the old address can cancel until the change happens.
- **Cross-links:** R1 ← identity `Verify`, `Passkey-signin` (copy); R8 → `Main` with the new email; R13 → `Main`; R14 is a full copy of WP3.3’s final `Security`; E5 (`Error-link`) for bad links.
- **Resolve:** §8 Q8 to Q14 defaults into copy; R5’s fields (labels, which are optional, limits on length); whether R2 says an account already has an open request (default: yes, and where its status went); what support’s reply time line says (§8 Q9).

### 7.4 WP3.5 SSO in sign-in (`spec-sso.md`)

- **Boards (14):** §5.2 WP3.5 table. Port **8815**, out `.design/edges/out-wp35`, results `.design/edges/qa/wp35`.
- **Base:** `Main` (WP3.1) for every A1 state; `Signup` is your new base (identity’s Create account fields in the new frame: First name, Last name (optional), Email, **Create account**, legal line, then "or" and the two provider buttons, no passkey button on sign-up).
- **Draw:** P1 (the Google button busy: spinner instead of the mark, "Taking you to Google…", `aria-disabled`; the others stay enabled); P2 (info alert, "You canceled signing in with Google. Nothing changed."-style copy, focus on the alert title); P3 (destructive alert with the reference) and its dark wrapper; A4 (remembered account: "Welcome back, Jamie", the chip with **Not you?**, **Sign in with Google** as the first action, **Sign in another way**); A2 and A3 (Priya’s names prefilled from Google, the email from Google shown as the chip, not a field, the legal line, **Create account**); P4; P5 and P6 (never link on the provider’s claim alone: an email code, or a passkey, proves the Richie account first); P7.
- **Rules to keep:** method buttons are `.ri-method` outline `lg` block, in the §2 row 8 order, never filled; no button says "SSO"; a provider’s email counts as confirmed only per §8 Q5; linking is by provider subject, never by email; P4 and P5 reveal no more than Sign in; a provider sign-in to an account in its deletion window goes to `Deletion-pending` or `Signin-deleted` as an email sign-in would.
- **Cross-links:** `Main` → `Signin-redirecting`; A1 alerts ← provider; P4 → `Signup-finish`; P5 → `Provider-link-code`; A3 → (identity passkey offer, copy); P7 → `Main`.
- **Resolve:** P2 and P3 copy; whether A4 shows the passkey button too (default: no, "Sign in another way" leads to A1); what A3 does when the provider gives no last name (field empty, optional); the passkey offer after a provider sign-up (default: skip, §8 Q19).

---

## 8. Open product questions, with defaults

Design proceeds on the default. Each names who should confirm.

| # | Question | Default | Confirm |
|---|---|---|---|
| Q1 | How long does a sign-in request last before E1? | **20 minutes** after the last step on an identity page (our pending-sign-in state; the code itself lasts 5) | Engineering |
| Q2 | How long does a signed-in device stay signed in? | **14 days without use**, 90 days at most; recovery and deletion end every session at once | Product, engineering |
| Q3 | Microsoft’s guidelines name "Sign in with Microsoft" (or "Sign in"); "Continue with Microsoft" isn’t listed. Keep D1’s label? | **Decided by the PM (2026-11-05): "Sign in with Google" and "Sign in with Microsoft" on every sign-in surface** (Sign in, Welcome back, Can’t get the code?, settings link flows), matching Microsoft’s named label and the page’s existing "Sign in with a passkey"; **"Sign up with Google" and "Sign up with Microsoft" on Create account** (Google lists "Sign up with Google"; Microsoft names no sign-up label, so brand confirms before launch, fallback "Sign in with Microsoft") | PM (decided), brand |
| Q4 | Google’s own button uses Google Sans Medium 14 and stroke `#747775`; Google recommends its rendered button over custom ones. Our custom button uses our font, 16 px and our `--input` edge | **Proceed with the custom button** (standard G unaltered, label from Google’s list, equal prominence, a white or dark ground). Brand check before launch | PM, legal |
| Q5 | When is a provider’s email "confirmed"? | **Google:** `email_verified = true`. **Microsoft:** only when `xms_edov = true` (domain owner verified) or for a personal Microsoft account’s own address; otherwise unconfirmed. Unconfirmed emails never match an account (P7 on sign-in) and need an email code on A3 | Engineering, security |
| Q6 | Link on a matching email automatically? | **Never.** Always an email code or a passkey for the matching account first (P5, P6) | Security (recommended by the brief) |
| Q7 | Can a Google sign-up skip the email code? | **Yes** when Google confirms the email; Microsoft unconfirmed → code | Security |
| Q8 | When does the 72-hour hold start? | **At approval.** The old address is emailed at submission and again at approval with the exact date and time | Product, support |
| Q9 | What reply time do we promise? | **"Richie support usually replies within 2 business days."** (placeholder until support confirms; one sentence everywhere, spec-recovery COPY-12) | Support |
| Q10 | What does support check? | The R5 details plus the account’s own records (devices, sign-in history, creation date). The written procedure is support’s (how-we-design §6) | Support, security |
| Q11 | A second request while one is open? | **Refuse**, say a request is open and that its status went to the new address it named | Product |
| Q12 | After a decline, when can someone ask again? | **After 7 days**, and after a cancel by the old address (spec-recovery D-R8); support can allow sooner | Support |
| Q13 | Can a completed recovery be undone? | **Yes, by support, within 30 days**, from the **Contact support** link in the completed email to the old address (B-10; "This wasn’t me" is now only on the changed-email message, S6) | Support, security |
| Q14 | What else happens at completion? | **Every device signed out, refresh tokens revoked**; passkeys, linked accounts and connected apps kept (R14’s alert asks to check them) | Security |
| Q15 | Gateway 429 thresholds | Engineering’s; always send `Retry-After` | Engineering |
| Q16 | Offline page through a service worker? | **Yes**, a minimal worker that caches only the offline page and its assets | Engineering |
| Q17 | "Sign out of every device" on the sign-out prompt? | **Yes** (E11). The Richie app’s account menu may add it too; that is the Richie app’s call | Product |
| Q18 | Where does "Go to Richie" go for a third-party client? | The client’s `ClientUri` when known, else the Richie app root | Engineering |
| Q19 | Passkey offer after a provider sign-in or sign-up? | **No**; offer it after email-code sign-ins only, as today | Product |
| Q20 | Step-up to unlink? | **Yes**, like removing a passkey (AS §4.4). Linking also needs step-up (like adding a passkey) | Security |
| Q21 | Link a provider whose email differs from the Richie email (from settings)? | **Yes**: the person is signed in and stepped up | Security |
| Q22 | The provider account’s email changes later | **No effect**: links are by provider subject | Engineering |
| Q23 | Microsoft tenants | **Any** (`common`): work, school and personal accounts | Product |
| Q24 | Recovery for an account scheduled for deletion | **Allowed**; the deletion stays scheduled and `Deletion-pending` shows at the next sign-in | Product |
| Q25 | Long-term: company SSO | Not now (D1). The email step stays first so domain routing can replace the primary’s action later ("Continue with SSO") without moving anything | Product |

---

## 9. For the PM: decide before the domain designers start

**PM decisions, 2026-11-05 (binding for WP3.2–3.5; they override any default above):**

- **P-1 (Q3) Provider labels.** "Sign in with Google" / "Sign in with Microsoft" on every sign-in surface; "Sign up with Google" / "Sign up with Microsoft" on Create account. Reason: Microsoft’s branding guidance names "Sign in with Microsoft" (shortened "Sign in"), and the Sign in page already says "Sign in with a passkey", so the three methods read as one list. Google allows both labels.
- **P-2 (Q4) Custom Google button.** Proceed with our outline method button and Google’s unaltered G; brand check before launch.
- **P-3 The Google G is an image**, not inline SVG: `<img class="ri-provider-mark" src="/_blob/5162155e6d0adb1f015faa8e669bafac" alt="" width="20" height="20" data-icon="inline-start">` (an 80 × 80 PNG rendered once from Google’s official mark, local copy in `.design/edges/blobs/`). The bundle SVG used `foreignObject`, filters and fixed ids that would collide when two boards are imported into one page. The Microsoft mark stays inline SVG (four solid squares). In the app, ship Google’s official asset file as an `<img>`.
- **P-4 (Q1) A sign-in request lasts 20 minutes.** Accepted as the default number for copy.
- **P-5 (Q8, Q9) Recovery timing.** The 72-hour hold starts when support approves. Support’s first reply is promised "within 2 business days". Both numbers are final copy unless support operations change them (then a text swap).
- **P-6 Order dependency.** WP3.3 keeps the `Security` base board unchanged unless a gap truly needs it, and states any change in its reply. WP3.4 copies WP3.1’s `Security` now for `Security-recovered`; the PM re-checks drift at assembly.
- **P-7 Everything else in §8** proceeds on its recommended default.


1. **Q3 (the Microsoft label)** and **Q4 (custom Google button)**: they change copy and styling on 10 boards across WP3.5 and WP3.3. The designers proceed on the defaults; a change later is a text swap.
2. **Q8 (hold start)** and **Q9 (reply time)**: WP3.4 writes them into four pages and five emails.
3. **Q1 (20 minutes)**: WP3.2 writes the number into the timeout copy.
4. **Publish the foundation first** (`canvas.json`, `Main`, then `edges.css`, `account.css`, the other WP3.1 boards), and check two things on the live canvas: the Google mark’s gradient renders (inline `foreignObject`; fallback in §6.2), and the SVG mascot renders.
5. **`canvas.json` is the lead’s.** It already lists every planned board; designers change sizes only through `CANVAS-CHANGES.md`. Boards that don’t exist yet will show as missing until their WP lands.
6. **Order dependency:** WP3.4’s `Security-recovered` copies WP3.3’s final `Security`. WP3.3 should tell WP3.4 when its `Security` edits are done (or WP3.4 copies WP3.1’s `Security` and re-copies at the end).

---

## 10. Changelog

- **Revision round (Phase 5, 2026-11-05, the revision designer):** Part A synced to the round’s fixes so it no longer contradicts the domain specs: §3.1 rows R6a, R6b (now drawn) and R10c (B-36, added); §3.5 L1/L2 rows and the wording note (B-34); §4.3, §4.4, §4.6 limit lines (B-34); §2 row 15, §4.7 and §4.10 alert focus (B-38); §4.7 the code-screen grammar (B-39) and the time-limit board; §4.8 the chip wraps after the @ (P-B); §4.9 badges R6b, R10c; §5.3 rows for R6a, R6b, emails #16–#17 and the time-limit board. §5.2 (board list) is the PM’s, from `canvas.json`: four new boards are in CANVAS-CHANGES REV-1 to REV-4.
- **Revision 2 (2026-11-05):** synced to the domain specs and Part B after Phase 4 (B-26). §3.1 compared row by row with spec-edge, spec-settings, spec-recovery and spec-sso (rows updated and added are marked in the table; unchanged rows were left as they were); §3.2 and §3.3 corrected for the provider challenge, R8 and R1 return URLs; §3.4 journeys updated (J1 to J20) and J2b, J5b, J22, J23 added; §3.5 keeps L1 to L9 and gains the "Limits and lifetimes" table; §4.2 to §4.12 examples replaced where B-6, B-9, B-11 to B-15, B-27 to B-31 and the domain specs replaced them; §5.2 regenerated from `canvas.json` (80 boards), §5.3 rebuilt from the four state matrices. §6, §7 and §9 are Revision 1 text; §8 Q9 and Q12 and the §2.1 document-title row were corrected. **Open wording items between the domain specs, not settled by Part B:** (1) the inline count for L1 and L2 is written differently in spec-edge E9, spec-settings §3.1, spec-sso §3.5, §5.2, §7.2 and spec-recovery §3.2, §3.3 (see the wording note in §3.5); (2) the support topic for the settings security emails ("Someone changed how I sign in", spec-settings §5.2) is not in spec-recovery §15.1’s fixed topic list; (3) how long an open recovery request stays open ("up to 7 days", spec-recovery §3.2) has no lifetime row in spec-recovery §2.4.
- **Revision 1 (2026-11-05):** Part A written: plan, conflicts, IA for every page in scope, grammar, canvas plan (73 boards), CSS plan, briefs, questions. Reference boards `Main`, `Main-mobile`, `Main-dark`, `Signedout`, `Signedout-mobile`, `Signedout-dark`, `Security`, `Account-sidebar`, `Profile`; `edges.css`; `canvas.json` draft. Rig: 0 blockers, 0 majors.

---

# Part B: the domain specs and PM rulings (assembly, 2026-11-05)

Part A (above) is the foundation. Each area’s page-by-page copy, states, behaviour, rationale per page, state matrix, handoff mapping and open questions live in its own file. **Precedence (one order for all five files, ruling B-37):** Part B rulings in `spec.md`, then the domain specs’ page tables and recorded departures, then Part A (Revision 2 synced it to both). A board that disagrees with its spec is a bug in the board.

| Area | File | Boards | Owner |
|---|---|---|---|
| Edge pages (E1–E14) | [`spec-edge.md`](spec-edge.md) | 17 | WP3.2 |
| Settings gaps: passkeys, change email, Google and Microsoft (S1–S9) | [`spec-settings.md`](spec-settings.md) | 15 | WP3.3 |
| Account recovery (R1–R15) | [`spec-recovery.md`](spec-recovery.md) | 18 | WP3.4 |
| Google and Microsoft in sign-in and sign-up (A1–A4, P1–P7) | [`spec-sso.md`](spec-sso.md) | 14 | WP3.5 |
| Foundation boards (Sign in, Signed out, Sign-in and security, Profile, sidebar) | Part A §5.2 | 9 | WP3.1 |

Canvas: **Richie Identity Edges**, https://claude.ai/artifact/6Xe7bnSdGHE3xvVR5LeMge (assembly version `1793888623-5e41`, 73 boards). Working mirror: `.design/edges/project/`.

**PM rulings at assembly:**

- **B-1 Stale provider callback → E1b (sign-in timed out), not E5.** WP3.2 and WP3.5 disagreed (spec-sso OQ-11 followed Part A’s E5). For the person, the provider round trip took too long; "start again" is the accurate next step, and a dead-link page would suggest a broken email link. spec-sso is aligned in the fix round.
- **B-2 Main’s "Sign in with Microsoft" links to `Provider-link`** on the canvas (J2, Jamie’s first Microsoft sign-in), and the five full copies of Main follow (CANVAS-CHANGES WP3.5-1).
- **B-3 Shared CSS requests merged** into `edges.css` §1, §2, §6 and §8: chip text start-aligned, status-slot icons inline with the first word, the page-alert gap, the textarea recipe, the address-ending-a-lede rule, short local parts (`.ri-email-short`), and the forced-colours badge. Scoped copies in the WP sections were removed. `.ri-email-short` is also a **settings-canvas handoff** (the same gap exists in the shipped sidebar).
- **B-4 Board sizes and rows** follow CANVAS-CHANGES WP3.3-1…4 and WP3.4-1…5 (rows from "passkeys" down moved by +100, "status" and "owner" by a further +200).
- **B-5 Dark is drawn for one error per flow**; every other board is tested dark through the rig’s forced-dark build (`BOARD_THEME=dark`).

**PM copy rulings after Phase 4 copy review (`qa/p4-copy.md`), 2026-11-05:**

- **B-6 (COPY-01) One failure phrase per surface, whichever method was pressed.** Sign-in surfaces (A1, A4, R1, E7, P5): "We couldn’t sign you in…" for the passkey, Google and Microsoft buttons alike. Create account (A2, A3): "We couldn’t sign you up…". Settings Link/Unlink: the button’s verb, "We couldn’t link Microsoft…" / "We couldn’t unlink Google…". "Reach" is retired.
- **B-7 (COPY-04) One "provider linked" security email**, owned by spec-settings §5.2; spec-sso §8.5 points to it. Button **"Check Sign-in and security"** (matches the settings emails’ body wording). Absolute date with the zone ("on November 5 at 10:12 AM Pacific Time"), never "today". One fine print for every security email in all three specs: "You’re getting this because it’s a security change on your Richie account. We always send these."
- **B-8 (COPY-05) Every email names the time zone**: the account’s time zone, else UTC (spec-recovery §2.5 rule, now global). The same applies to "Requested" and "Approved" facts on the status pages.
- **B-9 (COPY-06) Microsoft organization approval:** title or alert "Your organization hasn’t approved Richie"; next step "Ask your IT admin to allow Richie, or {surface clause}" — sign-in: "…or use your email or a passkey."; settings: "…or link a personal Microsoft account." Never "Microsoft 365 admin".
- **B-10 (COPY-08) The old-address email’s button is "Cancel this request"**, with "If this wasn’t you, cancel the request." in the sentence above it. "This wasn’t me" stays only on settings S6 (it goes to support, not a cancel).
- **B-11 (COPY-09) E10’s title is "Too many attempts"**, so "request" keeps one meaning in this area (the recovery request).
- **B-12 (COPY-11) One rate-limit sentence pattern:** "This network has made {n} {things} in {window}." / "This email has been used for {n} recovery requests today." with the countdown or clock time after it.
- **B-13 (COPY-20) Document titles** are "{h1} – Richie", except when the h1 already contains "Richie": then the h1 alone ("Sign in to Richie", "Create your Richie account").
- **B-14 (COPY-21) "Back to Richie"** where the person came from the app (settings sidebar, mobile index); **"Go to Richie"** on edge and signed-out pages.
- **B-15 (COPY-27) The 1:00 rule is for the resend countdown only.** Rate-limit countdowns start at the server’s `Retry-After` (m:ss up to 59:59; over an hour, "Try again at 2:30 PM").
- **B-16 (COPY-30) Relative dates stop at "yesterday"**; anything older is a date ("Last used October 30"). Settings-spec clarification.
- **B-17 (COPY-29)** "teammates" on the shipped Profile stays (it isn’t workspace-specific). No change.

**PM behaviour rulings after Phase 4 coverage audit (`qa/p4-coverage.md`), 2026-11-05:**

- **B-18 (COV-01, J7) Any successful sign-in starts the 10-minute step-up window**, including Google and Microsoft. Step-up methods stay a passkey or an email code. R1’s provider route signs the person in with the return URL `/account/profile#email` and opens Change email at once, so a provider-only person with a lost mailbox changes their email inside the window with no step-up. R1 copy says so. After the window, they sign in with the provider again. spec-recovery, spec-settings Q-S1 and spec-sso OQ-2 align to this.
- **B-19 (COV-02) The support form belongs to Richie support (the help site), not identity.** Identity owns the contract every "Contact support" link relies on, written once in spec-recovery ("Support handoff"): the URL and parameters (`topic`, `ref` for the E4 reference, `request` for recovery tokens, `from` page), the fixed topic list used across all four areas, a required reachable contact email when signed out (prefilled and the account attached when signed in), the message field, the confirmation and the reply promise "within 2 business days". It always opens in a new tab with the accessible-name suffix ", opens in a new tab".
- **B-20 (COV-04) "New code sent. Use the code in the newest email." stays until the first character is typed**, everywhere (S5, R4, P6, A3’s code step), as spec-settings decided: the person leaves to read mail.
- **B-21 (COV-19) Dark wrappers are added** for `Limited`, `Signin-limited`, `Recover-limited` and `Security-load-error` (the limit and load-error states). Every other state stays tested dark through the forced-dark build, with its screenshots in the QA report.
- **B-22 (COV-20) Mobile stays harness-tested** for state pages without a 390 board; the QA report carries a 390 render of every board.
- **B-23 (COV-28) Per-network limits count failures only** for provider sign-ins (a successful Google or Microsoft sign-in never counts toward L2/L6), so an office at a sign-in rush isn’t throttled. Numbers remain engineering-confirmed.
- **B-24 (COV-35) The Help article "Can’t sign in"** is owned by support content; spec-recovery lists its minimum content (the ways in, a link to R1, the support contact).
- **B-25 (COV-37) Signed-in "Can’t get the code?" (settings step-up screen C) offers "Request account recovery"** (R2 with the account prefilled) instead of free-form support, so signed-in and signed-out people get the same safeguards (old-address alert, 72-hour hold). This is a settings-canvas handoff delta, specified in spec-recovery.
- **B-26 (COV-21, COV-27, COPY-19) Part A is synced** to the domain strings with a "superseded by" note on each changed row, and §3.5 gains one "limits and lifetimes" table that collects every number from the four specs.

**PM accessibility rulings after Phase 4 keyboard and screen-reader audit (`qa/p4-a11y.md`), 2026-11-05:**

- **B-27 (A11Y-01) A countdown never ticks inside an accessible name.** In a countdown button ("Send code in 41:12", "Try again in 12:00", "Send a new code in 0:41"), the digits are `aria-hidden` and the button carries a fixed sr suffix with the clock time: accessible name "Send code, available at 4:53 PM" (it still starts with the visible label, 2.5.3). At 0:00 the polite slot says "You can send a code now." (or the page’s 0:00 line) once. Applies everywhere in this area and is a settings-canvas handoff for `Stepup-code-locked`.
- **B-28 (A11Y-03) Every busy state is announced once** by writing its busy text ("Taking you to Google…", "Sending code…", "Signing out…") into the page’s existing polite slot paragraph (sr-only when the slot shows nothing visible). The button’s label still changes visually; no second live region.
- **B-29 (A11Y-04, WCAG 2.2.1) Time limits are adjustable.** The 20-minute sign-in request, the 30-minute recovery draft and the 30-minute new-address confirmation **slide with activity** (any input or request renews them server-side). Two minutes before one runs out, a polite alert in the page’s alert slot says "Still there? This page times out in 2:00." with a plain **Keep going** button that renews it (20 s minimum, up to 10 times). WP3.2 specifies this "time limit warning" once in spec-edge; WP3.4 points R3–R5 at it. The 5-minute email code itself is the authentication factor’s lifetime (essential) and keeps "Send a new code".
- **B-30 (A11Y-05) Drawer and action-sheet footers:** below 768 the commit is drawn on top, so the app renders the commit **first in the DOM** there (the suiss `confirm` already re-renders its footer on mobile), with `flex-direction: column`, not `column-reverse`. Desktop keeps Cancel then the commit, end-aligned. The static boards share one DOM across widths, so this is a handoff rule (spec §8 handoff, and the settings canvas, where `account.css:2878` is the source), not a board change.
- **B-31 (A11Y-16) Every code screen behaves the same** (Verify, S5 step 2, R4, P6, step-up B): arrival focus on the code input; "New code sent…" per B-20; after a failed submit the document title gets the "Error: " prefix and focus stays in the input with its text selected (AS §4.4).
- **B-32 (RIG-07) "States don’t move the page" applies to the states of one page** (a page and its busy, error, offline, limited and success states). Different pages in one flow share the mascot and `h1` position (measured: never moves) but not the primary button’s, which follows each page’s content.
- **B-33 New boards from the fix round** (canvas.json by the PM): `Signedout-client` (E13 with "Back to Tallybird", COV-13, WP3.2), `Limited-dark`, `Signin-limited-dark` (B-21, WP3.2), `Security-load-error-dark` (B-21, WP3.3), `Recover-limited-dark` (B-21, WP3.4), `Verify-help` (the sign-in Verify page in this frame with the R1 entry, a handoff reference for the identity canvas, COV-12, WP3.4), `Provider-not-linked` (P8 "Signed in, not linked", COV-11, WP3.5, if its layout differs from `Provider-no-email`).

**PM rulings after the Part A sync, 2026-11-05:**

- **B-34 Canonical rate-limit lines** (supersede the domain specs’ variants): per address (L1) "You’ve asked for 5 codes in an hour." (second person, as everywhere else); per network (L2/L6) "This network has made 30 attempts in an hour." (the "sign-in" form measured 2 lines at 358 px; B-23 still counts only failures for provider sign-ins). Each is followed by the countdown or clock time (B-15).
- **B-35 Support topics:** the fixed list in spec-recovery §15.1 gains `signin-changed-not-me` ("Someone changed how I sign in"), used by the settings security emails (link, unlink, passkey added or removed). `email-changed-not-me` stays for S6.
- **B-36 A recovery request stays open at most 7 days.** If support hasn’t approved or declined it by then, it closes as expired: both addresses get the decline-style notice ("We couldn’t finish your request in time."), and a new request is allowed at once (no 7-day wait). The approved request’s 72-hour hold runs inside or after those 7 days as specified.

**PM rulings after the Phase 5 grade (`evaluation.md`), 2026-11-05:**

- **B-37 Precedence, one order for all five files:** Part B rulings, then the domain specs’ page tables and recorded departures, then Part A. Every file’s header now says so. A board that disagrees with its spec is a bug in the board. Rulings must also be written into the page tables an engineer reads, not only listed here.
- **B-38 Page alerts that appear on arrival** (P2, P3, E3, the recovery expiry alerts, R14) take focus on the **alert container** (`tabindex="-1"`, `aria-labelledby` its title, `aria-describedby` its description and, on P3, the reference row), not the title paragraph, so the next step is read with it. No ring on the programmatic focus.
- **B-39 One code-screen grammar** (Verify, `Verify-help`, S5 step 2, R4, P6, A3’s code step): head (lede names the purpose: "…to link Microsoft", "…to confirm your new email"), code input, the reserved code meta, the commit, then the alternatives as one switch line **below** the commit ("Can’t get the code?" and, where available, "Use a passkey instead"). Tab order: input, commit, alternatives. Settings step-up dialogs keep their AS layout (alternatives inside the body, above the footer), because the footer is a dialog footer.
- **B-40 `role="alert"` lines may be inserted when they occur** (they are announced on insertion); only `role="status"` paragraphs must exist at page load. Base boards draw the polite paragraph only. (Closes S-A/S-C "draw both paragraphs" as won’t-fix, with this reason.)
- **B-41 Mobile drawer and action-sheet boards:** their visual order is normative and their DOM order is the shared desktop DOM. Each such board’s spec row says "DOM order: commit first below 768 (B-30)". Boards drawn only at 390 (`Security-rename-error-mobile`, `Profile-email-code-mobile`) put the commit first in their DOM.
- **B-42 Brand sign-off** (the custom Google button, "Sign up with Microsoft") is a release gate owned by brand/legal, not a design change; the fallback label "Sign in with Microsoft" on A2 is a text swap.
- **B-43 Divider spacing:** 12 px above "or" when it follows a form (`edges.css` §9), so a filled status line groups with its button.
- **B-44 (revision round) Where the countdown sits on a rate-limit line:** the per-network line (L2/L6) shows the sentence alone in the slot ("This network has made 30 attempts in an hour."), and the countdown lives on the unavailable button above it (B-27: digits `aria-hidden`, sr "available at 4:53 PM"). The sentence plus "Try again in 41:12." measured 2 lines at 358 px. The per-address line (L1) fits with its countdown and keeps it: "You’ve asked for 5 codes in an hour. Try again in 41:12." This amends B-34.
