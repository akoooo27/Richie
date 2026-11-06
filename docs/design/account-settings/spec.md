# Richie account settings: design spec

WP3-1, Richie Design Program. Written 2026-11-04 for the canvas builders and testers. This document is the single source for the **Richie Account settings** canvas: information architecture, layout, copy, states and the board list.

Read first, in this order: [`../how-we-design.md`](../how-we-design.md) (principles, §6 state checklist, §8 harness), [`../suiss-ui-guide.md`](../suiss-ui-guide.md) (§7 is the CSS kit whose `data-slot` names this spec mirrors), [`../suiss-blocks-catalog.md`](../suiss-blocks-catalog.md) (§2 and §5 block references).

**Revisions** (newest first; the internal review IDs behind each change are listed in §11):

- **Revision 5 (2026-11-05):**
  - Phones show Profile’s form first, then a compact preview of your reply.
  - Each step-up screen has one initial-focus rule.
  - "Can’t get the code?" offers the passkey route only to people who have a passkey.
  - Security suggests a passkey as the way back in if email is lost.
  - At sign-in, a live account takes precedence over a recently deleted one.
  - The signed-out download states its privacy trade-off and gets a rate limit.
  - One source of truth: `account.css` for values, this spec for behaviour, copy and states.
  - A note on why reserved space looks empty.
  - Phone Profile order, final: the form, with the save-error slot right under Save, then the compact preview with its visible caption. The 150 px distance target is replaced by the reason for this order. The compact-preview rules match canvas version `1793877977-4d5d`.
  - Review IDs moved to §11.
- **Revision 4 (2026-11-05):**
  - A "Can’t get the code?" path, with support-assisted recovery.
  - Export-link lifetimes, a signed-out "Download your data" entry, and a sign-in page for recently deleted accounts.
  - A skip link, the hand-off from a confirm to step-up, drawers that respect the keyboard, behaviour after a language change, typed-phrase matching and app logos.
  - The delete dialog becomes a form dialog, and the shiba becomes a small brand mark.
  - 56 boards.
- **Revision 3 (2026-11-05):**
  - A "photo removed" board.
  - Status slots reserve space by column width, and every status fits one line at 480 px. Shorter failure copy.
  - Segmented controls are a `fieldset` with a `legend`.
  - Canvas mechanics classes documented.
  - Equal-width footers for confirms only.
  - The board tables synced to `canvas.json`.
- **Revision 2 (2026-11-05):**
  - The 30 + 30-day deletion lifecycle, with its pages and emails.
  - An inline Undo for photo removal, and the long-email rule.
  - Accessible names start with the visible label, and countdowns start at 1:00.
  - Dialog rules: one height, forms, no close button, focus targets.
  - Failure, offline and loading copy for every page.
  - "Email code" as the one name.
- **Revision 1 (2026-11-04):** identity-only scope.
  - The Workspaces page and every workspace reference are removed.
  - "Back to Richie", and a workspace-neutral preview in Signal blue.
  - Deleting the account is never blocked.

Scratch files for this package live in `.design/wp3-1/`:

- `contrast.mjs` computes every contrast ratio in §7.1. The results are in `contrast-report.md`: all 49 required pairs pass, and 4 more are reported for reference. Revision 2 adds no new colours.
- `harness/build-boards.mjs` is the identity harness with two changes: it takes the stylesheet name, and `BOARD_THEME=dark` renders every board dark.

---

## Contents

1. [Design plan](#1-design-plan)
2. [Self-review and departures from the brief](#2-self-review-and-departures-from-the-brief)
3. [IA, URLs and navigation](#3-ia-urls-and-navigation)
4. [Page-by-page spec](#4-page-by-page-spec)
5. [State matrix](#5-state-matrix)
6. [Board list and canvas layout](#6-board-list-and-canvas-layout)
7. [CSS plan](#7-css-plan)
8. [Handoff mapping](#8-handoff-mapping)
9. [Open product questions](#9-open-product-questions)
10. [Appendix: sample data, photo stand-in, icons](#10-appendix)
11. [Changelog: internal review IDs](#11-changelog-internal-review-ids)

---

## 1. Design plan

### 1.1 Concept

The account area is **quiet suiss settings with one live object in it.** Four of the five pages are deliberately plain:

- the suiss sidebar shell;
- a 680 px column;
- grouped lists on Mist;
- one accent colour.

They are pages people visit to change one thing and leave.

The **Profile** page breaks the pattern once. Next to the photo and name fields sits a small, real-looking Richie messenger window, **as a customer sees it**, with your photo and first name on a reply bubble. Its header and the customer’s bubble are a full block of Signal blue. Typing in the name field or changing the photo updates it immediately. It is the only shadow, the only large area of colour and the only illustration in the area. It is specific to a messaging product: the place people meet your account is a chat window.

### 1.2 Palette

Six named colours. The hex values match the suiss tokens in `globals.css`, except where §7.1 records a measured deviation.

| Name | Role | Light | Dark |
|---|---|---|---|
| **Paper** | Page canvas | `#ffffff` | `#000000` (true black, not a tinted near-black) |
| **Ink** | Text, icons | `#1d1d1f` | `#f5f5f7` |
| **Mist** | Grouped lists, sidebar, preview stage | `#f5f5f7` | `#161617` (suiss `muted`) |
| **Slate** | Secondary text, field and control edges | `#6e6e73` text, `#86868b` edges | `#a1a1a6` text, `#6e6e73` edges |
| **Signal** | Filled buttons, switches, focus, and the preview’s messenger header and customer bubble. Links use a darker or lighter step | fill `#0071e3`, link `#0066cc` | fill `#0071e3` (kept from identity), link and focus `#2997ff` |
| **Alarm** | Errors and the destructive tint | `#d0000a`; text on the tint `#b31917` | `#ff5447`; text on the tint `#ff7263` |

Popovers (dialogs, sheets, toasts) are Paper in light and `#1d1d1f` in dark. That is suiss `popover`, one step above Mist so a dialog reads as raised.

### 1.3 Type

- **Face:** one family, the suiss stack `-apple-system, BlinkMacSystemFont, "Inter Variable", "Inter", sans-serif`. That is SF Pro on Apple devices and Inter elsewhere. Boards load Inter from Google Fonts (`wght@400;600;700`).
- **No monospace anywhere.** Codes use tabular figures, not a code font.
- **Weights:** 400 and 600 only, as in suiss. No 500. Button labels are 400.
- **The scale is fixed.** No type size depends on the viewport, neither its height nor its width.
- Every size maps to a Tailwind class.

| Role | Size / line height | Weight | Tracking | Tailwind | Used for |
|---|---|---|---|---|---|
| Title | 30 / 36 | 600 | −0.025em | `text-3xl font-semibold tracking-tight` | Page `h1`, account-deleted title |
| Heading | 18 / 28 | 600 | 0 | `text-lg font-semibold` | Section `h2`, dialog and sheet titles, empty-state title, sidebar name |
| Body | 16 / 24 | 400 | 0 | `text-base` | Row titles, input text, ledes (Slate), paragraphs |
| Label | 16 / 24 | 600 | 0 | `text-base font-semibold` | Field labels, the account row name on the mobile index |
| Detail | 14 / 20 | 400 | 0 | `text-sm` | Row descriptions, hints, errors, section descriptions, **button labels**, toasts, status lines |
| Caption | 12 / 16 | 400, or 600 for badges | 0 | `text-xs` | Badges, messenger name label and timestamp, avatar initials (sm) |
| Code glyph | 20 / 28 | 600, tabular, uppercase | 0 | `text-xl font-semibold tabular-nums` | The 8 code slots (suiss `input-otp`) |
| Action-sheet action | 18 / 28 | 400 | 0 | `text-lg` | Mobile confirm actions (suiss `alert-sheet-action`) |

The `h1` stays 30 px at every width: "Sign-in and security" wraps onto two balanced lines at 320 px, and that is accepted. Large buttons (`lg`, 44 px) use 16 px labels, as suiss does.

### 1.4 Layout concept

**Desktop shell** (≥768 px; shown at 1440):

```
┌───────────── 256 ──────────┬──────────────────────── main (1184) ───────────────────────────┐
│ [<-] Back to Richie        │  padding 40 top / 36 sides / 96 bottom                         │
│                            │                ┌──────────── 680 column ────────────┐          │
│ ────────────────────────── │                │ Profile                    (h1 30) │          │
│ (photo 64)                 │                │ This is how teammates and          │          │
│ Jamie Rivera       (18/600)│                │ customers see you.         (Slate) │          │
│ jamie.rivera@northwind…    │                │                                    │          │
│                            │                │ section ─ 40 ─ section ─ 40 ─ …    │          │
│ [o] Profile       (active) │                │                                    │          │
│ [o] Sign-in and security   │                └────────────────────────────────────┘          │
│ [o] Preferences            │                  column centred in main, same on every page   │
│ [o] Connected apps         │                                                                │
│ [o] Data and privacy       │                                                                │
│                            │                                                                │
│                            │                                                                │
│ [?] Help                   │                                                                │
│ [|->] Sign out             │                                                                │
└────────────────────────────┴────────────────────────────────────────────────────────────────┘
  Mist, 1 px Border end edge, sticky, full height       Paper
```

**Mobile index** (<768 px; `/account`). `>` stands for the trailing caret icon (Phosphor `CaretRight`); it is never a character in the copy.

```
┌──────────────── 390 ─────────────────┐
│ [<-] Back to Richie                  │  44 tall, Signal link colour
│                                      │
│ Account                     (h1 30)  │
│                                      │
│ ┌──────────────────────────────────┐ │  group 1: account row (links to Profile)
│ │ (photo 64) Jamie Rivera        > │ │
│ │            jamie.rivera@northw…  │ │
│ └──────────────────────────────────┘ │
│ ┌──────────────────────────────────┐ │  group 2: the other 4 pages
│ │ [tile] Sign-in and security    > │ │
│ │ [tile] Preferences             > │ │
│ │ [tile] Connected apps          > │ │
│ │ [tile] Data and privacy        > │ │
│ └──────────────────────────────────┘ │
│ ┌──────────────────────────────────┐ │  group 3
│ │ [tile] Help                      │ │
│ │ [tile] Sign out                  │ │
│ └──────────────────────────────────┘ │
└──────────────────────────────────────┘
   16 px gutters, 20 px between groups
```

**Mobile sub-page** (<768 px; the same base board, with the sidebar hidden):

```
┌──────────────── 390 ─────────────────┐
│ [<-] Account                         │  44 tall back link (to the index)
│ Sign-in and security        (h1 30)  │
│ Sign in with an email code or a      │
│ passkey. There’s no password to      │
│ forget.                              │
│                                      │
│ Passkeys                     (h2 18) │
│ Use your face, fingerprint or…  (14) │
│ ┌──────────────────────────────────┐ │
│ │ [key] MacBook Pro                │ │
│ │       iCloud Keychain            │ │
│ │       Added March 12.            │ │
│ │       Last used today.           │ │
│ │       [Rename] [Remove]          │ │  actions wrap under the text when the column is <480
│ │ ──────────────────────────────── │ │  hairline starts at the text edge (62 px)
│ │ [key] Pixel 8 …                  │ │
│ └──────────────────────────────────┘ │
│ [+ Add a passkey]                    │  section action, start-aligned
└──────────────────────────────────────┘
```

**Profile with the signature preview** (desktop column 680 = form 340 + gap 32 + stage 308). It is two columns when the column is at least 620 px: the form shrinks from 340 to 280 before the stage drops below 308.

```
Profile                                                          (h1)
This is how teammates and customers see you.

┌─ form column 340 ─────────────┐   ┌─ preview stage 308 (Mist, radius 18, padding 16) ─┐
│ Photo and name           (h2) │   │ How customers see you                        (h2) │
│ (photo 64)  [Change photo]    │   │ When you reply to a customer, this is what        │
│             Remove photo      │   │ they see.                                         │
│ JPG, PNG or WebP, up to 5 MB. │   │ ┌─ messenger 276 (Paper, radius 22, shadow) ────┐ │
│                               │   │ │▓▓ Chat with us                              ▓▓│ │
│ First name            (label) │   │ │▓▓ The team usually replies in a few         ▓▓│ │
│ [Jamie                      ] │   │ │▓▓ minutes                                   ▓▓│ │
│ Last name (optional)          │   │ │                     ┌───────────────────────┐ │ │
│ [Rivera                     ] │   │ │                     │ Hi! Can I change the  │ │ │
│ Customers see your first      │   │ │                     │ delivery address?  ▓▓ │ │ │
│ name. Teammates see your      │   │ │                     └───────────────────────┘ │ │
│ full name.            (Slate) │   │ │ Jamie                                (12)     │ │
│                               │   │ │ (24) ┌─────────────────────────┐              │ │
│ ( action row, 36 reserved )   │   │ │      │ Of course. Send me the  │              │ │
└───────────────────────────────┘   │ │      │ new address and I’ll    │              │ │
                                    │ │      │ update it right away.   │              │ │
                                    │ │      └─────────────────────────┘              │ │
                                    │ │ Just now                             (12)     │ │
                                    │ │ ( Write a message…                       [>] )│ │
                                    │ └───────────────────────────────────────────────┘ │
                                    └───────────────────────────────────────────────────┘
Email                                                                     (h2)
┌───────────────────────────────────────────────────────────────────────────────────────┐
│ jamie.rivera@northwind.coffee                                       [Change email]     │
│ We send email codes and security alerts here.                                          │
└───────────────────────────────────────────────────────────────────────────────────────┘
```

▓ is Signal `#0071e3` (`--ra-fill`): the messenger header and the customer’s bubble. No workspace name, logo or colour appears.

**Below a 620 px column** (tablet at 768, 200 % zoom, and phones), the order is:

1. Photo row.
2. First name, Last name.
3. The hint.
4. The action row, with the save-error slot directly under Save.
5. 32 px below: the **compact preview**. It keeps its visible caption, the "How customers see you" heading and its one-line description, above your reply: the name label, avatar and bubble. It has no messenger header, customer message, timestamp or composer.

**Why this order:** a labelled preview, and errors right next to Save, matter more than keeping the preview in view above the on-screen keyboard. The preview confirms a change; it isn’t needed to complete the task. Desktop is unchanged.

### 1.5 Alignment rules

1. **One left edge.** The `h1`, lede, section headings, groups, section actions and notes all start on the column’s left edge. Headings are not indented to the row text (this differs from the suiss block `px-1`).
2. **Same column on every page.** All five pages use the same 680 px column, centred in main, so the `h1` does not move when you switch pages. Profile’s preview lives inside that column.
3. **Row grid.** Rows are laid out as `[media 32] 14 [content 1fr] 8 [actions auto]` with 16 px side padding.
   - Hairlines start at the text edge: 62 px in when the rows have media, 16 px when they don’t.
   - Actions are end-aligned and vertically centred on single-line rows, and top-aligned with the title on rows of 2 or more lines.
   - When the column is under 480 px, actions wrap onto their own line, starting at the text edge (62 px).
4. **One height per context.**
   - In rows, every control is 32 px (`sm` buttons, `sm` select, `sm` segmented). It is 36 px when the column is under 480 px, the viewport is under 768 px, or the pointer is coarse.
   - In forms and dialogs, fields and buttons are 44 px.
   - In section footers, buttons are the suiss default of 36 px.
5. **Actions sit with what they act on.**
   - Section actions go under their group, start-aligned, never full width on desktop.
   - **Confirm dialogs** (every `confirm`: the alert-dialog pattern) use an equal-width 2-column footer, Cancel then the action.
   - **Form dialogs** (change email, rename, photo crop, step-up and the delete-account dialog, which has a field) right-align their footer, Cancel first.
   - Below 768 px, every footer stacks at full width with the commit button on top.
6. **Numbers are tabular** (`tabular-nums`): timers, counts, sizes and dates in lists.
7. **Measure.** Ledes and descriptions are at most 560 px wide (about 70 characters). Hints under fields follow the field width.
8. **Reserved space for states.** These slots are always in the layout, so a state change never moves the content below it (the identity rule):
   - **Section status slots:** on their own row under the section action. They reserve by column width: 1 line (20 px) at 480 px or more, 2 lines below 480 and 3 below 350. **Every status message fits one line at a 480 px column** (§4.0), so a message never outgrows its reservation.
   - **The name form:** the action row (36 px), plus an alert slot under it (40 px).
   - **The photo status slot:** 40 px.
   - **The code meta area:** 2 lines, or 3 below a 360 px container.
   - **Step-up and change-email descriptions:** 3 lines below a 400 px dialog width.
   - **Multi-step dialogs** keep one height across their steps (§4.0 Dialogs).
   - **Why empty space is kept.** In a clean state these reserved slots look empty: the gap under the name hint, under a section action, on step-up screen A, under the Preferences sections and in change-email step 1. That is intentional. They are the room the next state needs, so saving, an error or a status never pushes the content below it while someone is reading or about to tap. Keep them in the build; don’t collapse them to tighten the layout.
   - **Two documented exceptions:**
     - A field error may move the content below it by its own line, because the line isn’t reserved under every field.
     - At the shortest heights, the crop frame stops at its 160 px floor, and the Zoom slider may scroll below it. The scroll cue shows, and `+` and `-` still zoom.

### 1.6 Principles that make this area Richie’s

1. **Show the customer’s side.** When a setting changes what customers see (name, photo), show it as they see it, live. Nothing else gets a preview.
2. **Only the account, nothing else.** These are identity pages: who you are, how you sign in, what can use your account. Workspaces, their notifications and their data aren’t shown, linked or counted here; they live in Richie.
3. **No passwords, ever.** Our own copy uses the word "password" exactly once: "There’s no password to forget." Provider names such as "Google Password Manager" are exempt. Sign-in is an email code or a passkey, and the page says where codes go.
4. **Red marks the point of no return.** Row buttons that open a confirmation are neutral (outline). The tinted destructive button appears on the button that actually does it, and on the two standalone triggers whose section exists for that purpose: "Sign out of other devices" and "Delete account".
5. **Say what stops working.** Removing a passkey, signing a device out, removing an app’s access and deleting the account each say what stops working and what you can still do.
6. **Security in plain words, with real numbers.** "Each code works once and expires after 5 minutes", "You have 3 tries left", "We won’t ask again for 10 minutes".
7. **Calm feedback.** Saves confirm inline, next to what changed. A toast appears only when the thing you acted on has left the screen. Nothing celebrates.

---

## 2. Self-review and departures from the brief

### 2.1 Where the plan read like the generic default, and what changed

| Reads generic | Change | Why |
|---|---|---|
| A sidebar plus a stack of identical grey cards, page after page (the suiss and Apple settings default) | Kept for four pages on purpose, because it is the system. Profile breaks it with a two-column form and preview, and the preview carries the only shadow and the only full block of Signal blue. | Settings should be predictable. One deliberate exception makes the area memorable without making it noisy. |
| A "profile card" preview showing your avatar and name on a badge | Replaced with a working-looking messenger: a "Chat with us" header, a customer question, your reply with your face, a timestamp and a composer | The badge is a template idea. The messenger is the thing customers actually see, and it explains why your photo matters. |
| iOS colour tiles on the mobile index (`settings-device`) | Neutral tiles (`#e8e8ed` / `#2c2c2e`) with an Ink glyph, everywhere rows have media | A rainbow of tiles is decoration and competes with the preview. |
| suiss block section headings at 14 px semibold (iOS style) | Section `h2` at 18 / 600 (`text-lg`) with a 14 px Slate description | Account sections carry descriptions and actions. A 14 px heading is smaller than the 16 px row titles under it, which inverts the hierarchy. |
| Every destructive trigger tinted red | Row-level triggers are outline. Red is reserved for the confirming button and for the two dedicated triggers (principle 4) | With red row triggers, the passkey, session and app lists would be striped with pink pills, which reads as alarm. |
| "Place · Time" meta lines (suiss `users-detail`) | Sentences on their own line: "Portland, Oregon. Last active 2 hours ago." | Middle-dot meta strings are a template tell, and sentences read better aloud. |
| A "security score" ring (`log-security`) | Dropped. A short "Recent security activity" list instead | A score gamifies an account that has no passwords to weaken. Facts are more useful. |
| Step-up by redirecting to the sign-in page | An in-place "Confirm it’s you" dialog that keeps the page and names the action | You don’t lose your place, and the dialog says why it is asking. |
| Delete-account confirmation in a 340 px alert dialog | A 448 px dialog (suiss `dialog-destructive` pattern) with a consequences list and a typed phrase | The alert dialog can’t hold the consequences, and it can’t validate input (suiss guide §4.5). |

### 2.2 Field style: suiss filled, with a measured edge

suiss fields are filled: 44 px tall, a 4 % black fill (white 10 % in dark), 10 px radius, 16 px text, and a border only on focus or error. Identity drew outlined 52 px fields. **Settings uses the suiss field with one addition: a permanent 1 px `--input` edge** (`#86868b` light, `#6e6e73` dark).

- **Why not plain suiss:**
  - The filled field is 1.09:1 against the page in light and 1.20:1 in dark. In dark mode inside a dialog it nearly disappears.
  - WCAG 1.4.11 needs a 3:1 boundary or another cue. Our name fields sit beside a busy preview with no other cue.
  - The edge measures 3.62 and 3.33 (light), and 4.14 and 3.32 (dark).
- **Why not identity’s outline:** settings is built from suiss components. Keeping the fill, the 44 px height, the 10 px radius and the focus behaviour means the implementation only adds `border-input` to `Input`. It doesn’t restyle it.
- **Identity follow-up:** identity should move to 44 px fields so the two apps match. This is noted for the identity owner, not done here.

### 2.3 Other measured choices

| Choice | Value | Reason |
|---|---|---|
| Grouped-list radius | **18 px**, from `rounded-2xl` = 1.8 × 10 px. The brief said 16 | In this theme `rounded-2xl` is 18 px: the same as suiss cards, dialogs and `ItemGroup inset`. 16 would need an override on every group. |
| Dark `muted` | `#161617` (suiss), not identity’s undocumented `#1d1d1f` | It matches `globals.css`. `#1d1d1f` stays for dark popovers, where suiss puts it. |
| Dark filled primary | `#0071e3`, kept from identity | White on suiss `#2997ff` is 3.0:1 (the suiss guide measured this too). White on `#0071e3` is 4.70:1. Dark links and focus stay `#2997ff`. |
| Primary hover | `#0068d6` (darker) | The suiss `primary/80` hover lightens it and drops white text below 4.5:1. |
| Light link | `#0066cc`, not `#0071e3` | Links and plain buttons sit on Mist all the time here: `#0071e3` on `#f5f5f7` is 4.31:1, `#0066cc` is 5.11:1. |
| Plain-button hover | Underline, no fill | With the suiss `bg-accent` fill under `#0066cc` on Mist the text is 4.48:1. |
| Destructive button text | `color-mix(in oklab, var(--destructive), var(--foreground) 15%)`: `#b31917` light, `#ff7263` dark | Plain `--destructive` on its own tint is 4.37:1 on Mist and 4.07:1 in a dark dialog. The mix is suiss’s own hover formula, applied to the resting state. |
| Disabled and unavailable | No opacity-only states. "Unavailable" uses an outline look with Slate text (5.07 / 6.54) and a 3:1 edge, plus `aria-disabled` | Fixes identity’s 2.09:1 dimmed primary. |
| Outline button edge | `--input`, not `--border` | `--border` is 1.51:1 (light). `--input` is 3.62 on the page, 3.33 on Mist and 3.32 in a dark dialog. |
| Selected segment | Adds a 1 px `--ra-selected-edge` (`#86868b` / `#8e8e93`) and a 600 label to the suiss raised pill | The suiss pill is 1.09:1 against its track. The edge is 3.05 / 4.20. |
| Active nav item | suiss `bg-sidebar-accent` with a `sidebar-primary` icon, plus a 600 label | Fill and icon colour alone are colour-only cues. The weight change is a non-colour cue. The icon is 3.78 / 4.54 against the active fill. |
| Avatar fallback | Flat fill (`#6e6e73` / `#636366`) instead of the suiss gradient | The top of the suiss gradient puts white initials at 4.13:1 light and 4.44:1 dark. The flat fill gives 5.07 / 5.99. |
| Success colour | Tailwind `green-600` / `green-400`, icons only (3.30 / 12.05). Status text stays Ink | suiss has no success token, and green-600 text would be 3.2:1. |

### 2.4 Where I departed from the brief

| Brief | This spec | Reason |
|---|---|---|
| Grouped-list radius 16 | 18 | §2.3 |
| Step-up for change email, remove passkey, delete account, and "maybe sign out everywhere" | Step-up for **change email, add a passkey, remove a passkey, request a data download, delete account**. **Not** for signing out other devices | Adding a passkey is the most sensitive action: a stolen session could plant a permanent way in. A data export bundles everything. Signing other devices out only protects you, and it is what you reach for in a hurry, so it shouldn’t be gated. |
| Destructive actions use the tinted destructive button | Tinted on the confirming button and on the two dedicated triggers. Outline on row triggers | §1.6 principle 4, §2.1 |
| No "›" or "→" appended to links | Kept for all text. **Rows that navigate** (the mobile index) use the suiss trailing `CaretRight` **icon** as a disclosure indicator, `aria-hidden` | Push navigation needs an affordance. It is an icon in the trailing slot, not a glyph in link text (suiss guide §4.2). |
| Shared chrome via `dc-import` | The sidebar is one fragment board (`Account-sidebar`). The active item is drawn by CSS from `data-page` on the shell, because static imports can’t take props | One edit updates every page. The trade-off: the boards can’t carry `aria-current="page"`, so the implementation must set it (§8). Testers: don’t flag its absence on boards. |
| Mobile "settings-device style" index | Same, but neutral tiles, no search field, and no Profile row: the account row *is* Profile | Five destinations don’t need search. |
| (Revision) Workspaces page, invitations, ownership rules | Out of scope. Account settings are identity pages only | User decision, 2026-11-04 |

---

## 3. IA, URLs and navigation

### 3.1 Map

Account settings are routes of the identity app (`apps/identity`). Document titles use an en dash. When a submit fails validation, the title gets the prefix "Error: ".

| Nav label | URL | Document title | Phosphor icon | Contents |
|---|---|---|---|---|
| (mobile index) | `/account` | Account – Richie | | Below 768 px: the index. At 768 px and wider: replaced by `/account/profile` (`history.replace`) |
| Profile | `/account/profile` | Profile – Richie | `UserCircle` | Photo and name with the preview, email |
| Sign-in and security | `/account/security` | Sign-in and security – Richie | `Key` | Email codes, passkeys, where you’re signed in, recent security activity |
| Preferences | `/account/preferences` | Preferences – Richie | `SlidersHorizontal` | Appearance and language, date and time, emails from Richie |
| Connected apps | `/account/connected-apps` | Connected apps – Richie | `PlugsConnected` | OAuth grants |
| Data and privacy | `/account/data` | Data and privacy – Richie | `ShieldCheck` | Download your data, delete your account |
| (after asking to delete) | `/account/deleted` | Deletion scheduled – Richie | | Signed-out screen, rendered only right after a deletion request (one-time flag). A direct visit redirects to sign-in (§4.8) |
| (signing in, days 0–30) | `/signin/deletion-pending` | Account scheduled for deletion – Richie | | Identity sign-in interstitial after a successful code or passkey sign-in (§4.8) |
| (signed out, days 30–60) | `/export` | Download your data – Richie | | Without a link token: `Export-start` (enter the address). With the day-30 token (`/export?token=…`, reusable until day 60): `Export-request` (address prefilled). Then `/export/code` (Check your email – Richie) and `/export/requested` (Preparing your download – Richie) (§4.8) |
| (signed out) | `/export/expired` | This link has expired – Richie, or This download link has expired – Richie | | After day 60, every link shows the window-over page. Before it, an old download link shows the download-expired page, whose action goes back to `/export` (§4.8) |
| (identity sign-in, days 30–60) | `/signin`, email step | This account was deleted – Richie | | `Signin-deleted`, shown for an address deleted in the last 30 days. A handoff item for the identity canvas’s sign-in flow (§4.8) |

- **Deep links** (used by emails and the "Back" behaviour):
  - `/account/profile#email`
  - `/account/security#passkeys`, `#sessions` and `#activity`
  - `/account/data#download` and `#delete`
- **Security emails:**
  - "New sign-in" links to `#sessions`.
  - "Your download is ready" links to `#download`.
  - The deletion emails (§4.8) link to sign-in or to `/export`.
- **Dialogs are not routes.** Browser Back closes an open dialog first: push a history state on open and pop it on close. This matches the suiss `modal` and drawer behaviour on mobile.

### 3.2 Shell behaviour

| Width | Shell |
|---|---|
| **≥1024** (1280, 1440) | Sidebar 256 px (suiss), sticky, full height, Mist with a 1 px `--border` end edge. Main padding is 40 top, 36 sides, 96 bottom. The 680 px column is centred in main |
| **768–1023** (tablet; 768 tested) | Same sidebar, 256 px. Main is 512 px, so the column is 440 px. Profile stacks the preview under the form (the column is under 600). The name, email and nav labels in the sidebar truncate with an ellipsis |
| **<768** (390, 375, 320) | No sidebar and no tab bar. `/account` shows the index. Sub-pages show a 44 px back bar ("Account"), 16 px gutters and 8 px top padding below the bar. Dialogs become floating bottom drawers. Confirms become action sheets (suiss `confirm`, switch point 48rem) |
| **Height ≤540** | Nothing changes size. Dialogs and drawers cap at `100svh - 32px` and scroll inside, with the title and footer staying visible. The sidebar scrolls on its own |
| **200 % zoom** | At 1440 css px that is 720 px, so the mobile layout applies. It reflows with no horizontal scroll |

**Sidebar contents, top to bottom:**

**Skip link**: the first focusable element on every page of the shell, at every width, is `<a class="ra-skip" href="#page-title">Skip to content</a>`.

- It is visually hidden until focused. Then it shows at the top-start corner over the sidebar: a 44 px outline pill with the focus ring.
- It moves focus to the page `h1` (`id="page-title"`, `tabindex="-1"`). Without it, keyboard users would pass 8 sidebar stops on every page.
- The index and the solo pages don’t need it: their content comes first.

The sidebar container is a plain `div`. One `<nav aria-label="Account">` wraps everything in it, so there is no `aside` and no content outside a landmark.

1. Back link (`ArrowLeft` and "Back to Richie").
2. Hairline, drawn as a `border-top` so it survives forced colours.
3. Header: photo 64, name (Heading 18/600, two lines then an ellipsis), email (Detail 14, Slate). The email follows the long-email rule (§4.0):
   - the local part truncates first, and the domain is never truncated;
   - when the domain alone can’t fit, the address wraps after the @ onto a second line.
4. `nav` "Account" with the five items: 36 px, 16 px label, 16 px icon.
5. Footer, pushed to the bottom: Help (`Question`) and Sign out (`SignOut`).

### 3.3 Navigation and back

- **Desktop:** sidebar items are plain links (full page navigation within the SPA). The active item has `aria-current="page"`. Focus moves to the new page’s `h1` (`tabindex="-1"`) after navigation, and the document title updates.
- **Mobile:**
  - The index rows push to sub-pages.
  - The sub-page back link "Account" is `<a href="/account">`, not `history.back()`. A deep-linked page still goes back to the index.
  - Browser Back from a sub-page reached from the index returns to the index, at its previous scroll position.
- **"Back to Richie"** goes to the Richie app root (`/`). Richie decides where you land; account settings don’t track or name workspaces.
- **Sign out** (sidebar footer and mobile index) signs out this device only, with no confirmation, and lands on identity sign-in. "Sign out of other devices" lives on the Security page.
- **Unsaved changes** (the Profile name form): leaving the page with unsaved edits opens a suiss `confirm`:
  - Title: "Discard changes to your name?"
  - Description: "Your new name hasn’t been saved."
  - Buttons: Cancel and "Discard" (destructive).
  - No board; copy only.
- **Resizing across 768 px:** the index at desktop width is replaced by Profile. A sub-page keeps its URL.

---

## 4. Page-by-page spec

### 4.0 Shared grammar and copy rules

**Page header:**

- `h1` (Title), then the lede (Body, Slate, max 560 px). 8 px between them, 32 px to the first section.

**Section:**

- `<section aria-labelledby>`:
  - head: `h2` (Heading), then an optional description (Detail, Slate, 4 px below);
  - 12 px gap, then the group (suiss `ItemGroup variant="inset"`);
  - 12 px gap, then the optional foot.
- **The foot** has two rows:
  - first the section action, if any;
  - then, 8 px below it, the **status slot**, on its own row at every width. It is reserved even when empty, by the width of the column:

| Column width | Reserved | Why |
|---|---|---|
| ≥480 px | 1 line (20 px) | Every status message fits one line here |
| 350–479 px | 2 lines (40 px) | The same messages wrap to at most 2 lines |
| <350 px (phones under about 380 px) | 3 lines (60 px) | Headroom at 320 |

  - **The rule: every status message fits one line at a 480 px column.** That is 458 px of text after the 16 px icon and its 6 px gap, at 14/20, in Inter and in SF.
    - Check new or translated strings with `node .design/wp3-1/measure-status.mjs <strings.json> 458,328,266`.
    - A locale whose strings can’t meet the rule raises its reservation to 2 lines at every width; it never lets a message grow past its slot.
  - **Fixed slots** don’t depend on the section’s width: 2 lines (40 px) for the photo status slot and the name alert slot, except 1 line in a 420–619 px one-column Profile form, where every message fits one line. The code meta is as in §4.4.
  - Notes (`.ra-note`) come **after** the status slot, so a status message always sits directly under its group.
- Sections with no action and no status have no foot.
- **One section pattern**: every section is `<section aria-labelledby="{h2 id}">`. A `fieldset` is used only for a radio group (the segmented controls), never to wrap a whole section or a group of switches.
- 40 px between sections, measured from the end of the foot.

**Row (suiss `Item`):**

- Media: a 32 px tile, radius 8. Content: title (Body 16/400, inside inset groups as suiss does) and description lines (Detail, Slate).
- Actions: `sm` buttons, a switch, a select, a segmented control or a trailing value.
- Height: one line = 52 px; title + 1 line = 76 px; title + 2 lines = 96 px. Padding is 14 16; media and actions sit in a 24 px box on the title line, so 32 and 36 px controls never change a row’s height.

**Status slot** (`.ra-slot`, one grid cell, so the polite status and an alert can occupy the same place):

- **Polite status:** `<p class="ra-status" role="status">`, always present and empty when idle.
  - Success: a `CheckCircle` icon in success colour, then Ink text ending with a period, for example "Name saved."
  - Busy: the spinner (`aria-hidden`) and Slate text.
- **Errors:** a `<p class="ra-status is-error" role="alert">` with a `WarningCircle` icon and Alarm text, in the same cell.
- **Static Slate lines** that are not news (for example "You’re not signed in anywhere else.") use `.is-quiet` and no live role.
- **When to use `role="alert"`:** only when focus does **not** move to the message, or to the control whose `aria-describedby` includes it. A field error that focus moves to is read once through `aria-describedby` and carries no `role`.

**Toasts** (suiss toast, bottom-end on desktop and bottom-centre on mobile):

- **Rule:** a toast appears only when the thing acted on has left the screen: a passkey removed, a device signed out, app access removed.
- **Toasts carry no action buttons.** Undo lives inline (Remove photo, §4.2), so no recovery depends on a timed toast (WCAG 2.2.1).
- They time out after 5 s and pause while hovered or focused. The viewport is the labelled "Notifications" region (F6 reaches it). A toast is never the only place a result is shown.

**Buttons:** five variants. suiss `secondary`, `tinted` and `link` are not used.

| Variant | Use |
|---|---|
| default (filled, `#0071e3`) | The one commit per form or dialog, plus at most one recommended action per page (Add a passkey; Download when ready) |
| outline (`--input` edge) | Every neutral action, including Cancel. One exception: on step-up screen A (§4.4), Cancel is the third stacked option and is plain, so the two real choices stay ahead of it |
| plain (link colour, underline on hover) | Low-stakes inline actions: "Remove photo", "Undo", "Email me a code instead", "Send a new code", "Use a passkey instead" |
| destructive (tinted) | §1.6 principle 4 |
| unavailable (`aria-disabled`) | Countdown buttons only |
| ghost `icon-sm` | **Only** the toast’s Dismiss (×), `aria-label="Dismiss"` |

**Busy buttons:**

- They keep their variant colours, show the suiss spinner (`data-icon="inline-start"`, `aria-hidden`, no `role`) and a gerund ending in "…", and set `aria-disabled="true"`.
- Never use `disabled`, which drops focus.

**Dialogs** (every modal, confirm, drawer and sheet in this area):

- **No close (×) button.** Cancel is always visible.
- **Dismissal:**
  - `Escape` closes any dialog or confirm and counts as Cancel.
  - Clicking outside closes nothing, so typed input is never lost by accident.
  - Browser Back closes the dialog first (§3.1).
- **Every dialog is a `<form>`** around its body and footer, so `Enter` submits the commit button (`enterkeyhint="go"` on code inputs). The commit is `type="submit"`, and Cancel is `type="button"`.
- **One height per dialog.**
  - Dialogs that change step or state keep the height of their tallest state at the current width: change email steps 1 and 2; step-up screens A, B and C and all their states; the delete dialog followed by step-up. Their `dialog-content` gets that `min-height`. The title and the footer therefore never move.
  - The commit button’s `min-width` is its widest label across states. For example, "Confirm" reserves the width of "Send a new code in 0:41", so Cancel doesn’t jump.
- **Async confirms stay open:**
  - The confirm (or dialog) stays open while its action runs. The commit is busy and Cancel is `aria-disabled`.
  - It closes on success. On failure, an alert appears inside the dialog above the footer, and both buttons are enabled again.
- **Initial focus:** listed for each dialog in §7.4. On close, focus returns to the trigger, unless the action’s own rule names another target.
- **From a confirm to step-up** (remove passkey, and any confirm whose action needs step-up). The confirm is an `AlertDialog`/`AlertSheet` and step-up is a `Dialog`/`Drawer`, so one can’t become the other:
  1. Pressing the confirm’s action closes the confirm.
  2. Step-up opens at once, its description naming the action ("To remove the “Pixel 8” passkey, …").
  3. Focus follows the per-screen rule (§4.4), as it does from every entry path: screen A’s title, screen B’s code input, or screen C’s title.
  4. **Cancel or Escape** closes step-up, changes nothing, and returns focus to the button that started it (for example the row’s **Remove**).
  5. **Failures** of the action after a successful step-up show inside the step-up dialog, above its footer.
  6. Form dialogs (change email, delete account) stay one dialog: step-up replaces their content in place.
- **Drawers with inputs and the on-screen keyboard** (step-up code, new email, rename, typed phrase):
  - When the keyboard opens, the drawer grows to the visual viewport, not the layout viewport.
  - The body scrolls, and the footer stays visible above the keyboard.
  - The focused field scrolls into view (`block: "nearest"`).
  - The drawer never extends under the keyboard, and the page behind never scrolls.
- **Short screens:** when the body scrolls, a hairline appears on the edge of the header and the footer that content passes under, as a scroll cue.

**Copy rules:**

- US English: "canceled", "recognize", "color".
- Sentence case everywhere, including nav items and buttons.
- "You" for the user, "we" for Richie. Contractions, curly apostrophes and a real ellipsis.
- Digits for numbers; m:ss for timers. No Oxford comma.
- Errors say what to do, with real numbers. Never "invalid", "oops" or an exclamation mark, except inside sample customer messages.
- **One verb for a new code: "Send a new code"** everywhere. Identity mixes "Resend code" and "Send a new code" and should adopt this.
- **Countdowns start at 1:00**, because Duende allows a new send only after at least 60 s. Drive the timer from the server’s next-allowed time, not from a client guess. Boards draw the arrival state at "1:00".
- The example address is **name@example.com**, in both the placeholder and the error.
- **Dates:**
  - **This year:** "March 12". **Another year:** "January 4, 2027".
  - **Times:** "9:14 AM".
  - **Within the last 7 days:** "Today", "yesterday" or "2 hours ago". Older dates use the date ("Last active October 27").
- **One name per thing**:

| Thing | Name in copy | Not |
|---|---|---|
| The 8-character code we email | **email code** ("Email codes", "Sign in with an email code", "use a passkey or an email code"); inside a flow, "the code". sr label: "Email code, 8 letters and numbers" | sign-in code, verification code, a code from your email |
| A signed-in browser | **device** ("Where you’re signed in", "Sign out of other devices", "Signed out of 2 other devices") | session |
| An OAuth grant | **connected app** | integration, authorization |
| The account export | **your data** / **download** ("Download your data", "Request download") | export, archive |
| Deleting the account | **delete your account** ("Delete your account", "Delete account", "Delete your account?") | close, remove, deactivate |

**Screen-reader rules:**

- Every icon is `aria-hidden`. Icon-only buttons have an `aria-label`.
- **Accessible names start with the visible label** (WCAG 2.5.3). Add context after it, as a sr-only *suffix* ("Remove the Pixel 8 passkey" for "Remove"), or through `aria-describedby` when the context is already on screen: Security’s "Change email" is named "Change email" and described by the address. Never use a prefix.
- Avatars next to the name use `alt=""`.
- Each page’s `h1` takes focus after navigation.
- When a row is removed, focus moves to the section’s `h2` (`tabindex="-1"`). On a page with no `h2`, it moves to the `h1`, or to the empty-state title when the list becomes empty.
- Text in another language carries `lang`, for example language names in the picker (§4.5).

**Long emails** (one rule for every place an address is shown):

- **One accessible string.** The full address is rendered once as sr-only text, and the visual pieces (local part, "@", domain) are `aria-hidden`. Screen readers and braille get one word, and link names stay clean.
- **Truncation uses `text-overflow: ellipsis`** on the local-part span, built into the `EmailText` component. Never a generated pseudo-element: on the canvas, one overlapped the last visible letter.
- **The domain is never truncated**: it tells a work address from a personal one.
- **Rows** (Profile email, Security email codes, dialog descriptions): the address wraps after the @ when it doesn’t fit, and the domain wraps at dots or hyphens only if it is longer than the whole line.
- **Sidebar and index header:**
  - One line while the domain fits with at least 3 characters of the local part ("jam…@northwind.coffee").
  - Otherwise two lines: line 1 is the local part and "@", truncated at the end if it must be; line 2 is the whole domain, which wraps at dots or hyphens if it is longer than the line.

**Failure, offline and loading copy**. Every action that talks to the server has one phrase, used in three patterns:

| Cause | Pattern |
|---|---|
| Offline (known) | "We couldn’t {phrase} because you’re offline." |
| Network failure or timeout | "We couldn’t {phrase}. Check your connection." |
| Server error (5xx) | "We couldn’t {phrase}. Try again in a moment." |

| Page | Action | {phrase} | Where it shows |
|---|---|---|---|
| Profile | Save name | save your name | Name alert slot under the action row |
| Profile | Upload photo | upload your photo | Photo status slot |
| Profile | Save the cropped photo | save your photo | Inside the crop dialog, above the footer |
| Profile | Remove photo | remove your photo | Photo status slot |
| Profile | Change email (send code) | send a code | Inside the dialog, step 1 |
| Profile | Change email (confirm) | change your email | Inside the dialog, step 2 |
| Security | Add a passkey (server rejects the new credential) | save your new passkey | Passkeys status slot |
| Security | Rename passkey | rename this passkey | Inside the dialog |
| Security | Remove passkey | remove the “Pixel 8” passkey | Inside the confirm, or inside step-up when it ran |
| Security | Sign out one device | sign out Safari on iPhone | Sessions status slot |
| Security | Sign out of other devices | sign out your other devices | Inside the confirm |
| Step-up | Send a code | send a code | Code meta, line 1 |
| Step-up | Check the code | check your code | Code meta, line 1 |
| Preferences | Any change (the control reverts) | save that change | The section’s status slot |
| Connected apps | Remove access | remove access for Tallybird | Inside the confirm |
| Data | Request download | request your download | Download status slot |
| Data | Delete account | delete your account | Inside the delete dialog |
| Export (§4.8) | Send a code / request download | send a code / request your download | Under the button / code meta |

- **Offline, on every page:** a suiss `Alert` (default variant, `WifiSlash` icon) sits between the lede and the first section, with title "You’re offline" and description "Changes won’t save until you’re back online." It uses `role="alert"`, because it appears in response to a change. Controls stay operable. `Preferences-offline` draws it.
- **Initial loading, on every page:**
  - Static copy (`h1`, lede, `h2`s, descriptions, field labels, row titles) renders at once. Only data is replaced by skeletons shaped like the real content.
  - Each loading group gets `aria-busy="true"`.
  - A sr-only `role="status"` paragraph sits **outside** the busy groups, as the first child of `.ra-page`.
  - Foot actions are hidden while loading; their status slots stay reserved.

| Page | Skeletons | sr status |
|---|---|---|
| Profile | Avatar 64 circle; the two inputs as 44 px filled bars, labels shown; in the preview, the avatar circle and a 60 px name bar (the conversation text is static); the email row title bar | Loading your profile… |
| Sign-in and security | §4.3 | Loading your sign-in settings… |
| Preferences | Row titles shown. Controls become bars: segmented 32 × 180, picker 36 × 200, switch 24 × 52, trailing value 20 × 80 | Loading your preferences… |
| Connected apps | 2 rows: tile, then lines 16 × 30 %, 12 × 70 % and 12 × 50 % | Loading your connected apps… |
| Data and privacy | The download row: title and description bars, with the action hidden. The delete section is static and shown | Loading your data settings… |

### 4.1 Shell: sidebar and mobile index

**Sidebar (`Account-sidebar`):**

| Element | Copy | Notes |
|---|---|---|
| Skip link (before the sidebar) | Skip to content | `a.ra-skip` to `#page-title`, hidden until focused (§3.2) |
| Back link | Back to Richie | `<a>`, `ArrowLeft` icon, 36 px menu-button look, `href="#richie"` on boards (the Richie app root) |
| Header photo | (photo stand-in, 64) | `alt=""`; the name follows |
| Header name | Jamie Rivera | Heading 18/600, 2-line clamp |
| Header email | jamie.rivera@northwind.coffee | Detail Slate. Long-email rule (§4.0): one sr string, the local part truncates, and it wraps after the @ when the domain can’t fit |
| Container | (sr) Account | A plain `div` with `<nav aria-label="Account">` around everything below it, including the back link and footer. No `aside` |
| Items | Profile; Sign-in and security; Preferences; Connected apps; Data and privacy | Each `<a data-nav="profile\|security\|preferences\|apps\|data">` linking `Main.dc.html`, `Security.dc.html`, `Preferences.dc.html`, `Connected.dc.html` and `Data.dc.html` |
| Footer | Help; Sign out | Help is a link (`href="#help"`). Sign out is a `<button>`. A `border-top` hairline separates it |

**Mobile index (`Account-index`, `/account` below 768 px):**

| Element | Copy | Notes |
|---|---|---|
| Back link | Back to Richie | 44 px, link colour, `href="#richie"` |
| `h1` | Account | |
| Landmark | (sr) Account | `<nav aria-label="Account">` wraps groups 1–3 (below 768 px this is the navigation) |
| Account row | **Jamie Rivera** / jamie.rivera@northwind.coffee | Photo 64 (suiss avatar `xl`). Label 16/600 title, Detail email (long-email rule). `href="Main.dc.html"`. The title ends with a sr-only suffix ", profile", so the name starts with the visible label: "Jamie Rivera, profile, jamie.rivera@northwind.coffee" |
| Rows | Sign-in and security; Preferences; Connected apps; Data and privacy | Tile with the page icon, Body title, trailing caret |
| Group 3 | Help; Sign out | Help is a link row. Sign out is a `<button>` row with no caret. Its contents are `span`s only (phrasing content) |

### 4.2 Profile (`/account/profile`, board `Main`)

- `h1`: **Profile**
- Lede: **This is how teammates and customers see you.**

**Section A: Photo and name** (`h2` "Photo and name"):

- **Two columns** when the column is at least 620 px: form 280–340, gap 32, stage ≥308.
- **Below 620:** photo row, First name, Last name, the hint, then the action row with the save-error slot directly under Save. Then, 32 px below, the **compact preview** (§1.4). It keeps its visible caption: the `h2` "How customers see you" and its description. Above your reply it shows only the name label, avatar and bubble, without the messenger header, the customer message, the timestamp or the composer. The DOM order is the visual order. Errors next to Save, and a labelled preview, come before keeping the preview above the keyboard: the preview confirms a change, and isn’t needed to finish one.

| Row | Value | Control | Action and behaviour |
|---|---|---|---|
| Photo | Jamie’s photo (64, `xl`) | **Change photo** (outline `sm`, opens the file picker `accept="image/jpeg,image/png,image/webp"`). **Remove photo** (plain `sm`). With no photo, the only button is **Upload photo** (outline `sm`) | Picking a file opens **Adjust your photo** (below). **Remove photo:** see the photo states below. **Layout:** the buttons share one line. When the controls box is under 232 px (phones under about 380 px), they stack on two lines, and the second line is reserved even when only Upload photo shows, so removing a photo never moves the form. |
| Photo status slot (reserved 40 px, 2 lines; 1 line in a 420–619 px one-column form, where every photo message fits) | Idle: the hint "JPG, PNG or WebP, up to 5 MB." | | The hint is linked to the button by `aria-describedby`. Status messages and errors replace the hint in the same slot (see the photo states). |
| First name | Jamie | suiss `Input` (filled, 44 px, `--input` edge), label **First name**, `autocomplete="given-name"`, `autocapitalize="words"`, `enterkeyhint="done"` | The preview updates on each keystroke (150 ms debounce) using the trimmed value. |
| Last name | Rivera | Same, label **Last name (optional)**, `autocomplete="family-name"` (§9 Q5) | |
| Description (under both) | Customers see your first name. Teammates see your full name. | | `id` referenced by both inputs |
| Action row (36 px) | Clean: the polite status line only. Dirty: **Cancel** (outline) and **Save** (default, 36 px). The status line stays in the same row (it takes the remaining width and never wraps under the buttons) | | Cancel restores the saved values (the preview follows). Save validates, then saves. |
| Alert slot (reserved 40 px, always present; 1 line in a 420–619 px one-column form), directly under Save | Empty, except for the save-failed alert | | It keeps everything below fixed in every state. |

**Name states and copy:**

| State | What shows |
|---|---|
| Dirty (no board; same as Saving without the spinner) | Cancel and Save visible; the preview shows the new name |
| Saving (busy) | Save reads "Saving…" with the spinner, `aria-disabled`; Cancel `aria-disabled` |
| Saved (`Profile-name-saved`) | **Board data:** first name changed to "Jay". The field, the preview ("Jay") and the sidebar ("Jay Rivera") all show it. The buttons are gone, and the status line reads "Name saved." (polite) |
| First name empty (`Profile-name-error`) | The field is `aria-invalid`, with the error "Enter your first name." (no `role`: focus moves to the field, which reads it through `aria-describedby`). The document title becomes "Error: Profile – Richie". The preview keeps the **last saved** first name, "Jamie" |
| Over 50 characters | "Use 50 characters or fewer." |
| Save failed (`Profile-save-failed`) | **Board data:** the field and the preview show "Jay", and the sidebar keeps "Jamie Rivera". The alert slot (`role="alert"`) reads "We couldn’t save your name. Check your connection." (offline and server variants in §4.0). Save is enabled again |

**Photo states** (all in the photo status slot; no toast):

| State | What shows | Focus |
|---|---|---|
| Uploading | The avatar shows the spinner over a 50 % scrim. Change photo reads "Uploading…" (`aria-disabled`). The status reads "Uploading your photo…" | Stays on the button |
| Saved (after the crop dialog) | "Photo saved." (polite, success icon). The photo updates in the sidebar, the photo row and the preview | Returns to Change photo |
| Removed | The avatar shows the initials "JR" in the sidebar, the photo row and the preview. The slot shows "Photo removed." (polite) followed by a plain **Undo** button. The button is outside the live paragraph, with sr name "Undo, restore your photo". Undo stays until you leave the page or change the photo again; it is not timed | Moves to **Upload photo**, because the button that was pressed is gone |
| Restored (Undo) | The photo returns. The status reads "Photo restored." | Moves to Change photo |
| Errors (`role="alert"`) | **Too large:** "That photo is 8.4 MB. Choose one that’s 5 MB or smaller." | Stays on the button |
| | **Wrong type:** "That file isn’t a photo we can use. Choose a JPG, PNG or WebP." | |
| | **Too small:** "That photo is too small. Choose one at least 128 × 128 pixels." | |
| | **Upload or remove failed:** the §4.0 patterns ("We couldn’t upload your photo. Check your connection.") | |

**Adjust your photo** (dialog, board `Profile-photo-crop`):

| Element | Copy |
|---|---|
| Title | Adjust your photo |
| Description | Drag to move it. Customers see it in a circle. |
| Crop area | 280 × 280 (shrinks to fit at short heights, down to 160, to keep the Zoom slider in view where it can: `min(280px, 100svh - 360px)`. Below the floor the slider may scroll, as documented in §1.5 rule 8. This is the one exception to "nothing changes size"). The photo shows under a circular mask with a 50 % scrim outside the circle; the pointer is a "move" cursor |
| Crop area, role and keys | A focusable `div` with `tabindex="0"`, `role="application"` (scoped to this area only, so screen-reader browse mode passes keys through), `aria-roledescription="photo position"`, `aria-label="Photo position"` and `aria-describedby` pointing at the description plus a sr sentence: "With a keyboard, use the arrow keys to move your photo, plus and minus to zoom, and Home to center it." **Keys:** arrows move 4 px (Shift + arrow: 20 px); `+` and `-` zoom by 10 %, in step with the slider; `Home` recenters. After each keyboard change, a polite sr status outside the area (debounced 500 ms) reads the position, for example "Showing 40 % from the left and 30 % from the top, zoom 120 percent." **Initial focus:** the crop area |
| Zoom | suiss slider, label **Zoom** (visible), 100–300 %. sr value: "120 percent". The track stays 4 px; below 768 px and on coarse pointers the slider’s box is 44 px tall |
| Footer | **Cancel** (outline), **Save photo** (default); busy "Saving…". A save failure shows inside the dialog, above the footer (§4.0) |
| After saving | The dialog closes; see the photo states |

**Preview: How customers see you** (`<figure>` beside the form):

| Element | Copy and behaviour |
|---|---|
| `h2` (in `figcaption`) | How customers see you |
| Caption | When you reply to a customer, this is what they see. |
| Messenger header (Signal `--ra-fill`, white text) | No mark, logo or workspace name. Title "Chat with us" (14/600). Subline "The team usually replies in a few minutes" (12/16, solid white: 85 % white is 3.82:1 on blue) |
| Customer message (end-aligned, Signal bubble, white text) | Hi! Can I change the delivery address on my order? |
| Your reply (start-aligned) | Name label "Jamie" (Caption, Slate), avatar 24 at the bottom of the bubble, teammate bubble: "Of course. Send me the new address and I’ll update it right away." Then "Just now" (Caption, Slate) |
| Composer (drawn, not an input) | A pill with the `--input` edge and the placeholder "Write a message…", plus a 28 px Signal circle with `PaperPlaneRight` |
| Live behaviour | The name label shows the current first name, truncated with an ellipsis to the bubble width (max 220 px). The avatar shows the current photo, or the initials. Updates crossfade over 150 ms (none with reduced motion) |
| Neutral by design | The preview never shows a workspace name, logo or colour. It shows what your account contributes (photo and name), not any one workspace’s messenger |
| Accessibility | The messenger drawing is `aria-hidden="true"`. The figure has a sr-only paragraph that is updated with the name but is **not** a live region: "Preview: a customer conversation in a Richie messenger. Your reply shows your photo and the name Jamie." |
| Dark | Messenger window `#1d1d1f`, teammate bubble white 10 %. The header and customer bubble stay `#0071e3` (`--ra-fill`, the same in both themes). Rationale: customers see the messenger in their own appearance; we show yours |

**Section B: Email** (`h2` "Email", `id="email"`):

| Row | Value | Control |
|---|---|---|
| Email | jamie.rivera@northwind.coffee (Body; wraps after the @, never truncated: §4.0 long emails) | **Change email** (outline `sm`; sr "Change email address") |
| Description | We send email codes and security alerts here. | |

**Change email flow.** Step-up first if needed (§4.4). Then one dialog with two steps; the title changes per step. The dialog keeps the height of step 2 throughout (§4.0 Dialogs), so the title and footer stay in place. Initial focus: the New email field (step 1), then the code input (step 2).

1. **Step 1** (`Profile-email-change`; the board shows the field filled with "jamie@rivera.studio" and focused):
   - Title "Change your email".
   - Description "We’ll send a code to the new address to check it’s yours. Your current address keeps working until then."
   - Field label **New email**: `type="email"`, `autocomplete="email"`, `inputmode="email"`, `autocapitalize="none"`, `spellcheck="false"`, placeholder "name@example.com".
   - Footer **Cancel**, **Send code**; busy "Sending code…".
   - Errors:
     - "Enter your new email address."
     - "Enter a full email address, like name@example.com."
     - "That’s already your email. Enter a different one."
     - "That email already belongs to another Richie account. Use a different one." (§9 Q4)
     - "We couldn’t send a code. Check your connection."
2. **Step 2** (`Profile-email-code`):
   - Title "Check your new email".
   - Description "Enter the code we sent to **jamie@rivera.studio**."
   - Code: 8 slots, 4 + 4 (§4.4 code rules). The meta area shows "Codes expire after 5 minutes." and "Send a new code in 1:00". The same polite sr status element as step-up screen B follows the meta. It carries "Checking your code…", "You can send a new code now." and "We sent a new code.".
   - Under the meta: plain **Use a different email** (returns to step 1 with the field filled).
   - Footer **Cancel**, **Change email**; busy "Checking code…".
   - Errors as in §4.4.
3. **Done:**
   - The dialog closes and the email row shows the new address.
   - The status line under the group reads "Email changed. We’ve let your old address know." The row already shows the new address, where codes go from now on.
   - A security email goes to the old address.
   - Where the browser supports it, call `PublicKeyCredential.signalCurrentUserDetails()` so password managers show the new email on your passkeys.

**Extreme data (`Profile-extreme`)**, sample in §10:

- Name "Maximiliana-Alexandra Featherstonehaugh-Okonkwo", no photo (initials "MF").
- A long email:
  - In the sidebar, the domain `northwind-specialty-coffee-roasters.co.uk` can’t fit on one line, so the address uses two lines: "maximiliana.feathersto…@" on line 1, then the whole domain (wrapping at a hyphen only if needed).
  - In the email row, the address wraps after the @.
  - The domain is never truncated (§4.0).
- The preview label is "Maximiliana-Alexandra", truncated.

### 4.3 Sign-in and security (`/account/security`, board `Security`)

- `h1`: **Sign-in and security**
- Lede: **Sign in with an email code or a passkey. There’s no password to forget.**

**Section: Email codes** (`h2` "Email codes")

| Media | Title | Description | Action |
|---|---|---|---|
| `EnvelopeSimple` | jamie.rivera@northwind.coffee | Always on. Each code works once and expires after 5 minutes. | **Change email** (plain `sm` link to `Main.dc.html#email`; the accessible name is the visible label "Change email", and `aria-describedby` points at the row title, the address). As a trailing plain `sm` row action, it pulls back 12 px at the end so its text lines up with the actions edge) |

**Section: Passkeys** (`h2` "Passkeys", `id="passkeys"`)

- Description: **Use your face, fingerprint or screen lock to sign in. Richie never sees your face or fingerprint.**

| Media | Title | Description (2 lines) | Actions |
|---|---|---|---|
| `Key` | MacBook Pro | iCloud Keychain / Added March 12. Last used today. | **Rename** (outline `sm`, sr "Rename the MacBook Pro passkey"), **Remove** (outline `sm`, sr "Remove the MacBook Pro passkey") |
| `Key` | Pixel 8 | Google Password Manager / Added October 28. Last used October 30. | same |

- Foot: **Add a passkey** (default, `Plus` icon), then the status slot on its own row (reserved by width, §4.0).
- **Empty (`Security-no-passkeys`):** one row (media `Key`).
  - Title "No passkeys yet".
  - Description "Add a passkey so you can still sign in if you lose access to your email."
  - The foot is unchanged.

**Add-a-passkey states** (in the passkeys foot):

| State | Button | Status line |
|---|---|---|
| Idle | Add a passkey | (empty) |
| Step-up needed | Opens "Confirm it’s you" (§4.4) first | |
| Waiting (`Security-passkey-waiting`) | Spinner and "Waiting for your device…", `aria-disabled` | "Follow the prompt on your screen to finish." (Slate) |
| Canceled or timed out (`Security-passkey-failed`) | Add a passkey | (alert) "The passkey prompt closed. Try again." |
| Already registered (`InvalidStateError`) | Add a passkey | (alert) "You already have a passkey on this device. Use it to sign in." |
| Not supported | Button hidden | (Slate) "This browser can’t create passkeys. Try the latest Chrome or Safari." |
| Added | Add a passkey | The new row appears first ("Added just now"), named after its provider (for example "iCloud Keychain"). Status: "Passkey added. Rename it so you can tell your passkeys apart." |
| At the limit (10) | Button hidden | "You have 10 passkeys, the limit. Remove one to add another." |

**Rename** (dialog, no board): title "Rename passkey"; description "Pick a name that helps you recognize it, like the device you use it on."

- Field **Name**, prefilled, `autocomplete="off"`.
- Footer **Cancel**, **Save**; busy "Saving…".
- Errors: "Enter a name for this passkey." / "Use 40 characters or fewer."
- Done: the dialog closes, the row updates, and the status line reads "Passkey renamed."

**Remove** (`confirm`, board `Security-remove-passkey`; action sheet on mobile, `Security-remove-passkey-mobile`):

| Element | Copy |
|---|---|
| Title | Remove the “Pixel 8” passkey? |
| Description | You won’t be able to sign in with it anymore. You can still use your other passkey or an email code. |
| Description, last passkey | It’s your only passkey. After you remove it, you’ll sign in with an email code. |
| Actions | **Cancel**, **Remove passkey** (destructive) |
| Then | Step-up if needed (§4.4): the confirm closes and step-up opens, as in the §4.0 hand-off. The row is removed. Toast "Passkey removed" with the description "You can also delete it from Google Password Manager." Focus goes to the "Passkeys" `h2`. Where supported, call `PublicKeyCredential.signalUnknownCredential()`. On failure, the alert shows inside the confirm, or inside step-up when it ran (§4.0) |

**Section: Where you’re signed in** (`h2`, `id="sessions"`)

- Description: **Don’t recognize a device? Sign it out, then check your passkeys and your email account’s security.**

| Media | Title | Description | Action |
|---|---|---|---|
| `Laptop` | Chrome on macOS | Portland, Oregon. Active now. | Badge "This device" (suiss badge `secondary`: Slate 12/600, no fill) |
| `DeviceMobile` | Safari on iPhone | Portland, Oregon. Last active 2 hours ago. | **Sign out** (outline `sm`, sr "Sign out of Safari on iPhone") |
| `Desktop` | Firefox on Windows | Seattle, Washington. Last active October 27. | **Sign out** |

- Foot: **Sign out of other devices** (destructive, default size), then the status slot. The button shows only when there are other devices.
- **Sign out one device:** immediate, no confirmation. The button reads "Signing out…", then the row is removed. Toast "Signed out of Safari on iPhone".
- **Sign out of other devices** (`confirm`, no board; copy only):
  - Title "Sign out of 2 other devices?"
  - Description "You’ll stay signed in here. Anyone using Safari on iPhone or Firefox on Windows will need a code or passkey to get back in." With more than 3 devices: "Anyone using them will need…".
  - Actions **Cancel**, **Sign out** (destructive). No step-up.
  - Toast "Signed out of 2 other devices".
- **Only this device:** the foot shows the static Slate line "You’re not signed in anywhere else." (`.is-quiet`, no live role).

**Section: Recent security activity** (`h2`, `id="activity"`)

- Description: **The last 30 days. We also email you when something changes.**

| Media | Title | Description |
|---|---|---|
| `SignIn` | Signed in with a passkey | Today at 9:14 AM. Chrome on macOS in Portland, Oregon. |
| `Key` | Added the passkey “Pixel 8” | October 28 at 6:02 PM. Chrome on Android in Portland, Oregon. |
| `EnvelopeSimple` | Signed in with an email code | October 26 at 8:47 AM. Firefox on Windows in Seattle, Washington. |

- More than 5 entries: the last 5 are shown, then plain **Show all activity**, which opens the suiss `paged-list` (no board).
- Empty: "Nothing in the last 30 days." The board `Security-no-passkeys` is not empty here: this device is active, so it keeps one row ("Signed in with an email code / Today at 9:14 AM. Chrome on macOS in Portland, Oregon.").

**Loading (`Security-loading`):**

- The `h1`, lede, `h2`s and descriptions render immediately, because they are static.
- Each group shows skeleton rows shaped like its real rows: tile 32, then lines 16 × 40 % and 12 × 60 % (12 × 50 % for the passkeys’ second line). There are 2 passkey rows, 3 session rows and 3 activity rows.
- Below a 480 px column, rows that have actions also get a 36 px pill skeleton on the wrapped action line, so loaded rows don’t grow.
- `aria-busy="true"` goes on each loading group, **not** on `main`. The sr-only `role="status"` "Loading your sign-in settings…" is the first child of `.ra-page`, outside the busy groups.
- Foot buttons are hidden while loading; status slots stay reserved.
- **Load error** (no board): suiss `Alert` (destructive) in place of the group.
  - Title "Couldn’t load your passkeys", description "Check your connection."
  - Action **Try again** (outline `sm`).
  - The same pattern applies to every list.

### 4.4 Confirm it’s you (step-up)

**When it runs:**

- **Required for:** change email, add a passkey, remove a passkey, request a data download, delete account.
- **Trigger:** your last sign-in or step-up was **more than 10 minutes ago** (§9 Q2).
- **Not required for:** names, photo, preferences, rename, signing out (this device, one device or the others) or removing an app.

**Order:** the action’s own confirmation comes first, if it has one (remove passkey, delete account). Then the step-up runs: after a confirm it opens as its own dialog (the §4.0 hand-off), and inside a form dialog it replaces the content. Then the action happens. Cancelling the step-up cancels the action, changes nothing, and returns focus to the trigger.

**Component:** suiss `modal` (Dialog ≥768 px, max 448 wide; floating drawer below), in a `<form>`. The title is the same in every state of screen B (screen A shares it; screen C has its own). The dialog keeps the height of its tallest state at the current width, so the title and buttons never move (§4.0 Dialogs).

**Reserved lines:**

- The description reserves 3 lines below a 400 px dialog width (drawers on phones), 2 lines otherwise.
- The code meta reserves 2 lines, or 3 below a 360 px container.
- Error icons sit inline at the start of a centred, `text-wrap: balance` paragraph, so the icon stays with the first word.

**Screen A: choose.** Shown when the account has a passkey and the browser supports WebAuthn (board `Stepup`). Otherwise it goes straight to screen B.

| Element | Copy |
|---|---|
| Title | Confirm it’s you |
| Description (per action) | **Remove passkey:** To remove the “Pixel 8” passkey, use a passkey or an email code. |
| | **Change email:** To change your email, use a passkey or an email code. |
| | **Add a passkey:** To add a passkey, use one you already have or an email code. |
| | **Download:** To download your data, use a passkey or an email code. |
| | **Delete:** To delete your account, use a passkey or an email code. |
| Buttons (stacked, block, 44) | **Use a passkey** (default), **Email me a code instead** (outline), **Cancel** (plain). Initial focus: the title (`tabindex="-1"`, no ring), whatever opened step-up (a confirm, a row action or a page button) |
| Note (Detail, Slate, `text-wrap: balance`) | After this, we won’t ask again for 10 minutes. |
| Waiting (no board) | The first button shows the spinner and "Waiting for your device…" (`aria-disabled`). Status: "Follow the prompt on your screen to finish." |
| Failed (no board; same layout, with an alert above the buttons) | "That didn’t work. The prompt was canceled or timed out, or this device has no passkey for Richie. Try again, or get a code by email." The first button becomes **Try again** |

**Screen B: email code** (`Stepup-code`). Sending the code is automatic when screen B opens.

| Element | Copy and rule |
|---|---|
| Title | Confirm it’s you |
| Description | To remove the “Pixel 8” passkey, enter the code we sent to **jamie.rivera@northwind.coffee**. (The action part changes per action, as in screen A) |
| Code | One real `<input>` over 8 drawn slots, 4 + 4 with a separator, the identity Verify pattern. `autocomplete="one-time-code"`, `inputmode="text"`, `autocapitalize="characters"`, `autocorrect="off"`, `spellcheck="false"`, `enterkeyhint="go"`. It accepts Crockford Base32: uppercase, no I, L, O or U. Typed I or L becomes 1 and O becomes 0, and hyphens and spaces are ignored. Paste fills all 8. sr label: "Email code, 8 letters and numbers" (identity should adopt the same name). It takes initial focus on screen B, whatever opened step-up. No auto-submit; `Enter` submits the form |
| Meta, line 1 (reserved) | Codes expire after 5 minutes. |
| Meta, line 2 (reserved) | **Send a new code in 1:00** (tabular, counts down from the server’s next-allowed time). At 0:00 it becomes the plain button **Send a new code**. The polite sr line at 0:00 reads "You can send a new code now." |
| Alternatives | One centred line of plain buttons under the meta, wrapping when narrow: **Use a passkey instead** (only when available) and **Can’t get the code?** (always; opens screen C) |
| Footer | **Cancel** (outline), **Confirm** (default); busy "Checking code…" (sr status "Checking your code…") |
| Wrong code (`Stepup-code-error`) | Slots invalid. The error (alert) replaces meta line 1: "That code isn’t right. You have 3 tries left." Then "…You have 2 tries left." and "…You have 1 try left." The input stays focused, with its text selected |
| Locked, 4th wrong try (`Stepup-code-locked`) | **Description** changes to: "To remove the “Pixel 8” passkey, you’ll need a new code. We’ll send it to **jamie.rivera@northwind.coffee**." **Error:** "That was the last try for this code." **Input:** `readonly` and `aria-disabled="true"`, never `disabled`, so focus isn’t dropped. The slots are drawn disabled (Mist fill, `--border` edges). **Focus** moves to the error message: `tabindex="-1"`, rendered without `role="alert"` because focus announces it. As a programmatic focus target it shows **no ring**, so the board draws none. **Meta line 2** is hidden. **Commit button:** becomes **Send a new code in 0:41** (unavailable style, `aria-disabled`, still focusable), then **Send a new code** (default) at 0:00, with the polite line "You can send a new code now." Its `min-width` is set by this label, so Cancel never jumps |
| Expired, after 5 minutes (no board) | Description as in the locked state. Error "This code has expired." Button **Send a new code** (default) |
| New code sent (no board) | The slots clear and the timer restarts at 1:00. Meta line 1 reads "We sent a new code." (polite), and returns to "Codes expire after 5 minutes." after 5 s |
| Send failed (no board) | "We couldn’t send a code. Check your connection." (offline and server variants in §4.0). Button **Try again** |
| Success | The dialog closes, the action runs, and its own feedback appears (for example the "Passkey removed" toast). Focus follows the action’s rule |

**Screen C: can’t get the code** (board `Stepup-cant-get-code`, drawer `Stepup-cant-get-code-mobile`). It opens from **Can’t get the code?** on screen B, inside the same dialog, which keeps its height (§4.0).

| Element | Copy and rule |
|---|---|
| Title | Can’t get the code? (`tabindex="-1"`, initial focus, no ring) |
| List (Body, Ink, `ul.ra-list`) | Look in your spam or junk folder. |
| | Wait a minute, then send a new code. |
| | Use a passkey instead. (**Only when the person has a passkey**; otherwise the list has two items) |
| Passkey button | **Use a passkey instead** (plain, as in the §4.0 button table), shown with the bullet above **only when the person has a passkey** (and the browser supports WebAuthn). It runs the passkey step-up (screen A’s waiting and failed states) |
| Paragraph | Can’t get email at **jamie.rivera@northwind.coffee** any more? Contact Richie support. We’ll check it’s you and help you change your email. (The address follows the long-email rule) |
| Footer (form dialog, right-aligned) | **Back** (outline), **Contact support** (default). Contact support opens the support contact form, with the topic "I can’t get my email code" and the signed-in account attached, in a new tab. Its accessible name: "Contact support, opens in a new tab" |
| Back | Returns to screen B. The code, the tries and the 1:00 countdown are unchanged, and focus returns to **Can’t get the code?** |
| Policy | Support-assisted recovery (§9 Q7): support verifies identity, changes the email, notifies both addresses, and marks the account *recently recovered* |

**Mobile (`Stepup-mobile`):**

- A floating drawer, 8 px from the edges, radius 28, with the swipe handle.
- The title and description are centred (suiss drawer header). The code row fits from 320 px (slot width ≈ 27 px, glyph 20 px).
- Buttons are stacked in block style, with the commit button on top.

### 4.5 Preferences (`/account/preferences`, board `Preferences`)

- `h1`: **Preferences**
- Lede: **These follow you to every device you sign in on.**
- Every control **saves when changed**. There is no Save button.
- Each section has its own status slot (reserved by width, §4.0). It confirms with "Saved." and clears after 4 s.
- Loading copy is in §4.0.

**Section: Appearance and language** (`h2`)

| Row title | Description | Control |
|---|---|---|
| Appearance | System follows your device’s setting. | Segmented `sm`: a **radio group** styled as the suiss segment. Markup: `<fieldset data-slot="toggle-group" data-spacing="0">`, with a sr-only `<legend>Appearance</legend>` (the row title’s text) and real radios **System**, **Light** and **Dark**, each inside its `label`. One option is always selected; arrow keys move and select. Selected: System. It applies at once |
| Language | | Picker (suiss `picker`: custom select on desktop, native select on mobile), `sm`. Value "English (United States)". Options, each with its `lang`: English (United States) `en-US`, English (United Kingdom) `en-GB`, Deutsch `de`, Español `es`, Français `fr`, Português (Brasil) `pt-BR`, ქართული `ka`. The trigger’s value text carries the selected option’s `lang` (`<span id="language-value" lang="en-US">`). **Changing it** reloads the interface in place: the same page, scrolled so the "Appearance and language" section is at the top, with focus on the Language trigger. The section’s status then reads "Saved. Richie is now in {language}." in the new language, for example "Saved. Richie is now in English (United Kingdom)." A one-time flag set before the reload carries the message across it |

**Section: Date and time** (`h2`)

| Row title | Description | Control |
|---|---|---|
| Set time zone automatically | Uses your device’s time zone. | Switch, on |
| Time zone | | When automatic: trailing Slate value "Pacific Time (Los Angeles)", not a control. When the switch is off: a picker with the same value |
| Time format | | Segmented radio group, the same `fieldset` + sr `legend` ("Time format") pattern: **1:30 PM** and **13:30**. Each radio’s accessible name starts with its visible label and adds a sr suffix: "1:30 PM, 12-hour" and "13:30, 24-hour". Selected: 1:30 PM |

**Section: Emails from Richie** (`h2`; a plain `section aria-labelledby` like every other section: no `fieldset`)

| Row title | Description | Control |
|---|---|---|
| Account and security | Email codes and security alerts. These can’t be turned off. | Trailing Slate text "Always on" (not a disabled switch) |
| Product news | New features and improvements, about once a month. | Switch, off |
| Getting started tips | A few short emails in your first month. | Switch, on |
| Feedback requests | Occasional invitations to tell us what you think. | Switch, off |

- Foot: the status slot first, then the note (Slate): **Every email from us also has an unsubscribe link.** The note goes inside `.ra-section-foot`.

**Errors:**

- **Save failed:** the control reverts. An alert appears in the section’s status slot, using the §4.0 patterns: "We couldn’t save that change. Check your connection."
- **Offline (`Preferences-offline`):**
  - A suiss `Alert` (default variant, `WifiSlash` icon) sits between the lede and the first section. Title "You’re offline", description "Changes won’t save until you’re back online."
  - In the board, "Product news" has been flipped back to off. Its section’s alert reads "We couldn’t save that change because you’re offline."
  - Controls stay operable.

### 4.6 Connected apps (`/account/connected-apps`, board `Connected`)

- `h1`: **Connected apps**
- Lede: **Apps you’ve let use your Richie account. You can remove access at any time.**
- The page has no `h2`: the group follows the header directly, labelled with `aria-label="Connected apps"`.

| Media | Title | Description (2 lines) | Action |
|---|---|---|---|
| The app’s logo, or tile "T" | Tallybird | Can see your name, email address and photo. / Connected August 14. Last used yesterday. | **Remove access** (outline `sm`, sr "Remove access for Tallybird") |
| The app’s logo, or tile "S" | Slotwise | Can see your name and email address, and stay signed in when you’re not using it. / Connected September 2. Last used October 21. | same |

- **App identity**: when the OAuth client has a registered logo (Duende client `LogoUri`), show it as the 32 px media, radius 8, `alt=""` (the name follows). Otherwise use the letter tile. Boards draw letter tiles (invented apps).
- **Remove access** (`confirm`, board `Connected-remove`):

| Element | Copy |
|---|---|
| Title | Remove access for Tallybird? |
| Description | Tallybird won’t be able to use your Richie account anymore, and you’ll be signed out of Tallybird wherever you used Richie to sign in. To use it again, connect it from Tallybird. |
| Actions | **Cancel**, **Remove access** (destructive); busy "Removing…" |
| Then | The row is removed. Toast "Access removed" with the description "Tallybird can’t use your Richie account anymore." No step-up. **Focus:** the page has no `h2`, so it goes to the `h1`, or to the empty-state title (`tabindex="-1"`) when the last app was removed. A failure shows inside the confirm (§4.0) |

- **Empty (`Connected-empty`):** suiss `Empty`, filled (Mist), with the `PlugsConnected` icon tile.
  - Title "No connected apps".
  - Description "When you sign in to another app with your Richie account, it shows up here."
  - No button.
  - The board also shows the toast left by removing the last app: "Access removed / Slotwise can’t use your Richie account anymore."

### 4.7 Data and privacy (`/account/data`, board `Data`)

- `h1`: **Data and privacy**
- Lede: **Get a copy of your data, or delete your account. Read how we handle your data in our Privacy Policy.** "Privacy Policy" is an underlined link.

**Section: Download your data** (`h2`, `id="download"`)

- Description: **Your profile, passkeys, devices, sign-in history, preferences and connected apps, as JSON files in a .zip. It contains account data only.**

| State | Title | Description | Action |
|---|---|---|---|
| None (base) | Your Richie data | We’ll email you when it’s ready, usually within an hour. | **Request download** (outline `sm`). Step-up first. Status "Download requested." |
| Preparing (no board) | Preparing your download | Requested today at 10:42 AM. We’ll email you when it’s ready, usually within an hour. | Badge "Preparing" (`secondary`) |
| Ready (`Data-ready`) | Your download is ready | 2.4 MB. Available until November 11. | **Download** (default `sm`, `DownloadSimple`; sr "Download your data, 2.4 MB zip file") |
| Expired (no board) | Your last download expired | It expired on November 1. Request a new one whenever you need it. | **Request download** |
| Limit (no board) | Your download is ready (or Preparing) | You can request a new download once a day. The next one is available tomorrow at 10:42 AM. | |

**Section: Delete your account** (`h2`, `id="delete"`)

- Description: **We sign you out everywhere and delete your account 30 days later. Until then, you can sign in to keep it.**
- **Delete account** (destructive, default size, start-aligned) follows the description. **There is no status slot:** the button only opens the dialog, and failures show inside the dialog (§4.0). Deletion is never blocked, because nothing outside the account is checked. The full lifecycle is in §4.8.

**Delete dialog** (suiss `Dialog` in a `<form>`, 448 wide, the `dialog-destructive` pattern; board `Data-delete-confirm` over `Data`; drawer on mobile, board `Data-delete-confirm-mobile`):

| Element | Copy |
|---|---|
| Title | Delete your account? |
| Description (lede) | We’ll sign you out on every device and delete your account on December 5. |
| List (Body, Ink, `ul`) | Until December 5, you can sign in to keep your account. |
| | After that, you can’t sign in, use Richie with this account or get it back. |
| | Until January 4, 2027, you can still ask for a copy of your data. We’ll email you how. |
| | Connected apps lose access right away. |
| Field | Label **Type “delete my account” to confirm**. `autocomplete="off"`, `autocapitalize="none"`, `spellcheck="false"`. **Matching** trims spaces at both ends, collapses inner runs of spaces and ignores case. The phrase is **localized** with the interface language: the label shows the phrase in that language, and only that phrase matches (for example German "mein konto löschen"). **Initial focus**, and on open the field is scrolled into view (`block: "nearest"`), so it’s visible at short heights and at 200 % zoom. The board shows it filled and focused |
| Error on submit (field invalid, focus to the field, no `role`) | Type “delete my account” exactly as shown. |
| Footer | Form-dialog footer, right-aligned: **Cancel** (outline), **Delete account** (destructive). The button stays enabled and validates on press: it is never dimmed (§2.3). This departs from the suiss example’s disabled-until-match |
| Then | Step-up (§4.4, description "To delete your account, …") **inside the same dialog**, which keeps its height (§4.0). Then "Deleting…" (busy), then the signed-out page `Data-deleted` (§4.8). A failure shows inside the dialog |

### 4.8 Account deletion: lifecycle and signed-out pages

**Lifecycle** (dates use the sample "today" of Thursday, November 5, 2026):

| When | What happens | What the person sees |
|---|---|---|
| **Day 0** (November 5) | After the typed confirmation and step-up: every session and refresh token is revoked (signed out everywhere), and every connected app’s grant is revoked. The account is marked *scheduled for deletion* | `Data-deleted` page; the "will be deleted" email |
| **Days 0–30** | The account still exists. A successful sign-in (email code or passkey) lands on the deletion-pending page instead of Richie. Signing up with the same email says the address already has an account | `Deletion-pending`; the reminder email on day 23 (November 28) |
| **Keep my account** (any time in days 0–30) | The account is restored as it was, except connected apps, which must be connected again. Passkeys, profile and preferences are kept | Richie app root; the "you kept" email |
| **Day 30** (December 5) | The account is deleted: no sign-in, no restore. Passkeys and sessions are gone. From now on the email can create a new account. A frozen copy of the account data is kept only for export | The "has been deleted" email, with **Download your data** |
| **Days 30–60** | A signed-out export flow, verified by an email code each time (see "Export links" below). It starts from the day-30 email link (`Export-request`), from `/export` without a link (`Export-start`), or from identity sign-in (`Signin-deleted`). Then `Export-request-code`, then `Export-requested` | Export pages; `Signin-deleted`; the "ready to download" email |
| **Day 60** (January 4, 2027) | Everything is purged, including any export | The "data has been deleted" email; every link shows the expired page |

**Export links**:

| Link | Lifetime | Rule |
|---|---|---|
| The day-30 email link (`/export?token=…`) | Valid and **reusable until day 60** | Every use needs a fresh email code (§4.4 rules), so the token alone never downloads anything |
| The download link (the "ready" email) | 7 days, never past day 60 | A new request replaces it: the old link then shows the download-expired page |
| Any link after day 60 | n/a | The window-over page |

- **"Request a new download" always goes to `/export`**: with the token when there is one, so the address is prefilled (`Export-request`), and without it otherwise (`Export-start`). It never goes to an expired page.

**Solo page frame.** This is the identity layout: a centred column, max 460, top-anchored.

- It is used for `Data-deleted`, `Deletion-pending`, the export pages, the expired pages and `Signin-deleted`.
- **The shiba appears only on these identity-style solo pages**, as a **1× brand mark** (31 × 39, at every width) 16 px above the `h1`. It is not a hero: the page is about a deletion, so the cheerful mascot stays small. It uses `image-rendering: pixelated` and `alt=""`.
- Sizes depend on width only. The title is 30 / 36. Footer links: Help, Privacy, Terms (separate links, no dots).

**`Data-deleted`** (`/account/deleted`; signed out, shown once right after the request):

| Element | Copy |
|---|---|
| Title | Your account will be deleted on December 5 |
| Body (Body, Slate) | We’ve signed you out on every device. If you change your mind, sign in before then to keep your account. |
| Detail (Detail, Slate) | We sent the details to jamie.rivera@northwind.coffee. |
| Link (underlined) | Sign in to keep your account (identity sign-in, `#signin` on boards) |

**`Deletion-pending`** (identity, after a successful sign-in in days 0–30):

| Element | Copy |
|---|---|
| Title | Your account is scheduled for deletion |
| Body | We’ll delete it on December 5. Keep your account to go on using Richie. You’ll need to connect your apps again. |
| Buttons | **Keep my account** (default; busy "Keeping your account…"), **Sign out** (outline). Initial focus: the title (`tabindex="-1"`) |
| Keep | Restores the account. The person continues to the sign-in’s **return URL**, a deep link or an OAuth authorize request from another app, falling back to the Richie app root. The "you kept" email is sent. Failure: the §4.0 patterns with the phrase "keep your account", under the buttons |
| Sign out | Back to identity sign-in. The deletion stays scheduled |
| Sign-up with the same email (identity sign-up error, days 0–30) | That email already has a Richie account. Sign in to keep it. (with the link "Sign in") |

**`Export-start`** (`/export` without a link, signed out; mobile `Export-start-mobile`):

| Element | Copy |
|---|---|
| Title | Download your data |
| Body | Deleted your Richie account? For 30 days after we delete it, you can still ask for a copy of your data. |
| Field | Label **Email address**. `type="email"`, `autocomplete="email"`, `inputmode="email"`, `autocapitalize="none"`, `spellcheck="false"`, placeholder "name@example.com". Prefilled when arriving from `Signin-deleted`. Initial focus: the `h1`, then the field |
| Button | **Email me a code** (default, `lg`, `type="submit"`); busy "Sending code…" |
| Note (Detail, Slate) | We’ll send an 8-character code to this address to check it’s you. |
| Errors (field error, focus to the field, no `role`) | Empty: "Enter your email address." Not an address: "Enter a full email address, like name@example.com." **No deleted account with data:** "There’s no deleted Richie account with data to download for this address. Check the address, or sign in if you still use Richie." Network and offline: the §4.0 patterns with "send a code" |
| Existence and limits | **Accepted trade-off:** as on `Signin-deleted`, this page reveals whether an address had an account deleted in the last 30 days. **Rate limit:** 5 code requests per address per hour and 20 per IP address per hour (§9 Q8). It is shown with the existing countdown pattern: the button becomes unavailable as **Email me a code in 41:12** (`aria-disabled`, m:ss) and returns at 0:00 with the polite line "You can ask for a new code now." |

**`Export-request`** (`/export?token=…`, signed out, days 30–60, from the day-30 email; mobile `Export-request-mobile`):

| Element | Copy |
|---|---|
| Title | Download your data |
| Body | Your Richie account was deleted on December 5. Until January 4, 2027, you can ask for a copy of your data. |
| Chip | The identity account chip: initials avatar "JR" and jamie.rivera@northwind.coffee (long-email rule). No "Not you?" link: the address comes from the email link. A visitor who wants another address uses `/export` without the token |
| Button | **Email me a code** (default, `lg`); busy "Sending code…" |
| Note (Detail, Slate) | We’ll send an 8-character code to this address to check it’s you. |
| Errors | The §4.0 patterns with the phrase "send a code". The rate limit is the 1:00 countdown on the next page |

**`Export-request-code`** (`/export/code`):

| Element | Copy |
|---|---|
| Title | Check your email |
| Body | Enter the code we sent to jamie.rivera@northwind.coffee. |
| Code | The §4.4 code rules: 8 slots (4 + 4), "Codes expire after 5 minutes.", **Send a new code in 1:00**, 4 tries, the locked and expired states, and the polite sr status element |
| Button | **Request download** (default, `lg`, `type="submit"`); busy "Checking code…" |

**`Export-requested`** (`/export/requested`):

| Element | Copy |
|---|---|
| Title | We’re preparing your download |
| Body | We’ll email a link to jamie.rivera@northwind.coffee when it’s ready. The link works for 7 days. |
| Body, fewer than 7 days before day 60 | We’ll email a link to jamie.rivera@northwind.coffee when it’s ready. The link works until January 4, 2027. |

**Expired links** (`/export/expired`; no board). The page title and the document title match:

| Case | `h1` and document title | Body | Action |
|---|---|---|---|
| Window over: any link after day 60 | This link has expired / "This link has expired – Richie" | You could download your data until January 4, 2027. After that, we deleted everything. | Link "Go to the Richie website" |
| Download link: older than 7 days, or replaced by a newer request, before day 60 | This download link has expired / "This download link has expired – Richie" | Until January 4, 2027, you can ask for a new one. | **Request a new download** (default), to `/export` with the token when the link carries one (`Export-request`), else to `Export-start`. Never to an expired page |

**`Signin-deleted`** (identity sign-in, email step, for an address whose account was deleted in the last 30 days). **This is a handoff item for the identity canvas’s sign-in flow**; this canvas draws it for reference.

| Element | Copy |
|---|---|
| Title | This account was deleted |
| Body | We deleted the Richie account for **jamie.rivera@northwind.coffee** on December 5. Until January 4, 2027, you can still ask for a copy of your data. |
| Buttons (stacked, block, `lg`) | **Download your data** (default; to `Export-start` with the address prefilled), **Create a new account** (outline; to identity sign-up with the address prefilled), **Use a different email** (plain; back to the sign-in email step) |
| Notes | It is shown after the email step and before any code is sent. **A live account wins:** if someone has already created a new account with this address, sign-in goes to that account and `Signin-deleted` is not shown. The deleted account’s download then stays reachable through its day-30 email link and `Export-start`. Like the identity sign-in itself, the page says whether an address has (had) an account: the trade-off is accepted for the 30-day window, and data still needs an email code. After day 60, sign-in treats the address like any other new address |

**Emails** (security emails, always sent; sender "Richie"; each has one button):

| When | Subject | Body | Button |
|---|---|---|---|
| Day 0 | Your Richie account will be deleted on December 5 | Hi Jamie, you asked us to delete your Richie account, so we’ve signed you out on every device and removed access for your connected apps. We’ll delete your account on December 5. Until then, you can sign in to keep it. If you didn’t ask for this, sign in now and check Sign-in and security. | Sign in to keep your account |
| Day 23 | Your Richie account will be deleted in 7 days | Hi Jamie, we’ll delete your Richie account on December 5. To keep it, sign in before then. After that, you can’t sign in, use Richie with this account or get it back. | Sign in to keep your account |
| Keep my account | You kept your Richie account | Hi Jamie, you signed in and kept your Richie account, so we won’t delete it. Your connected apps lost access when you asked us to delete it; connect them again from each app if you still use them. If this wasn’t you, sign in now and check Sign-in and security. | Go to Richie |
| Day 30 | Your Richie account has been deleted | Hi Jamie, we deleted your Richie account today. You can’t sign in to it anymore, but you can use jamie.rivera@northwind.coffee to create a new account. Until January 4, 2027, you can still download a copy of your data. | Download your data |
| Export ready (days 30–60) | Your Richie data is ready to download | Hi Jamie, here’s the copy of your data you asked for. The link works until December 16, or until you ask for a new copy. After that, you can ask for a new one until January 4, 2027. | Download your data |
| Day 60 | Your Richie data has been deleted | Hi Jamie, we’ve now deleted all the data from your Richie account, as we said we would. There’s nothing left to download. You’re welcome back any time. | Go to Richie |

---

## 5. State matrix

Each cell names the board that draws the state, or says why none is needed.

- **"copy"**: the state is fully specified in §4 and is a text or label swap on a drawn layout.
- **"harness"**: it is produced from existing boards with the harness: widths, `BOARD_THEME=dark`, `forced-colors` and `prefers-reduced-motion` emulation, and 200 % zoom.
- **"n/a"**: the state can’t occur on that page.

| how-we-design §6 item | Profile | Sign-in and security | Step-up | Preferences | Connected apps | Data and privacy | Deletion and export (§4.8) | Shell / index |
|---|---|---|---|---|---|---|---|---|
| Default, realistic data | `Main` | `Security` | `Stepup` (choose) | `Preferences` | `Connected` | `Data` (no download yet, delete available) | `Deletion-pending`, `Export-request` | `Account-index`, `Account-sidebar` |
| Empty | No photo: `Profile-extreme`, `Profile-photo-removed` | `Security-no-passkeys` (keeps one activity row) | n/a | n/a (every row always has a value) | `Connected-empty` | "No download yet" is the base, `Data` | n/a | n/a |
| Long or extreme data | `Profile-extreme` (two-line sidebar email) | Long email: copy (row wrap rule) | Long email in the description: copy (wraps after the @) | Long language names wrap: copy | Long scope lists wrap: copy | n/a | Long email in the chip: copy (long-email rule) | Sidebar in `Profile-extreme` |
| Initial loading | §4.0 loading table | `Security-loading` | n/a | §4.0 loading table | §4.0 loading table | §4.0 loading table | n/a (static pages) | n/a (the nav is static) |
| Saving (busy) | "Saving…", "Uploading…": copy | `Security-passkey-waiting` (busy button, drawn once) | "Checking code…": copy | n/a (instant) | "Removing…": copy | "Deleting…": copy | "Keeping your account…", "Sending code…": copy | n/a |
| Saved (inline + polite) | `Profile-name-saved` ("Jay"); `Profile-photo-removed` ("Photo removed." with inline Undo); "Photo saved.": copy | "Passkey renamed.": copy | n/a | "Saved.": copy | Toast in `Connected-empty` | "Download requested.": copy | `Export-requested` | n/a |
| Field validation | `Profile-name-error` | Rename: copy | `Stepup-code-error` | n/a | n/a | Typed phrase: copy | Code errors: copy (§4.4) | n/a |
| Server error | `Profile-save-failed` ("Jay"); other actions: §4.0 failure table | §4.0 failure table | §4.0 failure table | §4.0 failure table | §4.0 failure table | §4.0 failure table | §4.0 failure table | n/a |
| Offline | Page alert as in `Preferences-offline`; §4.0 offline pattern | same | same | `Preferences-offline` | same | same | same | n/a |
| Rate limit or cooldown | 1:00 resend timer in `Profile-email-code` | Passkey limit: copy | `Stepup-code-locked` (countdown in the button) | n/a | n/a | Once-a-day download: copy | 1:00 resend timer on `Export-request-code` | n/a |
| Destructive confirmation | Discard changes: copy | `Security-remove-passkey`, `…-mobile` (sheet) | n/a | n/a | `Connected-remove` | `Data-delete-confirm`, `…-mobile` (drawer) | n/a | n/a |
| Step-up | `Profile-email-change` and `Profile-email-code` (step-up before them, as in `Stepup`) | `Stepup` (over Security) | `Stepup`, `Stepup-code`, `Stepup-code-error`, `Stepup-code-locked`, `Stepup-mobile` | n/a | n/a (not gated) | Delete or download: copy (same dialog) | Email-code check on `Export-request-code` | n/a |
| Device waiting, failed, canceled | n/a | `Security-passkey-waiting`, `Security-passkey-failed` | Passkey waiting and failed: copy | n/a | n/a | n/a | n/a | n/a |
| Not allowed | n/a | n/a | n/a | The "Always on" security-email row in `Preferences` | n/a | n/a (deletion is never blocked) | Expired links: copy (`/export/expired`); no deleted account: field error on `Export-start` | n/a |
| Can’t receive the code, or lost the link (recovery) | n/a | n/a | `Stepup-cant-get-code`, `-mobile` (screen C) | n/a | n/a | n/a | `Export-start`, `-mobile` (no link); `Signin-deleted` (via sign-in) | n/a |
| Focus after the action | Remove photo → Upload photo (`Profile-photo-removed`); Undo → Change photo | Removed row → section `h2` | Locked → the error message (`Stepup-code-locked`) | n/a | Removed app → `h1`, or the empty title | Delete → `Data-deleted` title | Solo page titles (`tabindex="-1"`) | n/a |
| Widths 320, 375/390, 768, 1280, 1440 | harness: all boards at 320×640, 375×812, 390×844, 768×1024, 1280×800, 1440×900 | | | | | | | |
| Short height (≤540) | harness at 844×390 and 1280×540 on `Main`, `Profile-photo-crop`, `Profile-email-change`, `Stepup-code`, `Data-delete-confirm`, `Export-request-code` (dialogs scroll inside with the edge cue) | | | | | | | |
| Dark for every state | Drawn: `Profile-dark`, `Profile-name-error-dark` | `Security-dark` | `Stepup-code-error-dark` | `Preferences-dark` | `Connected-dark` | `Data-dark` | harness | `Account-index-dark`. **Every other state:** harness with `BOARD_THEME=dark` |
| Forced colours | harness (`forced-colors: active` emulation) on `Main`, `Security`, `Stepup-code-locked`, `Preferences` (switches, segmented), `Connected-remove`, `Security-remove-passkey-mobile` | | | | | | | |
| Reduced motion | harness (`prefers-reduced-motion: reduce`); no board can show motion. Rules in §7.6 | | | | | | | |
| Visible focus | `Profile-name-error` (first-name field), `Profile-email-change` (new-email field), `Profile-photo-crop` (crop area) | | `Stepup-code` (active slot). Programmatic targets (screen A’s title, the locked error message, screen C’s title) take focus with no ring | | `Connected-remove` (Cancel) | `Data-delete-confirm` (typed field) | `Export-request-code` (active slot) | Harness: tab through `Account-index` and `Security` |
| 200 % zoom | harness: 1440 at 200 %, equal to 720 px (the mobile layout) | | | | | | | |

---

## 6. Board list and canvas layout

### 6.1 Architecture rules

- **Static markup only.** No `{{holes}}`, `<sc-if>`, `<sc-for>`, Tweaks or state. All copy is literal.
- **Imports:** `<dc-import name="X" hint-size="Wpx,Hpx"></dc-import>`, always with an explicit closing tag (the harness regex needs it). An import inlines X’s `<x-dc>` body, **not X’s helmet**, so every style must live in `account.css`.
- **Shared chrome:** `Account-sidebar` is imported inside each page’s shell. The shell carries `data-page="profile|security|preferences|apps|data"`, and CSS highlights the matching `[data-nav]`. **Exceptions:** three boards carry their own sidebar copy instead of the import, because the user data in the sidebar differs:
  - `Profile-extreme`: another user;
  - `Profile-name-saved`: "Jay Rivera";
  - `Profile-photo-removed`: the initials avatar.

  Re-copy these three whenever `Account-sidebar` changes.
- **Board kinds:**
  - **Wrappers** (mobile and dark): a single `div` around one import.
    - Mobile: `<div style="width: 390px; min-height: 844px"><dc-import name="Security" hint-size="390px,844px"></dc-import></div>`.
    - Dark desktop: `<div class="dark" style="min-height: 1500px; background: #000000"><dc-import name="Security" hint-size="1440px,1500px"></dc-import></div>`, with `body{margin:0;background:#000}` in its helmet. There is no `width` on the desktop wrapper, so `fill` works.
  - **Overlay states:** `<div inert><dc-import name="Security" …></dc-import></div>`, then, as its **next sibling**, `<div class="ra ra--part ra-layer">` containing the overlay and the dialog.
    - `inert` makes the page behind a modal background and keeps Tab inside the dialog. The `inert` box is pinned to the viewport, as a modal scroll lock does, so the scrim covers the whole frame at any size.
    - They are never page copies. The same overlay markup renders as a dialog at 1440 and as a sheet or drawer at 390.
    - Mobile overlay boards are wrappers that import the desktop overlay board.
  - **Toasts and layers sit outside `.ra-page`**, because a size container traps `position: fixed`. Toast viewports are the last child of `<main>`; layers are siblings of the import.
  - **Board `<title>`** is always the board title from §6.2 (for example "Profile, first name missing"), never the app’s document title.
  - **Full copies:** in-page states that change content. There are 12:
    - `Profile-name-saved`, `Profile-name-error`, `Profile-save-failed`, `Profile-extreme`, `Profile-photo-removed`;
    - `Security-loading`, `Security-no-passkeys`, `Security-passkey-waiting`, `Security-passkey-failed`;
    - `Preferences-offline`;
    - `Connected-empty`;
    - `Data-ready`.
    - Each is a copy of its base `<main>` with local edits, and still imports `Account-sidebar`. **Builders: finish the bases before copying, and re-copy rather than hand-patch if a base changes.**
- **Board file template.** Identity’s template, with `account.css` instead of `richie.css`:
  ```html
  <!doctype html><html lang="en"><head><meta charset="utf-8">
  <title>Sign-in and security, desktop</title>
  <script src="./support.js"></script>
  <link rel="stylesheet" href="./account.css">
  </head><body><x-dc>
  <helmet>
  <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Inter:wght@400;600;700&amp;display=swap">
  <style>body{margin:0}</style>
  </helmet>
  <div class="ra"> … </div>
  </x-dc>
  <script type="text/x-dc" data-dc-script data-props='{"$preview":{"width":1440,"height":1500}}'>
  class Component extends DCLogic { renderVals() { return {}; } }
  </script></body></html>
  ```
  `$preview` equals the board size.
- **Links between boards:** `href="Security.dc.html"`. File names match `[\w-]+` (the harness rewrites those). Out-of-area links use `#…` fragments (`#richie` for the app root, `#help`, `#privacy`).
- **Assets:** the photo stand-in is inline SVG (§10.2). The shiba, used only on the solo pages (`Data-deleted`, Row 8), is already copied into this canvas as `/_blob/70084467a4a633969708f2f323162f2d` (from identity asset `6af6329655ac2e226c3e26d79e124698`). Use: `<img class="ra-mark" src="/_blob/70084467a4a633969708f2f323162f2d" alt="" width="31" height="39">` (1×).
- **Icons:** Phosphor regular, inlined from `https://unpkg.com/@phosphor-icons/core@2.1.1/assets/regular/<name>.svg` (checked: every name in §10.3 returns 200). Use `viewBox="0 0 256 256"`, `fill="currentColor"`, `width`/`height` 16 (20 in alerts and empty states, 24 in toasts) and `aria-hidden="true"`.
- **Canvas-only class:** `is-focused` draws a focus ring where the page would have focus on arrival. It never appears on a base board.

### 6.2 Boards (56)

Every value below is generated from `canvas.json` (canvas version `1793860937-9254`) and the boards’ own `<dc-import>` tags:

- file, title, size, x and y come from `canvas.json`;
- kind and imports are read from each `.dc.html`;
- the row title notes and their `maxW` come from `canvas.json`.

**Board titles:** in `canvas.json`, in each board’s `<title>` and in this table, they are identical for all 56 boards.

Abbreviations:

- **W** = wrapper; **O** = overlay; **F** = full copy; **B** = base; **P** = part.
- **fill** = `expand: "fill"`; **int** = `is_interactive: true`.
- **x, y** are the canvas position.

Builders round a board’s measured height up to the next 100 (minimum 900).

**Row 1: Profile** (y = 0)

- Title note at (0, −260): **"Profile: photo, name and email, with the live customer preview (entry board: Main)"**, `maxW` 8000 (the row spans 17110).

| # | File | Board title | Size | Kind | fill / int | Imports | x |
|---|---|---|---|---|---|---|---|
| 1 | `Main` | Profile, desktop | 1440×1000 | B | fill, int | Account-sidebar | 0 |
| 2 | `Profile-mobile` | Profile, mobile | 390×844 | W | int | Main | 1520 |
| 3 | `Profile-dark` | Profile, dark | 1440×1000 | W | fill, int | Main | 1990 |
| 4 | `Profile-name-saved` | Profile, name saved | 1440×1000 | F (own sidebar) | fill, int | none | 3510 |
| 5 | `Profile-name-error` | Profile, first name missing | 1440×1000 | F | fill, int | Account-sidebar | 5030 |
| 6 | `Profile-name-error-dark` | Profile, first name missing, dark | 1440×1000 | W | fill, int | Profile-name-error | 6550 |
| 7 | `Profile-save-failed` | Profile, name not saved | 1440×1000 | F | fill, int | Account-sidebar | 8070 |
| 8 | `Profile-photo-crop` | Profile, adjusting a new photo | 1440×900 | O | fill | Main | 9590 |
| 9 | `Profile-email-change` | Profile, changing your email | 1440×900 | O | fill | Main | 11110 |
| 10 | `Profile-email-code` | Profile, confirming a new email | 1440×900 | O | fill | Main | 12630 |
| 11 | `Profile-extreme` | Profile, long name and email, no photo | 1440×1000 | F (own sidebar) | fill, int | none | 14150 |
| 12 | `Profile-photo-removed` | Profile, photo removed | 1440×1000 | F (own sidebar) | fill, int | none | 15670 |

**Row 2: Account shell** (y = 1400)

- Title note at (0, 1140): **"Account shell: shared sidebar and the mobile index"**, `maxW` 2380.

| # | File | Board title | Size | Kind | fill / int | Imports | x |
|---|---|---|---|---|---|---|---|
| 13 | `Account-sidebar` | Account sidebar, shared part | 256×900 | P | none | none | 0 |
| 14 | `Account-index` | Account, mobile index | 390×844 | B | int | none | 1520 |
| 15 | `Account-index-dark` | Account, mobile index, dark | 390×844 | W | int | Account-index | 1990 |

**Row 3: Sign-in and security** (y = 2700)

- Title note at (0, 2440): **"Sign-in and security: email codes, passkeys, devices, activity"**, `maxW` 8000 (the row spans 11500).

| # | File | Board title | Size | Kind | fill / int | Imports | x |
|---|---|---|---|---|---|---|---|
| 16 | `Security` | Sign-in and security, desktop | 1440×1500 | B | fill, int | Account-sidebar | 0 |
| 17 | `Security-mobile` | Sign-in and security, mobile | 390×844 | W | int | Security | 1520 |
| 18 | `Security-dark` | Sign-in and security, dark | 1440×1500 | W | fill, int | Security | 1990 |
| 19 | `Security-loading` | Sign-in and security, loading | 1440×1500 | F | fill, int | Account-sidebar | 3510 |
| 20 | `Security-no-passkeys` | Sign-in and security, no passkeys, one device | 1440×1100 | F | fill, int | Account-sidebar | 5030 |
| 21 | `Security-passkey-waiting` | Sign-in and security, adding a passkey | 1440×1500 | F | fill, int | Account-sidebar | 6550 |
| 22 | `Security-passkey-failed` | Sign-in and security, passkey not added | 1440×1500 | F | fill, int | Account-sidebar | 8070 |
| 23 | `Security-remove-passkey` | Sign-in and security, removing a passkey | 1440×900 | O | fill | Security | 9590 |
| 24 | `Security-remove-passkey-mobile` | Sign-in and security, removing a passkey, mobile | 390×844 | W | none | Security-remove-passkey | 11110 |

**Row 4: Confirm it’s you** (y = 4600)

- Title note at (0, 4340): **"Confirm it’s you: step-up before sensitive changes (shown for removing a passkey)"**, `maxW` 8000 (the row spans 9980).

| # | File | Board title | Size | Kind | fill / int | Imports | x |
|---|---|---|---|---|---|---|---|
| 25 | `Stepup` | Confirm it’s you, choose a passkey or code | 1440×900 | O | fill | Security | 0 |
| 26 | `Stepup-mobile` | Confirm it’s you, code, mobile | 390×844 | W | none | Stepup-code | 1520 |
| 27 | `Stepup-code-error-dark` | Confirm it’s you, wrong code, dark | 1440×900 | W | fill | Stepup-code-error | 1990 |
| 28 | `Stepup-code` | Confirm it’s you, enter the code | 1440×900 | O | fill | Security | 3510 |
| 29 | `Stepup-code-error` | Confirm it’s you, wrong code | 1440×900 | O | fill | Security | 5030 |
| 30 | `Stepup-code-locked` | Confirm it’s you, no tries left | 1440×900 | O | fill | Security | 6550 |
| 31 | `Stepup-cant-get-code` | Confirm it’s you, can’t get the code | 1440×900 | O | fill | Security | 8070 |
| 32 | `Stepup-cant-get-code-mobile` | Confirm it’s you, can’t get the code, mobile | 390×844 | W | none | Stepup-cant-get-code | 9590 |

**Row 5: Preferences** (y = 5900)

- Title note at (0, 5640): **"Preferences: saved as you change them"**, `maxW` 4950.

| # | File | Board title | Size | Kind | fill / int | Imports | x |
|---|---|---|---|---|---|---|---|
| 33 | `Preferences` | Preferences, desktop | 1440×1200 | B | fill, int | Account-sidebar | 0 |
| 34 | `Preferences-mobile` | Preferences, mobile | 390×844 | W | int | Preferences | 1520 |
| 35 | `Preferences-dark` | Preferences, dark | 1440×1200 | W | fill, int | Preferences | 1990 |
| 36 | `Preferences-offline` | Preferences, offline | 1440×1300 | F | fill, int | Account-sidebar | 3510 |

**Row 6: Connected apps** (y = 7600)

- Title note at (0, 7340): **"Connected apps: apps that use your Richie account"**, `maxW` 6470.

| # | File | Board title | Size | Kind | fill / int | Imports | x |
|---|---|---|---|---|---|---|---|
| 37 | `Connected` | Connected apps, desktop | 1440×900 | B | fill, int | Account-sidebar | 0 |
| 38 | `Connected-mobile` | Connected apps, mobile | 390×844 | W | int | Connected | 1520 |
| 39 | `Connected-dark` | Connected apps, dark | 1440×900 | W | fill, int | Connected | 1990 |
| 40 | `Connected-remove` | Connected apps, removing access | 1440×900 | O | fill | Connected | 3510 |
| 41 | `Connected-empty` | Connected apps, none, after removing the last one | 1440×900 | F | fill, int | Account-sidebar | 5030 |

**Row 7: Data and privacy** (y = 8900)

- Title note at (0, 8640): **"Data and privacy: download your data, delete your account"**, `maxW` 8000 (the row spans 8460).

| # | File | Board title | Size | Kind | fill / int | Imports | x |
|---|---|---|---|---|---|---|---|
| 42 | `Data` | Data and privacy, desktop | 1440×900 | B | fill, int | Account-sidebar | 0 |
| 43 | `Data-mobile` | Data and privacy, mobile | 390×844 | W | int | Data | 1520 |
| 44 | `Data-dark` | Data and privacy, dark | 1440×900 | W | fill, int | Data | 1990 |
| 45 | `Data-ready` | Data and privacy, download ready | 1440×900 | F | fill, int | Account-sidebar | 3510 |
| 46 | `Data-delete-confirm` | Data and privacy, confirming deletion | 1440×900 | O | fill | Data | 5030 |
| 47 | `Data-delete-confirm-mobile` | Data and privacy, confirming deletion, mobile | 390×844 | W | none | Data-delete-confirm | 6550 |
| 48 | `Data-deleted` | Account deletion scheduled, signed out | 1440×900 | B (solo) | fill | none | 7020 |

**Row 8: Account deletion** (y = 10200)

- Title note at (0, 9940): **"Account deletion: 30 days to change your mind, then 30 days to download your data"**, `maxW` 8000 (the row spans 9980).
- `Deletion-pending` sits where it was dragged on the canvas, at (−189, 10144), just off the row’s grid. That is intentional, not a defect.
- On the export boards, "today" is December 9, 2026.

| # | File | Board title | Size | Kind | fill / int | Imports | x |
|---|---|---|---|---|---|---|---|
| 49 | `Deletion-pending` | Signing in, account scheduled for deletion | 1440×900 | B (solo) | fill | none | -189 (y 10144) |
| 50 | `Export-request` | Download your data after deletion | 1440×900 | B (solo) | fill | none | 1520 |
| 51 | `Export-request-mobile` | Download your data after deletion, mobile | 390×844 | W | none | Export-request | 3040 |
| 52 | `Export-request-code` | Download your data, enter the code | 1440×900 | B (solo) | fill | none | 3510 |
| 53 | `Export-requested` | Download your data, preparing | 1440×900 | B (solo) | fill | none | 5030 |
| 54 | `Signin-deleted` | Signing in, account already deleted | 1440×900 | B (solo) | fill | none | 6550 |
| 55 | `Export-start` | Download your data after deletion, without a link | 1440×900 | B (solo) | fill | none | 8070 |
| 56 | `Export-start-mobile` | Download your data after deletion, without a link, mobile | 390×844 | W | none | Export-start | 9590 |

**Layout notes:**

- **Columns** are desktop, mobile, dark, then states, as the brief asks. Gaps are 80 px. Row pitch is the tallest board plus 400: rows sit at y 0, 1400, 2700, 4600, 5900, 7600, 8900 and 10200. Each title note’s `maxW` is the row’s span (right edge of the last board), capped at 8000 on the canvas.
- **Order:** `Main` first, then rows top to bottom, left to right. `canvas.json`: `"v": 3`, title "Richie Account Settings", `"launch": {"view": "canvas"}`, `"designSystems": []`. If the canvas supports a focused or pages entry point, point it at `Main` so reviewers can click through (`is_interactive` boards link to each other).

### 6.3 Harness

1. Read the boards with the Artifact tool, as in how-we-design §8.
2. Build: `node .design/wp3-1/harness/build-boards.mjs <project> <out> <blobs>`. A smoke test with sample boards confirmed that nested imports (overlay → page → sidebar part), wrappers, link rewriting and dark builds work.
3. Build again with `BOARD_THEME=dark` into a second folder to cover **dark for every state**.
4. Adapt `shoot.js` (base URL, board list, sizes in §5) and screenshot both folders.
5. Expected reports:
   - no horizontal overflow at 320;
   - every interactive target ≥24×24 on fine pointers;
   - ≥44 px hit area below 768 or on coarse pointers (§7.3).
   - The `small` list in `shoot.js` will include 32 px row buttons on desktop by design.

---

## 7. CSS plan (`project/account.css`)

**Naming:**

- Components use **the suiss `data-slot` / `data-variant` / `data-size` attributes** from suiss guide §7. Copy that kit as the base and apply the overrides below, scoped under `.ra`.
- Richie-only compositions use the `ra-` prefix.
- Mocks map 1:1: an element with `data-slot="item"` becomes `<Item>`.

### 7.1 Tokens

**Theme mechanism:**

- `:root` holds the light values.
- `.dark` on any ancestor (the board wrapper, or `<body>` in the harness) holds the dark values.
- `@media (prefers-color-scheme: dark) { [data-appearance="system"] { …the dark values… } }` lets an element set to System follow the device.
- Base boards carry no `data-appearance`, so they always draw light. In the app, the theme script sets `.dark` on `<html>`, because the Tailwind `dark` variant is class-based (§8).
- `html:has(.dark)` and `html:has(.ra[data-appearance="system"])` (under the media query) paint the root black. `color-scheme` follows the theme.

**suiss tokens, unchanged:** names as in `globals.css`.

| Token | Light | Dark |
|---|---|---|
| `--background` | `#ffffff` | `#000000` |
| `--foreground` | `#1d1d1f` | `#f5f5f7` |
| `--card` / `--popover` | `#ffffff` | `#1d1d1f` |
| `--primary` (text and icons) | `#0071e3` | `#2997ff` |
| `--primary-foreground` | `#ffffff` | `#ffffff` |
| `--secondary` | black 4 % | white 10 % |
| `--muted` / `--sidebar` | `#f5f5f7` | `#161617` |
| `--muted-foreground` | `#6e6e73` | `#a1a1a6` |
| `--accent` / `--sidebar-accent` | black 6 % | white 10 % |
| `--destructive` | `#d0000a` | `#ff5447` |
| `--border` / `--sidebar-border` | `#d2d2d7` | `#424245` |
| `--input` | `#86868b` | `#6e6e73` |
| `--sidebar-primary` | `#0071e3` | `#2997ff` |
| `--radius` | 10 px | |
| `--r-md` / `--r-lg` / `--r-xl` / `--r-2xl` / `--r-3xl` | 8 / 10 / 12 / 18 / 28 | |

**Richie extensions** (`--ra-*`; each has a reason):

| Token | Light | Dark | Why |
|---|---|---|---|
| `--ra-fill` (filled buttons, switch on, the preview’s messenger header, customer bubble and send button) | `#0071e3` | `#0071e3` | White on the dark suiss primary is 3.0:1 |
| `--ra-fill-hover` | `#0068d6` | `#0068d6` | The hover darkens; lightening would fail |
| `--ra-link` (links, plain buttons) | `#0066cc` | `#2997ff` | 5.11 on Mist (light) |
| `--ra-focus` | `#0071e3` | `#2997ff` | A 2 px ring visible on black |
| `--ra-halo` | `rgb(0 113 227 / .3)` | `rgb(41 151 255 / .35)` | The 3 px halo outside the ring |
| `--ra-danger-on-tint` | `color-mix(in oklab, var(--destructive), var(--foreground) 15%)` ≈ `#b31917` | ≈ `#ff7263` | Destructive button text |
| `--ra-selected-edge` | `#86868b` | `#8e8e93` | Selected segment edge |
| `--ra-tile` | `#e8e8ed` | `#2c2c2e` | Neutral row media |
| `--ra-avatar-fallback` | `#6e6e73` | `#636366` | Flat initials fill |
| `--ra-success` (icons only) | `#16a34a` | `#4ade80` | Tailwind green-600 / green-400 |
| `--ra-scrim` | `rgb(0 0 0 / .5)` | `rgb(0 0 0 / .5)` | suiss overlay |
| `--ra-msgr-bubble` | `#f5f5f7` | white 10 % | Teammate bubble |

**Contrast** (computed by `.design/wp3-1/contrast.mjs`; alpha composited over the real surface; full output in `contrast-report.md`). Text needs 4.5:1, non-text 3:1.

| Pair | Light | Ratio | Dark | Ratio |
|---|---|---|---|---|
| Body text on page | `#1d1d1f` on `#ffffff` | 16.83 | `#f5f5f7` on `#000000` | 19.29 |
| Body text on grouped list or sidebar | on `#f5f5f7` | 15.46 | on `#161617` | 16.61 |
| Body text on dialog, sheet, toast | on `#ffffff` | 16.83 | on `#1d1d1f` | 15.46 |
| Input text on filled field (page / dialog) | on `#f5f5f5` | 15.41 / 15.41 | on `#1a1a1a` / `#343435` | 16.07 / 11.48 |
| Nav label on active or hovered item | on `#e6e6e8` | 13.54 | on `#2d2d2e` | 12.58 |
| Initials or glyph on tile | on `#e8e8ed` | 13.78 | on `#2c2c2e` | 12.80 |
| Slate text on page | `#6e6e73` on `#ffffff` | 5.07 | `#a1a1a6` on `#000000` | 8.16 |
| Slate text on grouped list or stage | on `#f5f5f7` | 4.66 | on `#161617` | 7.03 |
| Slate text on dialog or toast, unavailable button | on `#ffffff` | 5.07 | on `#1d1d1f` | 6.54 |
| Placeholder on filled field (page / dialog) | on `#f5f5f5` | 4.64 / 4.64 | on `#1a1a1a` / `#343435` | 6.80 / 4.86 |
| Link or plain button on page | `#0066cc` on `#ffffff` | 5.57 | `#2997ff` on `#000000` | 6.96 |
| Link or plain button on grouped list (also on hover: underline, no fill) | on `#f5f5f7` | 5.11 | on `#161617` | 6.00 |
| Link on dialog | on `#ffffff` | 5.57 | on `#1d1d1f` | 5.58 |
| Action-sheet action and cancel text (`--primary`) | `#0071e3` on `#ffffff` | 4.70 | `#2997ff` on `#1d1d1f` | 5.58 |
| White on filled button / hover | on `#0071e3` / `#0068d6` | 4.70 / 5.31 | same | 4.70 / 5.31 |
| Error text on page / grouped list / dialog | `#d0000a` | 5.70 / 5.23 / 5.70 | `#ff5447` | 6.61 / 5.69 / 5.30 |
| Destructive button text, page / list / dialog | `#b31917` on its tint | 5.70 / 5.25 / 5.70 | `#ff7263` on its tint | 6.39 / 5.21 / 4.82 |
| Input text on invalid field | on `#f3e9e9` | 14.10 | on `#301f1e` | 14.33 |
| White initials on avatar fallback | on `#6e6e73` | 5.07 | on `#636366` | 5.99 |
| Preview: white on Signal (header title and subline, customer bubble text) | on `#0071e3` | 4.70 | same | 4.70 |
| Preview: reply text on teammate bubble | on `#f5f5f7` | 15.46 | on `#343435` | 11.48 |
| Preview: name label and timestamp on messenger | `#6e6e73` on `#ffffff` | 5.07 | `#a1a1a6` on `#1d1d1f` | 6.54 |
| *Non-text* | | | | |
| Field and outline-button edge vs page / list / dialog | `#86868b` | 3.62 / 3.33 / 3.62 | `#6e6e73` | 4.14 / 3.57 / 3.32 |
| Selected segment edge vs track | `#86868b` on `#ebebed` | 3.05 | `#8e8e93` on `#2d2d2e` | 4.20 |
| Switch off track vs grouped list | `#86868b` on `#f5f5f7` | 3.33 | `#6e6e73` on `#161617` | 3.57 |
| Switch on track vs grouped list | `#0071e3` on `#f5f5f7` | 4.31 | on `#161617` | 3.85 |
| Switch thumb vs on track | `#ffffff` on `#0071e3` | 4.70 | `#f5f5f7` on `#0071e3` | 4.31 |
| Focus ring vs page / list / dialog | `#0071e3` | 4.70 / 4.31 / 4.70 | `#2997ff` | 6.96 / 6.00 / 5.58 |
| Filled button vs page | `#0071e3` on `#ffffff` | 4.70 | on `#000000` | 4.47 |
| Active nav icon vs active fill | `#0071e3` on `#e6e6e8` | 3.78 | `#2997ff` on `#2d2d2e` | 4.54 |
| Success icon vs page / dialog | `#16a34a` | 3.30 / 3.30 | `#4ade80` | 12.05 / 9.66 |
| Danger icon and invalid edge vs page / dialog | `#d0000a` | 5.70 / 5.70 | `#ff5447` | 6.61 / 5.30 |
| *Reported, not required* | | | | |
| Hairline vs grouped list (decorative) | `#d2d2d7` on `#f5f5f7` | 1.38 | `#424245` on `#161617` | 1.81 |
| Grouped list vs page (surface) | | 1.09 | | 1.16 |
| suiss filled field without our edge (why the edge exists) | | 1.09 | | 1.20 |
| Preview subline at 85 % white (rejected; the subline is solid white) | `#d9eafb` on `#0071e3` | 3.82 | same | 3.82 |

### 7.2 Recipes in `account.css`

The rows are in file order. "suiss" means: copy the selector from the guide §7 kit, then apply the stated override.

**One source of truth per concern.** `account.css` (the canvas stylesheet) is the source for **values**: sizes, spacing, colours and timings. This spec is the source for **behaviour, copy and states**, and the rows below explain what each recipe is for. When a value here differs from `account.css`, use `account.css`, and report the difference so this table can be corrected.

| # | Recipe | Selector(s) | Spec |
|---|---|---|---|
| 1 | Base | `x-dc{display:contents}`, `.ra`, `.ra--part`, `.ra-sr`, `.ra-layer` | **`.ra`:** font stack, 16/24, Ink on Paper, antialiased, `text-size-adjust:100%`, `box-sizing` reset, `:where(h1,h2,p,ul,figure)` margin 0. **`.ra--part`:** transparent background, no min-height (fragments and overlay layers). **`.ra-layer`:** `position:fixed; inset:0; z-index:50`. The `<div inert>` just before it is pinned with `position:fixed; inset:0`, so the document is one viewport tall. **`.ra-sr`:** visually hidden. **Durations** are tokens (`--ra-dur` 150 ms, `--ra-dur-ind` 200 ms) that reduced motion sets to 0 on `:root` |
| 2 | Shell | `.ra-shell[data-page]`, `.ra-main`, `.ra-page` | Shell: grid `256px 1fr`, `min-height:100svh`. Main: padding 40 36 96. Page: `max-width:680px; margin-inline:auto; container-type:inline-size`. **≤767:** shell becomes block, sidebar hidden, main padding 0 16 64. **`.ra-skip`**: the first child of the shell; visually hidden until `:focus-visible`, then `position:fixed`, top 8, start 8, z 60, a 44 px outline pill with the focus ring |
| 3 | Sidebar | `.ra-side-part`, `[data-slot=sidebar-container]`, `.ra-side-back`, `.ra-side-rule`, `.ra-side-head`, `[data-slot=sidebar-menu]`, `[data-slot=sidebar-menu-button]`, `.ra-side-foot` | suiss kit. A plain `div` container holding `nav[aria-label=Account]` (§3.2). Sticky, `height:100svh`, overflow auto, padding 8. **Head:** padding 16 12 20, photo 64, 12 gap; the email follows row 15. **Active:** `.ra-shell[data-page=X] [data-nav=X]` gets the suiss `[data-active]` look plus `font-weight:600` and the icon in `--sidebar-primary`. **Rules** under the back link and above the footer are `border-top: 1px solid var(--sidebar-border)`, not painted backgrounds. **Footer:** `margin-top:auto`. **Below 768:** hidden, scoped to `.ra-shell .ra-side-part` so the standalone part board still shows. Items are 44 on coarse pointers |
| 4 | Mobile bar and back link | `.ra-mobilebar`, `.ra-back` | Hidden ≥768. The bar is 44 px, the back link `--ra-link` 16/24 with `ArrowLeft` 16, hit area 44, no underline (it is navigation chrome; underline on hover) |
| 5 | Page header | `.ra-page-head`, `.ra-title`, `.ra-lede` | Title 30/36/600 −0.025em, `text-wrap:balance`, `tabindex=-1` focus target with no outline (it is not a control). Lede 16/24 Slate, max 560, margin-top 8. Head margin-bottom 32 |
| 6 | Section | `.ra-section`, `.ra-section-head`, `.ra-h2`, `.ra-desc`, `.ra-section-foot`, `.ra-note` | Sections 40 apart (`.ra-section + .ra-section`). `h2` 18/28/600. Description 14/20 Slate, max 560, 4 below the `h2`. Head → group 12; group → foot 12. **Foot:** a column: the action row (flex, gap 12), then 8 below it the status slot (row 16, reserved by column width), then notes. Note 14/20 Slate with underlined links, always inside the foot, never with an inline margin |
| 7 | Inset group | `[data-slot=item-group][data-variant=inset]` | suiss: radius 18, Mist, overflow hidden. Rendered as `<ul role="list">` with `<li>` rows. Separators are drawn by CSS (`[data-slot=item]+[data-slot=item]::before`, 1 px `--border`, inset 62 px when `.has-media`, else 16) instead of `ItemSeparator` elements, so the list markup stays valid |
| 8 | Row | `[data-slot=item]` (+`data-size=sm`), `item-media` (`.ra-tile`), `item-content`, `item-title`, `item-description`, `item-actions`, `a[data-slot=item]`, `button[data-slot=item]` | suiss. **Padding 14 16** (it produces the 52 / 76 / 96 heights), gap 14, min-height 52. Media and actions sit in a 24 px box on the title line, so 32 and 36 px controls don’t change the height. Title 16/24/400 (inset convention), clamp 1 line (`.wrap-2` allows 2, `.wrap-all` wraps freely). Description 14/20 Slate, no clamp. Actions gap 8. **≥2-line rows:** actions `align-self:flex-start`. **Controls:** 32 tall; 36 below a 480 px column (container query), below 768 px and on coarse pointers (media query). **`@container (max-width:479px)`:** actions `flex-basis:100%`, `padding-inline-start:46px`, except `.keep-inline`. **Trailing plain `sm` action:** `margin-inline-end:-12px`, so its text lines up with the actions edge. **Link rows:** hover `--accent`, trailing `CaretRight` 16 Slate (`.ra-caret`), `rtl` mirrored, focus ring inset (`outline-offset:-2px`). **Button rows** contain only phrasing content (`span`s) |
| 9 | Tile | `.ra-tile` | 32 × 32, radius 8, `--ra-tile` fill, Ink glyph 16 or initials 12/600. Rows with tiles get `.has-media` |
| 10 | Buttons | `[data-slot=button]` with `data-variant=default\|outline\|plain\|destructive\|ghost`, `data-size=sm\|default\|lg\|icon-sm`, `data-block` | suiss kit with overrides. **default:** `--ra-fill`, hover `--ra-fill-hover`. **outline:** edge `--input` (not `--border`). **plain:** `--ra-link`, hover underline 1 px offset 3, no fill. **destructive:** text `--ra-danger-on-tint`. **ghost `icon-sm`:** toast Dismiss only. Labels 14/20/400 (lg: 16/24). **≤767 or `(pointer:coarse)`:** sm becomes 36 tall with an `::after` hit area to 44. Never `opacity` for disabled. Spinners inside buttons are `aria-hidden` with no `role` |
| 11 | Busy and unavailable | `[data-slot=button][aria-disabled=true][data-busy]`, `[data-slot=button][aria-disabled=true]:not([data-busy])` | **Busy:** keeps its variant and shows `[data-slot=spinner]` (16, 8 spokes, `steps(8)` 0.8 s). **Unavailable:** transparent fill, `--input` edge, Slate label, `cursor:default`. Both drop the hover |
| 12 | Fields | `[data-slot=field]`, `field-label`, `[data-slot=input]`, `field-description`, `field-error`, `.ra-fields` | suiss filled input (44, radius 10, `--secondary` fill, 16/24) **plus `border-color:var(--input)` at rest.** Focus: `border-color:var(--ra-focus); box-shadow:0 0 0 1px var(--ra-focus),0 0 0 4px var(--ra-halo)` (a 2 px ring in total, border plus inset). Invalid: suiss (destructive edge, 5 % tint, 3 px destructive/20 ring). The error line 14/20 Alarm has a `WarningCircle` icon 16. It has **no `role`** when focus moves to its field (submit validation), and `role=alert` only where focus stays elsewhere (§4.0). **Invalid and focused:** the blue focus edge and halo win; the red tint and the error line stay (one focus colour). Label → control 8, control → description or error 8, fields 20 apart (tighter than the suiss 28, because the form column is narrow). `.ra-fields` is a stack |
| 13 | Switch, segmented, picker | `[data-slot=switch]`/`switch-thumb` (`data-checked`), `fieldset[data-slot=toggle-group][data-spacing="0"]` with a sr-only `legend`/`toggle-group-item` (a `label` around a real radio, `.ra-seg-input`), `[data-slot=select-trigger][data-size=sm]` | **Switch:** suiss, checked track `--ra-fill`, `::after` hit area −12/−8. **Segmented** (a radio group, never toggle buttons): the container is a `fieldset` (border, margin and padding reset), named by a sr-only `legend` that repeats the row title. `sm` is 32 total: 2 px track padding and 28 px items, or 36 when small. The selected item (`:has(input:checked)`) gets `--card` fill (dark `--border`), `shadow-sm`, `1px solid var(--ra-selected-edge)` and a 600 label. The focus ring goes on the item via `:has(:focus-visible)`. **Select trigger `sm`:** 36 tall, filled, `CaretUpDown` Slate, `--input` edge. **Hit areas** below 768 and on coarse pointers: segmented items and select triggers extend `::after { inset: -8px 0 }` to at least 44 |
| 14 | Avatar, badge | `[data-slot=avatar]` (`data-size=sm\|default\|lg\|xl`), `avatar-fallback`, `[data-slot=badge][data-variant=secondary]` | **Avatar:** suiss sizes 24/32/40/64; fallback flat `--ra-avatar-fallback`, white 12/16/24 px initials. **Badge secondary:** Slate 12/600, no fill |
| 15 | Emails | `.ra-email` (sidebar, index, chip), `.ra-email-wrap` (rows and dialogs), `.ra-email-local`, `.ra-email-domain` | §4.0 long-email rule. **Markup:** `<span class="ra-sr">jamie.rivera@northwind.coffee</span><span aria-hidden="true" class="ra-email">…halves…</span>`. **`.ra-email`:** one line while the domain fits next to at least 3 local characters. The local part truncates first, with `text-overflow: ellipsis` on its span (no pseudo-element ellipsis). When the domain can’t fit, a container query switches to two lines: local part and "@" on line 1, the whole domain on line 2 (`overflow-wrap` at dots and hyphens only). The domain is never truncated. **`.ra-email-wrap`:** inline flow that breaks after the @ (`<wbr>`, no inline-block), and the domain is never truncated |
| 16 | Status slot and line | `.ra-slot`, `.ra-status` (+`.is-ok`, `.is-busy`, `.is-error`, `.is-quiet`) | **`.ra-slot`:** one grid cell that stacks the polite status and the alert in the same place. **In a section foot** its `min-height` is `var(--ra-status-lines)`: 20 px by default, 40 px under `@container (max-width: 479px)`, 60 px under `@container (max-width: 349px)` (§4.0). **Fixed slots** (`.ra-photo-slot`, `.ra-form-alert`) are 40 at every width. **`.ra-status`:** 14/20, gap 6, icon 16 inline with the text, `text-wrap: balance`. ok: `CheckCircle` in `--ra-success`, Ink text. busy: spinner, Slate. error: `WarningCircle` and Alarm text. **`.is-quiet`:** a static Slate line, not a live region. Inline actions (Undo) follow the polite paragraph inside the slot, never inside it |
| 17 | Alert | `[data-slot=alert]` (+`data-variant=destructive`), `alert-title`, `alert-description`, `.ra-alert-action` | suiss: radius 18, Mist, padding 20, icon 20 in `--primary` (destructive: Alarm). Description 16 Slate. **Change:** the action sits below the description, start-aligned, not at the top end, so it doesn’t collide with long titles at 320 |
| 18 | Overlay and dialog | `[data-slot=dialog-overlay]`, `[data-slot=dialog-content]`, `dialog-header`, `dialog-title`, `dialog-description`, `dialog-footer`, `dialog-close` | suiss. Width ≤448, padding 24, gap 24, radius 18, popover, hairline and `shadow-xl`. **Change:** title 18/28/600, aligned with the alert dialog. **Footer:** right-aligned for form dialogs, Cancel first, `lg` (44) buttons. This includes the delete-account dialog, a form dialog with a field. Only confirms (row 19) use equal halves. **No `dialog-close` button** (§4.0). The content is a `<form>`. Multi-state dialogs set `min-height` to their tallest state, and the commit button’s `min-width` to its widest label. Max-height `calc(100svh - 32px)`, the body scrolls, the header and footer stay fixed. When the body overflows, the header’s bottom edge and the footer’s top edge show a 1 px `--border` hairline. `.ra-dialog-body` has 8 px of bottom padding (with a matching −8 px margin), so `::after` hit areas don’t make it scroll. **≤767 (drawer look):** `inset:auto 8px 8px`, width auto, radius 28, centred header with the swipe handle (`::before` 36 × 5, radius full, `--border`, 8 from the top), footer `flex-direction:column-reverse` with block buttons, `padding-bottom:max(16px,env(safe-area-inset-bottom))`. **With the keyboard open**: the drawer’s max height follows `visualViewport.height` (a `--ra-vvh` variable updated on `visualViewport` resize; with `interactive-widget=resizes-content` in the viewport meta where supported). The body scrolls and the footer stays above the keyboard, and the focused field gets `scrollIntoView({ block: "nearest" })` |
| 19 | Confirm | `[data-slot=alert-dialog-content]`, `alert-dialog-header`/`title`/`description`/`footer` | suiss: max 340, centred text, title 18/600, description 14 Slate, `lg` buttons. **Footer:** `grid-template-columns: 1fr 1fr` (equal widths), Cancel outline then the destructive action. When a label doesn’t fit its half, it stacks with the action on top. While the action runs, the confirm stays open (§4.0). **≤767 (action-sheet look):** fixed bottom, padding 8, max 448 centred. The content becomes a grid with areas `head`, `action` and `cancel`, and the footer is `display:contents`, so the board markup stays header, footer(Cancel, action). **Header:** popover, radius 18 18 0 0, padding 16, title 14/600 Slate, description 14 Slate, centred. **Action:** popover, radius 0 0 18 18, 1 px `--border` top hairline, 56 tall, 18/28, `--destructive` text, no tint. These two pieces read as one group. **Cancel:** its own popover piece, radius 18, 8 below, 56 tall, 18/28/600, `--primary`. The visual order differs from the DOM order only in this drawing. The real `Confirm` renders `AlertSheet` with the correct order (§8) |
| 20 | Code entry | `.ra-otp` (`.is-invalid`, `.is-disabled`, `.is-focused`), `.ra-otp-input`, `[data-slot=input-otp-group]`, `[data-slot=input-otp-slot]` (`data-active`), `[data-slot=input-otp-separator]`, `.ra-code-meta` | The identity `rc-otp` structure renamed: one input over 8 slots, `container-type:inline-size`. Slots are filled `--secondary` with a 1 px `--input` edge (suiss uses 1.5 px transparent; our edge gives 3:1). Radius 10. Width `min(44px, (100cqi - 58px) / 8)` (58 = 6 gaps of 6 plus the 22 px separator zone), height 52 (44 below 360 cqi). Glyph 20/600 tabular uppercase. Group gap 6, separator 10 × 2 `--input`. Active slot: the focus ring. Invalid: destructive edge and tint. Disabled (locked: the input is `readonly` + `aria-disabled`): Mist fill, `--border` edges. **`.ra-code-meta`:** min-height 2 × 20 + 4, or 3 × 20 + 4 below a 360 px container, centred, `text-wrap: balance`. The error icon sits inline before the first word. The polite sr status element follows the meta on every code screen |
| 21 | Toast | `[data-slot=toast-viewport]`, `[data-slot=toast]`, `toast-content`, `toast-icon`, `toast-title`, `toast-description`, `.ra-toast-action` | suiss: bottom-end 16 (bottom-centre with 16 insets ≤639), max 384, radius 12, popover, soft shadow. Icon 24 (`CheckCircle` in success, `Info` in `--primary`). Title 14/600, description 14 Slate. **No action buttons** (§4.0). Close: ghost `icon-sm` × with `aria-label` "Dismiss". 5 s timeout, paused on hover and focus. Boards draw one toast, unstacked |
| 22 | Skeleton | `[data-slot=skeleton]` | suiss: radius 8, Mist (on Mist groups use `--ra-tile`), pulse 2 s. Text skeletons are 16 or 12 tall inside a line box of 24 or 20 so the row height matches the real row. Below a 480 px column, rows with actions add a 36 px pill skeleton on the wrapped action line. `aria-busy` goes on each loading group, and the loading status sits outside them |
| 23 | Empty | `[data-slot=empty]` (+`.is-filled`), `empty-icon`, `empty-title`, `empty-description` | suiss: radius 18, padding 48, gap 16, centred. `.is-filled` uses Mist. Icon tile 40 radius 10 (`--ra-tile` when filled), glyph 20. Title 18/600, description 14 Slate max 384 |
| 24 | Preview stage | `.ra-profile-grid`, `.ra-preview`, `.ra-preview-cap` | **Grid:** `@container (min-width:620px)` `minmax(280px,340px) minmax(308px,1fr)`, gap 32, `align-items:start`: the form shrinks first, and the stage never drops below 308. Below that, a single column, 32 gap: the form (its action row and alert slot under Save), then the stage. **Below 620 the stage is compact:** the figure keeps its caption (`h2` and description); `.ra-msgr-head`, `.ra-msgr-composer`, the customer message (`[data-slot=message][data-align=end]`) and the reply’s `message-footer` ("Just now") are `display: none`; `.ra-msgr-body` pads 14 at the bottom, and the avatar has no bottom margin; the window stays `aria-hidden`, and the sr summary stays. `.ra-photo-slot` and `.ra-form-alert` reserve 1 line (20) in a 420–619 px column. Values are as in `account.css` (canvas version `1793877977-4d5d`). **Stage** (`<figure>`): Mist, radius 18, padding 16, gap 16. Caption: `h2` and 14 Slate |
| 25 | Messenger | `.ra-msgr`, `.ra-msgr-head`, `.ra-msgr-body`, `[data-slot=message]` (`data-align`), `message-avatar`, `message-header`, `message-footer`, `[data-slot=bubble]` (`data-variant`, `data-align`), `bubble-content`, `.ra-msgr-composer` | **Window:** popover, radius 22 (the suiss chat-support panel), hairline `0 0 0 .5px var(--border)` plus `0 24px 60px rgb(0 0 0/.2)` in light (dark: hairline only), overflow hidden. **Head:** `--ra-fill`, padding 12 14, white text, no mark or logo; title 14/600 "Chat with us"; subline 12/16 solid white (85 % white fails at 3.82:1). **Body:** padding 14 14 8, gap 12 (as built). **Bubbles:** max width `min(250px, 88%)`, so the reply wraps in 3 lines at 276. **Customer bubble** (`data-variant=default` with `--ra-fill`, align end): white 14/20, radius 18, padding 6 14, the suiss tail shape. **Teammate bubble:** `--ra-msgr-bubble`, Ink. In dark it is opaque `#343435`, the same colour composited, so the tail doesn’t double the alpha. Message header (name) and footer (time) 12/16 Slate. The avatar is 24 at the bottom of the bubble. **Composer:** 40 tall pill, `--input` edge, 14 Slate placeholder, a 28 `--ra-fill` circle with `PaperPlaneRight` 14 white. The whole window is `aria-hidden` |
| 26 | Photo crop | `.ra-crop` (`role=application`, §4.2), `.ra-crop-mask`, `.ra-slider-head`, `[data-slot=slider]` | Frame `min(280px, 100svh - 360px)`, min 160, square, radius 18; the image is a centred SVG. Focus ring on the frame (`:focus-visible`). The mask is `radial-gradient(circle 140px, transparent 139px, rgb(0 0 0/.5) 140px)`, a functional overlay, not decoration. A 2 px white ring on the circle. **Slider:** suiss track 4 `--border`, range `--ra-fill`, thumb 32 × 20 white with `shadow-md` and an `--input` edge. The slider box is 44 tall below 768 px and on coarse pointers |
| 27 | Mobile index | `.ra-index`, `.ra-index-groups`, `.ra-index-account` | **Index:** max 680 centred (harmless at desktop), gutters 16, groups 20 apart, `h1` margin 8 0 20. Groups 1–3 sit inside `nav[aria-label=Account]`. **Account row:** `a[data-slot=item]`, avatar `xl`, title 16/600 with the sr suffix ", profile", and the email as the description (row 15) |
| 28 | Solo page | `.ra-solo`, `.ra-solo-col`, `.ra-mark`, `.ra-solo-title`, `.ra-solo-text`, `.ra-solo-actions`, `.ra-chip`, `.ra-foot` | Used by `Data-deleted` and Row 8. Centred column, max 460, top padding 64 (≥768) or 32. Shiba 1× (31 × 39) at every width, a brand mark 16 above the title, pixelated. Title 30, `tabindex=-1`. Body 16 Slate. Actions stacked or in a row, 44 tall (`lg`), full width below 480. **`.ra-chip`** is the identity account chip on `Export-request`: initials avatar 28 and the email, using the row 15 markup (sr-only full address, `aria-hidden` halves). `Export-request-code` has no chip: it shows the code entry (row 20) under the body. Footer links 12 Slate, gap 20, padded to 44 on coarse pointers |
| 29 | Canvas-only | `.is-focused` | Draws the same ring as `:focus-visible` on fields, slots, buttons, the crop area and focused messages |
| 30 | Behaviour-bearing utilities | `.has-media` | On a group or a row: hairlines at 62 and wrapped actions at the text edge |
| | | `.keep-inline` | Row actions that stay on the title line below 480 (switches, short badges, short values) |
| | | `.wrap-2`, `.wrap-all` | Row-title line clamps |
| | | `.ra-value` | Trailing Slate value, not a control |
| | | `.ra-row-empty` | One Slate line as the only row of an empty list |
| | | `.ra-slot`, `.is-quiet` | Row 16 |
| | | `.ra-page-alert` | The offline alert between the lede and the first section, 32 above and below, `role=alert` |
| | | `.ra-form-actions`, `.ra-form-buttons` | The 36 px name action row and its buttons, with the 40 px alert slot under it |
| | | `.ra-photo` | The photo row: avatar, buttons and the photo status slot |
| | | `.ra-stack` | Stacked block buttons (step-up screen A) |
| | | `.ra-fields` | Field stack, 20 apart |
| | | `.ra-list` | Bulleted Body list in dialogs |
| | | `.ra-num` | `tabular-nums` |
| | | `.ra-caret` | Trailing disclosure icon |
| | | `.ra-avatar-scrim` | Uploading spinner over the avatar |
| | | Each maps to a className on the suiss part named in §8, or to a small wrapper component | |
| 31 | Canvas mechanics | `.ra-steps`, `.ra-step-ghost` | **One height for multi-state content.** `.ra-steps` is `display:grid`, and every child sits in `grid-area: 1 / 1`, so the cell is as tall and wide as its largest state. The inactive states carry `.ra-step-ghost` (`visibility:hidden`, plus `inert` / `aria-hidden` in markup). Used for: the change-email steps (both panes in the body), the step-up description variants, step-up screens A and B, and a commit button whose label changes ("Confirm" and "Send a new code in 0:41" stacked inside the button, `justify-items:center`, tabular) |
| | | `.ra-step-busy` | A ghost busy label that also reserves the spinner’s 16 px and its gap (an 18 px `::before`), so "Checking code…" with its spinner fits the same width |
| | | `.ra-dialog-form` | The `<form>` around a dialog’s body and footer (§4.0). `display:contents`, so the body and footer stay direct flex items of the dialog |
| | | `.ra-dialog-pane` | One step’s body content inside `.ra-steps`: a column, gap 20 |
| | | `.ra-form-alert` | The name form’s alert slot under the 36 px action row: `min-height` 40, `margin-top` 8 |
| | | `.ra-photo-row` | Avatar and actions on one line, gap 16. Below a 344 px container, `min-height` 86 (the stacked height), with or without a photo |
| | | `.ra-photo-actions` | Change photo (outline) and Remove photo (plain), gap 14 / 16. The plain button pulls back 13 px so its label keeps one start edge. Below 344 px, a column |
| | | `.ra-photo-slot` | The photo status slot: the hint, an error, or a status. `min-height` 40 |
| | | `.ra-photo-status` | A flex line holding the polite "Photo removed." paragraph and the plain **Undo**, drawn at text height (20) so the line stays one line. Its hit area is still 44 |
| | | `.ra-skel-text`, `.ra-skel-detail`, `.ra-skel-tile`, `.ra-skel-pill`, `.ra-skel-actions`, `.ra-skel-narrow`, `.ra-w40`, `.ra-w50`, `.ra-w60` | Skeleton shapes (row 22). Text bars sit in 24 or 20 px line boxes, and consecutive detail lines overlap by 4 px, like real descriptions. The tile is 32. The pill is 80 × 36, shown only on the wrapped action line below a 480 px column. `.ra-skel-narrow` is an extra detail line drawn only below 480, where the real description wraps. Widths are 40, 50 and 60 % |
| | | `.ra-trunc` | One-line ellipsis truncation (`min-width:0; overflow:hidden; white-space:nowrap; text-overflow:ellipsis`) |
| | | `.ra-bar` | The 44 px back bar. Always shown on the mobile index and solo pages; `.ra-mobilebar` shows it only below 768 |
| | | `.ra-inline` | A wrapping flex line, gap 8, for a lone section action (the delete button) |
| | | **Delete from `account.css`** (dead): `.ra-p`, `.ra-strong`, `.ra-toast-info` | Not used by any board |
| 32 | Cosmetic classes | `.ra-side-nav`, `.ra-side-id`, `.ra-side-name`, `.ra-side-email` | Sidebar internals: the nav column (flex, so the footer sits at the bottom), the header block, the 2-line name clamp, the 14 Slate email |
| | | `.ra-index-title` | The index `h1` margins (8 0 20) |
| | | `.ra-form`, `.ra-profile-form` | The Profile form column and its field-then-action-row stack (gap 16) |
| | | `.ra-hint`, `.ra-detail`, `.ra-center` | 14/20 Slate hint and detail text (`text-wrap` pretty and balance), and centring, which also balances centred status lines |
| | | `.ra-email-at` | The "@" piece of `.ra-email` (never shrinks) |
| | | `.ra-otp-slots` | The slot row inside `.ra-otp` (flex, gap 6, centred) |
| | | `.ra-step-footer` | A footer replica inside a ghost pane (screen A carries screen B’s footer), 4 above |
| | | `.ra-msgr-head-text`, `.ra-msgr-title`, `.ra-msgr-sub`, `.ra-msgr-placeholder`, `.ra-msgr-send` | Messenger header text column, title (14/600), solid-white subline (12/16), composer placeholder (one-line ellipsis) and the 28 px send circle |
| | | `.ra-toast-text`, `.ra-toast-actions`, `.ra-toast-ok` | The toast text column, the Dismiss cell (20 tall on the title line) and the success icon colour |
| | | `.ra-empty-text` | The empty state’s title and description column (gap 4) |
| | | `.ra-solo-link`, `.ra-link-solo` | The solo page’s link line (24 above) and its standalone link (`min-height` 24) |
| | | `.ra-alert-action`, `.ra-avatar-scrim`, `.ra-row-empty` | Not on any board, but they implement specified copy-only states: the load-error action, the uploading scrim and an empty list row. Keep them |
| | | All of the above map to classNames on the suiss parts in §8, or to Tailwind utilities | |

### 7.3 Targets

- **Fine pointer, ≥768:**
  - `sm` buttons are 32 px tall and at least 32 wide (above the AA 24 px minimum).
  - Plain inline buttons get an `::after` hit area of 44 px tall.
  - Switches use the suiss extended hit area.
- **Below 768 or `(pointer: coarse)`:** every control’s hit area is at least 44 × 44 (padding or `::after`). That includes segmented items, select triggers, the Zoom slider and sidebar items. Row and link heights are at least 52.
- **Hit areas never extend past a scroll container’s padding.** Dialog bodies carry 8 px of bottom padding for this (row 18).

### 7.4 Focus

- **One look:** a 2 px `--ra-focus` ring at a 2 px offset, plus the `--ra-halo`, on `:focus-visible` only.
  - Buttons and links use `outline: 2px solid var(--ra-focus); outline-offset: 2px; box-shadow: 0 0 0 5px var(--ra-halo)`. The 5 px spread fills the 2 px offset gap and shows 1 px outside the ring.
  - Fields and slots: see row 12.
  - Link rows draw the outline inset, following the group’s radius at the ends.
- **Destructive buttons keep the blue ring.** That departs from the suiss red ring: one focus colour is easier to see, and a red ring reads as an error.
- **Programmatic focus targets show no ring**: any element focused by script with `tabindex="-1"`, such as page `h1`s, section `h2`s, dialog titles, the locked step-up error message, the empty-state title and solo-page titles. The ring is only for elements people can operate.
- Focus order follows the DOM. Dialogs trap focus (Base UI). **Initial focus per dialog**:

| Dialog | Initial focus |
|---|---|
| Any confirm (remove passkey, sign out of other devices, remove access, discard changes) | Cancel |
| Change email, step 1 / step 2 | New email field / code input |
| Rename passkey | Name field, text selected |
| Adjust your photo | The crop area |
| Step-up, every entry path (a confirm, a row action, a page button) | Screen A: its title (`tabindex="-1"`). Screen B: the code input. Screen C: its title |
| Step-up screen C | Its title (`tabindex="-1"`). Back returns focus to **Can’t get the code?** |
| Step-up locked state | The error message (`tabindex="-1"`) |
| Delete dialog | The typed field, scrolled into view |
| Solo pages (Row 8, `Data-deleted`, `Signin-deleted`) | The `h1` (`tabindex="-1"`) |

- **After an action:** focus returns to the trigger, unless a rule names another target: a removed row → the section `h2`; no `h2` → the `h1` or the empty-state title; Remove photo → Upload photo; Undo → Change photo.

### 7.5 Dark mode

- Every recipe uses tokens; no recipe hard-codes a colour, except the shiba image.
- Dark differences that are more than token swaps:
  - the segmented selected fill is `--border` (suiss);
  - the switch thumb is Ink;
  - the messenger drops its drop shadow (invisible on black) and keeps the hairline;
  - toasts use `shadow-lg` with no ring (suiss).

### 7.6 Motion and reduced motion

| What | Motion | Reduced |
|---|---|---|
| Hover and colour changes | 150 ms | Instant |
| Dialog in/out | Fade and `scale(.95→1)`, 100 ms (suiss) | Fade 100 ms, no scale |
| Drawer and action sheet | Slide 40 px plus fade, 400 ms `cubic-bezier(.32,.72,0,1)` | Fade 150 ms |
| Toast | Slide up 500 ms `cubic-bezier(.22,1,.36,1)` | Fade 150 ms |
| Switch thumb, segmented indicator | 200 ms iOS ease | Instant |
| Preview name and photo update | 150 ms crossfade | Instant |
| Row removal | Height collapse 250 ms (the suiss grid-rows trick) | Instant |
| Spinner | `steps(8)` 0.8 s | 2.4 s (still turning, so the wait is visible) |
| Skeleton | Pulse 2 s | Static at 100 % |
| Code caret | Blink 1 s | Steady |

### 7.7 Forced colours (`@media (forced-colors: active)`)

- Groups, tiles, dialogs, sheets, toasts, the messenger window, bubbles and alerts get `outline: 1px solid transparent; outline-offset: -1px`, so their shapes survive.
- **Badges** get no outline and `border: 0`: they are text-only, and a frame clipped their labels.
- **Action sheets:** only the visible pieces (header and action group, Cancel) are outlined, not the full-width `alert-dialog-content`.
- **Unavailable buttons** (`[aria-disabled]:not([data-busy])`) use `color: GrayText; border-color: GrayText`, so the countdown button doesn’t look enabled.
- **Bubble tails** are hidden, so the bubble borders stay continuous.
- **Sidebar rules** are borders, so they survive.
- Fields, slots and outline buttons keep their borders (they become `CanvasText`). Invalid fields and slots switch to 2 px borders.
- Focus is `outline: 2px solid Highlight`.
- The active nav item and the selected segment get `background: Highlight; color: HighlightText` with `forced-color-adjust: none`.
- The switch: checked track `Highlight`, unchecked track `ButtonFace` with a `ButtonText` border, thumb `ButtonText`.
- Spinners and the code separator use `CanvasText`. Disabled slots use `GrayText`.
- The preview header and customer bubble lose the Signal fill (expected). Bubbles keep their borders.
- The crop scrim keeps `forced-color-adjust: none`, because the mask is functional.

---

## 8. Handoff mapping

Each recipe maps to suiss components, imported in apps as `@repo/ui/components/ui/<name>`, installed with `pnpm ui:add @esuiss/<name>`. All mappings were checked against `suiss-ui-guide.md` (it now exists). Rows marked *(unverified)* rest on registry class strings that were not rendered.

| Recipe (§7.2) | Build with | Notes and overrides |
|---|---|---|
| 2–3 Shell, sidebar | `sidebar` (`SidebarProvider`, `Sidebar collapsible="offcanvas"`, `SidebarHeader`, `SidebarContent`, `SidebarMenu`, `SidebarMenuButton isActive render={<Link/>}`, `SidebarFooter`, `SidebarInset`). Block reference: `settings-account` | Set `aria-current="page"` on the active button (the boards can’t). **Do not** use `app-shell`: its mobile tab bar is wrong for settings. Below 768, render the index route instead of the Sheet. Add `font-semibold` to the active label. The container is a plain `div` (render `Sidebar` without its complementary landmark) with one `<nav aria-label="Account">` around the back link, header, menu and footer |
| 4 Mobile bar | `trail` pattern (it becomes a back link on mobile), or a plain `Link` with an `ArrowLeft` icon | `href="/account"`, not `history.back()` |
| 5–6 Page header, section | Own `SettingsPage`, `SettingsSection` wrappers (suiss lacks them, catalog §5) | `h1 text-3xl font-semibold tracking-tight`, `h2 text-lg font-semibold`, `p text-sm text-muted-foreground` |
| 7–9 Group, row, tile | `item` (`ItemGroup variant="inset"` rendered as `ul role="list"`, `Item size="sm" role="listitem"` as `li`, `ItemMedia`, `ItemContent`, `ItemTitle className="font-normal"`, `ItemDescription`, `ItemActions`). Blocks: `users-detail` (sessions), `settings-device` (link rows) | **Don’t use `ItemSeparator`:** inside `role=list` it is a non-listitem child (axe `list`, `aria-required-children`). Draw separators in CSS on `Item + Item` (§7.2 row 7): 62 px in for media rows, 16 otherwise, as a `data-variant=inset` style in `packages/ui`. Link rows: `Item render={<Link/>}` with `CaretRight`. The tile is `ItemMedia` with `size-8 rounded-md bg-[--ra-tile]` |
| 10–11 Buttons | `button` (`variant` default, outline, plain, destructive; `size` sm, default, lg; `block`), `spinner` | Patch `button.tsx` in `packages/ui`: default `bg-[--ra-fill] hover:bg-[--ra-fill-hover]`; outline `border-input`; plain `text-[--ra-link] hover:bg-transparent hover:underline`; destructive `text-[--ra-danger-on-tint]`; replace `disabled:opacity-50` with `aria-disabled` styles for busy and unavailable; keep the blue focus ring on destructive. Busy: `<Spinner data-icon="inline-start" aria-hidden role={undefined}/>`, because suiss `Spinner` is `role=status` and must not nest a live region inside the button; the button’s label carries the busy text. `ghost size="icon-sm"` only for the toast Dismiss |
| 12 Fields | `field` (`Field`, `FieldLabel`, `FieldDescription`, `FieldError`, `FieldGroup`), `input` | `Input className="border-input"`. `FieldGroup` gap 20 in the Profile form. Set `aria-invalid` yourself. `FieldError` has `role=alert` built in: drop it (patch the component, or pass `role={undefined}`) wherever submit moves focus to the field, so the error is read once |
| 13 Switch, segmented, picker | `switch`; **`radio-group`** (Base UI `RadioGroup`) styled as the suiss segment (the `toggle-group spacing={0}` look); `picker` pattern (`select` on desktop, `native-select` on mobile) | **Appearance and Time format are radio groups, not `ToggleGroup`**. ToggleGroup items are toggle buttons (`aria-pressed`) that can be deselected to no value. Render it as a `fieldset` (`<RadioGroup render={<fieldset/>}>`) with a sr-only `<legend>` holding the row title, and copy the segment classes onto `RadioGroup` and `RadioGroupItem`: track, 28 px items, the selected `bg-card shadow-sm` with our selected edge, and a semibold label. Keep it segmented on mobile too (three short options fit at 320). **Language options** carry `lang` (`SelectItem lang="de"`, and `<option lang="de">` in the native select), and the trigger’s value text gets the selected `lang`. Checked switch track `--ra-fill` |
| 14 Avatar, badge | `avatar` (`AvatarImage`, `AvatarFallback`), `badge variant="secondary"` | `AvatarFallback className="bg-[--ra-avatar-fallback] bg-none"` (flat) |
| 15 Emails | Own `EmailText` component (from the identity chip), with `variant="truncate"` (sidebar, index, chip) or `variant="wrap"` (rows, dialogs) | It renders the full address once as `sr-only` text and the visual halves `aria-hidden`, and truncates the local part with `text-overflow: ellipsis`, never a pseudo-element. The domain is never truncated; the truncate variant switches to two lines when the domain can’t fit (§4.0) |
| 16 Status slot | Own `StatusSlot` (one grid cell: a `role="status"` line plus an optional `role="alert"` line; `reserve="section"` sizes by container width, 20 / 40 / 60, and `reserve="fixed"` is 40) | suiss has no persistent save-feedback component (catalog §5). Inline actions (Undo) render after the status paragraph |
| 17 Alert | `alert` (`Alert`, `AlertTitle`, `AlertDescription`, `AlertAction`) | Move `AlertAction` below the description (className) |
| 18 Dialog | `modal` pattern (Dialog above 768, Drawer below) | Title `text-lg`. Footer buttons `size="lg"`. No close button (`showCloseButton={false}`). Wrap the body and footer in a `<form>`. Pass `dismissible={false}`, so an outside click doesn’t close it; `Escape` still does. Handle the keyboard with `visualViewport` (§7.2 row 18). Multi-step dialogs measure their tallest state and set `min-height`. The step-up, change email, rename, photo crop and delete dialogs all use it |
| 19 Confirm | `confirm` pattern (`AlertDialog` above 768, `AlertSheet` below; `dismissible` false) | Remove passkey, sign out of other devices, remove access, discard changes. Delete account uses `modal` with `Dialog` instead (typed field; suiss guide §4.5). `AlertDialogAction` closes immediately, so async actions use a plain `Button` inside `ConfirmFooter` (suiss gotcha 4) |
| 20 Code entry | `input-otp` (`InputOTP maxLength={8} pattern={CROCKFORD}`, two `InputOTPGroup`s of 4, `InputOTPSeparator`). Block: `verify-code` | Slot width `min(44px, …)` via className. Add the `border-input` edge. Normalise I and L to 1 and O to 0 in `onChange`. Reuse identity Verify copy and timers, and share one `useCodeTimer` hook that starts at 1:00 from the server’s next-allowed send time. Locked state: `readOnly` plus `aria-disabled`, never `disabled`; move focus to the error message |
| 21 Toast | `toast` (`Toaster` once at the root; `toast.add({ title, description, type:"success" })`) | No `actionProps`: toasts carry no actions (Undo is inline). Keep Base UI’s pause on hover and focus; timeout 5000 ms |
| 22 Skeleton | `skeleton` | Shapes per §4.0 and §4.3 loading. `aria-busy` on each loading group, with the loading status outside them |
| 23 Empty | `empty` (`Empty className="bg-muted"`, `EmptyMedia variant="icon"`, `EmptyTitle`, `EmptyDescription`) | Connected apps only. Other empty lists use one row of Slate text (the settings-block convention) |
| 24–25 Preview, messenger | `message` (`Message align`, `MessageAvatar`, `MessageHeader`, `MessageFooter`, `MessageContent`) and `bubble` (`Bubble variant`, `BubbleContent`) inside a custom `MessengerPreview`; `avatar`. Block reference: `chat-support` (panel radius 22, header and composer layout) | Customer bubble: `Bubble variant="default"` with the `[--primary:var(--ra-fill)]` override on the wrapper (keeps `#0071e3` in dark). Teammate bubble: `variant="muted"`, with `--ra-msgr-bubble` in dark. Props: `name` (first name) and `photoUrl` only; the component knows nothing about workspaces *(unverified: bubble tail rendering)* |
| 26 Photo crop | `modal` and `slider`; cropping logic from a small library (for example `react-easy-crop`; not suiss) | The crop area gets the §4.2 role and key model (`role="application"`, `aria-roledescription`, arrows, +/−, Home, debounced position status), so check that the library’s keyboard handling matches or wrap it. Output a 512 × 512 WebP, upload, then update the avatar URL everywhere |
| 27 Mobile index | `item` link rows (`settings-device` block, minus the search field and colour tiles) | Route `/account` below 768 only |
| 28 Solo page | Identity layout components (`rc-main`, `rc-col`, the mark, the account chip, the Verify code entry) in `apps/identity` | Reuses identity’s existing page frame for `Data-deleted`, `Deletion-pending` and the `/export` pages |
| Deletion lifecycle | Identity backend | **Day 0:** revoke every session and refresh token and every grant, then mark the user `pending_deletion` with `delete_at` set to day 30. **Sign-in** while pending: after a successful code or passkey, redirect to `/signin/deletion-pending`. "Keep my account" clears the flag (grants stay revoked). **Day 30 job:** delete the user and its credentials, free the email, and keep a frozen account-data export until day 60. **Export:** a signed, single-purpose token from the day-30 email, reusable until day 60, with a fresh email-code check (the same Duende OTP rules) on every use. `/export` without a token looks the address up among accounts deleted in the last 30 days. Each new request replaces the previous download link. Download links are valid for 7 days and never past day 60. **Identity sign-in** (identity canvas handoff): for an address deleted in the last 30 days, show `Signin-deleted` instead of sending a code. **Keep my account** resumes the sign-in’s return URL. **Day 60 job:** purge everything. Send the §4.8 emails at each step |
| 31 Canvas mechanics | `.ra-steps` / `.ra-step-ghost` / `.ra-step-busy`: a small `StackedStates` wrapper (CSS grid, one cell) that renders every state, hides the inactive ones (`visibility:hidden`, `inert`, `aria-hidden`), and is used inside `DialogContent` and inside `Button` for changing labels. `.ra-dialog-form`: the `<form>` in §8 row 18. `.ra-dialog-pane`: a `div` per step. `.ra-form-alert`, `.ra-photo-slot`: `StatusSlot reserve="fixed"`. `.ra-photo-row`, `.ra-photo-actions`, `.ra-photo-status`: the layout of a `ProfilePhotoField` component (avatar, buttons, slot, inline Undo). `.ra-skel-*`: `Skeleton` with sizes. `.ra-trunc`: Tailwind `truncate`. `.ra-bar`: the `trail` back bar. `.ra-inline`: `flex flex-wrap items-center gap-2` | These exist so the static boards don’t move between states. In the app, measure or stack the same way rather than hardcoding heights |
| Shell skip link | A plain `<a href="#page-title" className="sr-only focus:not-sr-only …">Skip to content</a>` as the first child of the account layout | It focuses the `h1` on activation |
| Connected app logos | `ItemMedia variant="image"` with the client’s `LogoUri` (from the Duende client store, for the grant’s client) | Fall back to the letter tile when it is missing or fails to load |
| Confirm → step-up | Close the `Confirm`, then open the step-up `modal` with the action’s description | Keep the trigger element to restore focus on cancel |
| Theme | `@esuiss/theme` (ThemeProvider and ThemeScript) for System, Light and Dark, persisted on the account and mirrored in `localStorage` for the no-flash script | It sets `.dark` on `<html>` and listens to `matchMedia('(prefers-color-scheme: dark)')` when the preference is System |
| Step-up | Duende: re-authenticate with `prompt=login` / `max_age=600` in a popup, or an in-app endpoint that issues a fresh `auth_time`. The dialog UI is ours | Server-side check on every gated endpoint (`auth_time` ≤ 10 min). The UI gate is only a convenience |

---

## 9. Open product questions

Each has a recommended default; design proceeds on the default.

| # | Question | Recommended default |
|---|---|---|
| Q1 | Which name do customers see on your replies: first name, full name, or a separate display name? | **First name**, with no extra field. The Profile hint says so, and the preview shows it. Add a "Name customers see" field later only if teams ask |
| Q2 | How long does a step-up last? | **10 minutes** after your last sign-in or step-up, per device |
| Q3 | **Legal check (resolves the old "can it be restored?"; the lifecycle in §4.8 is decided):** is keeping account data for up to 60 days after a deletion request disclosed and lawful under GDPR’s right to erasure? | **Proceed on the lifecycle as specified, pending legal review.** The privacy policy must state the 30-day grace period, the 30-day export window and the purge on day 60. **The fallback, if legal prefers it:** at day 30, build the export archive and delete the live data at once, keeping only that prepared archive (encrypted, used for nothing else) until day 60 |
| Q4 | Changing to an email that already has a Richie account: say so, or stay enumeration-safe? | **Say so.** The user is signed in and stepped up, and sends are rate-limited, so the enumeration risk is low and the alternative is a dead end |
| Q5 | Is last name required? | **Optional** (people with one name). Sign-up should match; flag to the identity owner |
| Q6 | Is Appearance stored per account or per device? | **Per account**, applied on every device. It matches the "follows you" lede |
| Q7 | **Lost mailbox** (product and security): how does someone who can’t receive email codes, and has no passkey, change their email or get back in? | **Support-assisted recovery** (working assumption). Step-up screen C sends them to support. Support verifies their identity under a written procedure, then changes the email. Both the old and the new address are notified. The account is marked *recently recovered*: a security-activity entry, and step-up is required again for every sensitive action for 7 days. Security’s empty passkeys row already suggests it: "Add a passkey so you can still sign in if you lose access to your email." |
| Q8 | **Signed-out download: existence and rate limits.** `Export-start` and `Signin-deleted` reveal whether an address had an account deleted in the last 30 days. What limits apply? | **Accept the trade-off** for the 30-day window, because data still needs an email code. Limit code requests to **5 per address per hour and 20 per IP address per hour**, shown with the countdown button (§4.8) |

---

## 10. Appendix

### 10.1 Sample data

- **User:**
  - Jamie Rivera, `jamie.rivera@northwind.coffee`, initials JR, photo stand-in (§10.2).
  - Lives in Portland, Oregon, on Pacific Time. **Today is Thursday, November 5, 2026.** That is day 0 for the deletion sample: deleted on December 5, data available until January 4, 2027. The export boards (Row 8) are drawn on December 9, 2026.
- **No workspace data** appears anywhere in this area.
- **Passkeys:**
  - "MacBook Pro": iCloud Keychain, added March 12, last used today.
  - "Pixel 8": Google Password Manager, added October 28, last used October 30.
- **Sessions:**
  - Chrome on macOS, Portland, Oregon, active now (this device).
  - Safari on iPhone, Portland, Oregon, 2 hours ago.
  - Firefox on Windows, Seattle, Washington, last active October 27.
- **Connected apps:** Tallybird and Slotwise (§4.6). Both names are invented, not real brands.
- **Download:** none requested. The ready state is 2.4 MB, available until November 11.
- **Deletion and export (Row 8):** the export link email is dated December 9, and the download link works until December 16.
- **Change-email target:** `jamie@rivera.studio`.
- **Extreme user** (`Profile-extreme`):
  - First name "Maximiliana-Alexandra", last name "Featherstonehaugh-Okonkwo", initials MF, no photo.
  - Email `maximiliana.featherstonehaugh-okonkwo@northwind-specialty-coffee-roasters.co.uk`.

### 10.2 Photo stand-in (inline SVG, used for Jamie’s photo everywhere)

An illustrated portrait, so no real person’s photo is used. Scale it with `width`/`height`; it is `aria-hidden="true"` inside avatars, because the name is adjacent.

```html
<svg viewBox="0 0 64 64" width="64" height="64" aria-hidden="true" focusable="false">
  <rect width="64" height="64" fill="#c9b8a6"/>
  <path d="M8 64c2-14 12-20 24-20s22 6 24 20z" fill="#3d5a6c"/>
  <rect x="27" y="36" width="10" height="10" rx="4" fill="#b98463"/>
  <ellipse cx="32" cy="28" rx="11" ry="13" fill="#c8916c"/>
  <path d="M20 27c0-10 6-15 12-15s13 4 12 15c-2-5-6-8-12-8s-10 3-12 8z" fill="#2b2119"/>
</svg>
```

### 10.3 Icons (Phosphor regular, all checked at `@phosphor-icons/core@2.1.1`)

- **Nav and pages:** `user-circle`, `key`, `sliders-horizontal`, `plugs-connected`, `shield-check`. (`buildings` is no longer used.)
- **Shell:** `arrow-left`, `caret-right`, `question`, `sign-out`.
- **Rows:** `envelope-simple`, `laptop`, `device-mobile`, `desktop`, `sign-in`, `plus`, `download-simple`.
- **Feedback:** `check-circle`, `warning-circle`, `info`, `wifi-slash`, `x`.
- **Preview:** `paper-plane-right`.
- **Spinner:** draw the suiss 8-spoke spinner (8 rounded lines, `stroke-width` 2, opacities stepping from 1 down to .2) rather than Phosphor `spinner-gap`, so it matches the component.

---

## 11. Changelog: internal review IDs

The body of this spec carries no review IDs. This table maps them to where each change now lives, for anyone tracing a decision.

**Sources:**

- Phase 4 QA: `qa/qa-a.md` (A-), `qa/qa-b.md` (B-) and `qa/qa-c.md` (C-), plus the canvas notes.
- The regression re-test: `qa/qa-regression.md` (R-).
- The final check: `qa/qa-final-check.md` (N-).
- The evaluation: `evaluation.md`, round 1 (SH-, PR-, SE-, ST-, PF-, CA-, DP-, DL-, X-) and round 2 (N2-).

| IDs | What changed | Where |
|---|---|---|
| A-01, A-02, B-01, B-04, B-13, R-22 | Reserved space: the name action row and its alert slot; status slots on their own row, sized by column width; code meta and step-up description lines at narrow widths; every status fits one line at 480 px | §1.5, §4.0, §4.2, §4.4 |
| ST-2, PR-1, PR-4, SE-2, PF-1 | Why reserved space looks empty; change-email step 1 copy | §1.5, §4.2 |
| PR-6, PR-3, A-14, R-06 | The two exceptions to rule 8: field errors, and the crop frame floor | §1.5, §4.2 |
| A-03, R-04 | Multi-step dialogs keep one height; commit min-width | §4.0 |
| A-04, C-02, R-21 | Workspace-neutral preview copy | §4.2 |
| PR-2, N2-02 | Phone order and the compact preview | §1.4, §4.2, §7.2 |
| A-05, C-30 | State boards show their change ("Jay"); one activity row on the no-passkeys board | §4.2, §4.3 |
| A-06, C-10, R-05, SH-2 | Long emails: one accessible string, the domain never truncated, two-line sidebar, `text-overflow` | §4.0, §7.2, §8 |
| A-07, SE-1 | Skeleton shapes, including the action line below 480 | §4.3, §7.2 |
| A-08 | Photo buttons stack, with their height reserved | §4.2 |
| A-09 | Trailing plain button alignment | §4.3, §7.2 |
| A-10, R-07 | Two columns from a 620 px column; the form shrinks first | §4.2, §7.2 |
| A-11, B-11, R-23, DP-1, N-01 | Equal-width footers for confirms only; the delete dialog is a form dialog | §1.5, §4.7, §7.2 |
| A-12, B-07 | Dialog body bottom padding | §7.2 |
| A-13, B-06 | 44 px hit areas for the slider, segments and picker | §7.3 |
| A-15 | Messenger metrics | §7.2 |
| A-16 | Sidebar rules drawn as borders | §3.2, §7.2, §7.7 |
| A-17, B-14, C-26 | Workspace leftovers removed | §10.3 |
| B-02, C-07, R-02, R-03, R-17, ST-3, N2-05 | Focus targets: the locked error message, after removals, programmatic targets without a ring, one rule per step-up screen | §4.0, §4.4, §4.6, §7.4 |
| B-03, C-01, DL-1 to DL-5, N2-01, N2-06, N2-07, N2-08 | Deletion lifecycle, export links, `Export-start`, `Signin-deleted`, expired pages, return URL, small shiba, a live account wins, rate limit | §3.1, §4.7, §4.8, §9 |
| B-05, C-03, DP-2 | Download description | §4.7 |
| B-08 | Dialog scroll cue | §4.0, §7.2 |
| B-09 | Balanced notes | §4.4 |
| B-10, C-25 | Preferences: status slot before the note | §4.5 |
| B-12 | Countdowns start at 1:00 | §4.0, §4.4 |
| C-04, R-11, R-12 | Accessible names start with the visible label | §4.0, §4.1, §4.3 |
| C-05, R-14 | Crop area role and keys | §4.2 |
| C-06 | Polite status on the change-email code step | §4.2 |
| C-08 | Failure, offline and loading copy | §4.0 |
| C-09, R-20, PF-3 | Segmented radio groups (`fieldset` + `legend`); one section pattern | §4.0, §4.5, §7.2, §8 |
| C-11, R-23, N-02, X-1 | Canvas reconciliations, mechanics and cosmetic classes, one source of truth | §7.2, §8 |
| C-12 | No `ItemSeparator` | §8 |
| C-13 | Field errors without `role="alert"` when focus moves to the field | §4.0, §7.2, §8 |
| C-14 | Loading status outside the busy groups | §4.0, §4.3 |
| C-15 | Spinners inside buttons are `aria-hidden` | §8 |
| C-16, R-15 | Every dialog is a `<form>` | §4.0 |
| C-17, R-18 | One `nav` landmark | §3.2, §4.1 |
| C-18 | Phrasing content in button rows | §4.1 |
| C-19 | Board titles, not document titles | §6.1 |
| C-20 | Forced-colours polish | §7.7 |
| C-21 | Date rule applied to the spec’s copy | §4.0, §4.3, §4.6, §4.7 |
| C-22, R-01 | "Email code" as the one name | §4.0 |
| C-23 | "Password" in provider names; `ghost` for the toast Dismiss button | §1.6, §4.0 |
| C-24 | "Last name (optional)" | §4.2 |
| C-27, R-08, R-10 | Board tables from `canvas.json` | §6.2 |
| C-28 | `lang` on language options | §4.5, §8 |
| C-29, C-31, R-13 | Inline Undo for photo removal; "Photo saved." in the photo slot | §4.2 |
| C-32 | Dialog details: no close button, initial focus | §4.0, §7.4 |
| R-09 | `Profile-photo-removed`; boards with their own sidebar copy | §5, §6 |
| R-16 | Code meta reservation on the signed-out code page | §4.4, §4.8 |
| R-19 | No status slot under Delete account | §4.7 |
| ST-1, N2-03 | "Can’t get the code?" (screen C); the passkey option only with a passkey | §4.4, §9 Q7 |
| ST-4 | Drawers and the on-screen keyboard | §4.0, §7.2 |
| SE-3 | The hand-off from a confirm to step-up | §4.0, §4.3, §8 |
| SE-4, SE-5, N2-04 | Passkey failure copy, unknown-device advice, passkey nudge | §4.3, §9 Q7 |
| SH-1 | Skip link | §3.2, §4.1, §7.2, §8 |
| PF-2 | Behaviour after a language change | §4.5 |
| DP-3, DP-4, DP-5 | Consequence copy, typed-phrase matching, field scrolled into view | §4.7 |
| CA-1 | Registered app logos | §4.6, §8 |
| X-2 | `dismissible={false}` | §8 |
