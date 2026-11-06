# QA-C: account settings, cross-board code and content review

Tester C (independent; did not design the boards). 2026-11-05. Scope: the 42 built boards of the **Richie Account Settings** canvas. `Data-delete-confirm` and `Data-deleted` are on hold and are not reviewed.

**Inputs**
- Sources: `.design/canvas/project/*.dc.html`, `account.css`, `canvas.json`.
- Built pages: `.design/qa-out/consolidated/`. A fresh rebuild with `qa-rig/build.mjs` is byte-identical and gives 0 warnings.
- Requirements: spec.md (§3–§8), how-we-design §5–§6, suiss-ui-guide §5.

**Tooling**
- Script: `.design/qa-c/audit.mjs`, with Playwright and axe-core from `qa-rig`. Raw output is in `.design/qa-c/out/audit.json`, with per-board ARIA snapshots in `.design/qa-c/out/aria/`.
- Contrast re-check: `.design/qa-c/recheck.mjs`.
- The static server (port 9431/9432) and the browser were shut down after each run.

**Path shorthand in this report:** `P/` = `.design/canvas/project/`, `S/` = `.design/qa-c/out/shots/forced/`.

## Counts

| Severity | Count |
|---|---|
| Blocker | 1 |
| Major | 11 |
| Minor | 20 |

## Findings

| ID | Board(s) | Sev | What’s wrong | Expected (§) | Evidence | Fix |
|---|---|---|---|---|---|---|
| C-01 | Data, Data-dark, Data-mobile, Data-ready | Blocker | The "Delete your account" description reads "We’ll delete your account 30 days after you ask. Until then, you can sign in to cancel." That promises a grace period with cancel-by-sign-in. The spec says sign-in stops at once and deletion can’t be undone. This probably anticipates the pending deletion-lifecycle decision (canvas NOTES), but the base board now contradicts the spec, and a developer can’t tell which is right. | §4.7: "Deleting your account signs you out everywhere and removes your profile, passkeys and connected apps." §9 Q3: no restore | `P/Data.dc.html:55`, `P/Data-ready.dc.html:55` | Revert to the §4.7 copy until the decision lands, or update §4.7, §9 Q3 and this board together. |
| C-02 | Main, Profile-dark, -mobile, -name-saved, -name-error(-dark), -save-failed, -extreme, plus the 3 Profile overlays | Major | The preview conversation isn’t the spec copy. The customer says "Hi! Do you ship whole beans to Canada?" and the reply is "We do! Orders to Canada usually arrive in 4 to 6 business days." It describes a coffee merchant, so it isn’t workspace-neutral. "We do!" is an exclamation mark in the teammate’s reply, which only customer messages may use. The sr-only paragraph also drifts: "a customer’s conversation in the messenger" vs "a customer conversation in a Richie messenger". | §4.2 preview table: "Hi! Can I change the delivery address on my order?" / "Of course. Send me the new address and I’ll update it right away."; revision note (neutral preview); §4.0 (exclamation only in customer messages) | `P/Main.dc.html:87,100,113`; `P/Profile-extreme.dc.html:149,160,173` | Use the §4.2 strings and the §4.2 sr sentence. |
| C-03 | Data, Data-dark, Data-mobile, Data-ready | Major | The download description drops "passkeys, sessions" and the sentence "It contains account data only." The revision made that sentence the scope statement. | §4.7: "Your profile, passkeys, sessions, sign-in history, preferences and connected apps, as JSON files in a .zip. It contains account data only." | `P/Data.dc.html:34`, `P/Data-ready.dc.html:34` | Use the §4.7 copy. |
| C-04 | Security, Security-dark, -mobile, -no-passkeys, -passkey-waiting, -passkey-failed | Major | Label-in-name failure (WCAG 2.5.3, level A). The visible label is "Change email", but the accessible name is "Change the email that gets your codes": the hidden " the" splits the visible phrase. Voice-control users saying "click Change email" can miss it. The spec dictates this sr string, so developers will copy it. | §4.3 sr string; WCAG 2.5.3 | `P/Security.dc.html:43`; `out/aria/Security.yml` (`link "Change the email that gets your codes"`) | sr text "Change email, the one that gets your codes", or keep "Change email" as the name and point `aria-describedby` at the row title. Update §4.3. |
| C-05 | Profile-photo-crop | Major | The crop area is a focusable `div` with `role="img"`. An image role says "static" and gives no operable semantics. Screen-reader browse mode swallows the arrow keys, so the "Use the arrow keys" instruction fails for SR users. The spec gives only the sr string, not the role or key model. | §4.2 crop row; §8 row 26 | `P/Profile-photo-crop.dc.html:24` | Spec a pattern: `role="application"` with `aria-roledescription="photo position"` plus live position text, or two labelled position sliders, or nudge buttons. Add it to §4.2 and §8. |
| C-06 | Profile-email-code | Major | The change-email code step has no polite status region. Step-up screen B has one (`<p class="ra-sr" role="status">`) for "Checking your code…", "You can send a new code now." and "We sent a new code." The same OTP pattern is announced on one board and silent on the other, and a developer copying this board ships no announcements. | §4.2 step 2 "Errors as in §4.4"; §4.4 meta line 2, busy and new-code rows | `P/Profile-email-code.dc.html:33-37` vs `P/Stepup-code.dc.html:37` | Add the same sr status element after `.ra-code-meta`. |
| C-07 | Stepup-code-locked, Connected, Connected-remove, Connected-empty, Main | Major | Focus has nowhere defined to go in 3 flows. (a) Locked: the focused code input becomes `disabled`, so focus drops to `body`; in the keyboard pass the first Tab lands on "Use a passkey instead". (b) Connected apps: the removal rule says "focus to the section’s h2", but this page has no `h2`. (c) Remove photo: the focused button is removed from the DOM (replaced by "Upload photo"). | §4.0 (row removed → h2); §4.4 locked row; §4.6 "no h2"; §7.4 | `P/Stepup-code-locked.dc.html:26`; `audit.json → keyboard.Stepup-code-locked`; `P/Connected.dc.html:31`; `P/Main.dc.html:45` | Spec the targets: locked → the `aria-disabled` "Send a new code in 0:41" button (it stays focusable); Connected → `h1` (or the Empty title, `tabindex=-1`); Remove photo → "Upload photo". |
| C-08 | Security, Connected, Data, Profile, Preferences | Major | Several §5 cells say "copy", but §4 has no copy for them, so developers will invent it. **Server error:** sign out one device or the others, rename (server), remove passkey after step-up, add-passkey server rejection, remove access, request download. **Offline:** Profile (has an explicit Save), Security, Connected, Data. **Initial loading:** Preferences, Connected, Data (no sr text or skeleton shape; §4.3 covers Security only). | §5 rows Server error, Offline, Initial loading; how-we-design §6 checklist | spec §5 rows vs §4.2–§4.7 (absent) | Add one generic action-failure line ("We couldn’t … Check your connection and try again.") with its slot and role per action, plus the loading sr strings, to §4. |
| C-09 | Preferences (+dark, mobile, offline) | Major | Handoff mismatch. §4.5 and the boards build Appearance and Time format as a radio group (real radios in a `fieldset`). §8 row 13 maps them to suiss ToggleGroup / `segment-picker`, whose Base UI items are toggle buttons (`aria-pressed`) that can be deselected to no value. A developer following §8 ships different semantics, and Appearance can end up empty. | §4.5 ("radiogroup labelled by the row title"); §8 row 13 | `P/Preferences.dc.html:42,91`; spec.md:1221 | Map to `radio-group` styled as segmented, or require a controlled ToggleGroup with radio semantics and no deselect. Update §8. |
| C-10 | Every page with the sidebar, Account-index(-dark), Profile and Security email rows, Profile-extreme | Major | Both email recipes split the address in the accessibility tree. `.ra-email` flex halves expose "jamie.rivera @northwind.coffee"; `.ra-email-wrap` inline-block halves expose "jamie.rivera@ northwind.coffee". Braille shows two words, and the index link name becomes "Profile: Jamie Rivera jamie.rivera @northwind.coffee". In Profile-extreme the generated "…" (`.ra-email::after`) is also exposed. | §4.0 (screen-reader rules); §7.2 row 15 ("full text stays in the DOM"); §8 row 15 | `out/aria/Main.yml:5,51`, `out/aria/Account-index.yml:7`, `out/aria/Profile-extreme.yml:5`; `P/account.css:1270,1308,1337` | Render the full address once as `.ra-sr` and `aria-hidden` the visual halves (or use `<wbr>` without inline-block). Use `content: "\2026" / ""`. Specify this in §8 `EmailText`. |
| C-11 | All (handoff) | Major | `account.css` and spec §7.2 disagree, and the reconciliations exist only in the local, unpublished `.design/canvas/NOTES.md` §5. Examples: row padding 12 (spec) vs 14 (CSS); focus halo "3 px" vs a 5 px spread; Profile email row "local part truncates first" vs wraps after the @; segmented as `div role=radiogroup` (NOTES template) vs `fieldset` (boards); dark teammate bubble "white 10 %" vs opaque `#343435`; confirm footer "2-column grid" vs flex wrap-reverse. In addition, 51 classes used on the boards appear nowhere in the spec, including behaviour-bearing ones: `.ra-slot` (shared status/alert cell), `.keep-inline`, `.is-quiet` (static Slate line, not live), `.ra-page-alert`, `.ra-email-wrap`, `.ra-num`. | §7.2 rows 8, 12, 13, 15, 19, 25; §7.4; §8 | `P/account.css:647` (`padding: 14px 16px`), `:2399` (`0 0 0 5px`), `:150`; NOTES.md §5 items 3, 8, 10–12 | Back-port the NOTES §5 decisions into §7.2 and §8, and list the behaviour-bearing utility classes with their component mapping. |
| C-12 | All boards with inset groups (handoff) | Major | §8 rows 7–9 tell developers to use `ItemSeparator className="ms-[62px]"` inside `ItemGroup` (a list). §7.2 row 7 deliberately draws separators in CSS "so the list markup stays valid". Following §8 puts non-`listitem` children inside `role=list` (axe `aria-required-children` / `list`). | §7.2 row 7 vs §8 rows 7–9 | spec.md:1127 vs spec.md:1218 | Drop `ItemSeparator` from §8, or render it outside the list semantics. |
| C-13 | Profile-name-error(-dark) | Minor | Double announcement. The error has `role="alert"`, and focus also moves to the field, whose `aria-describedby` starts with that error, so SRs speak it twice. | §4.2 ("Focus moves to the field"); §7.2 row 12 (`role=alert`) | `P/Profile-name-error.dc.html:57-58` | When focus moves to the field, render the error without `role=alert` and rely on `describedby`. State this in §7.2 row 12 and §8 row 12. |
| C-14 | Security-loading | Minor | The sr-only `role="status"` "Loading your sign-in settings…" sits inside `main[aria-busy=true]`. Some AT defer or suppress updates inside a busy subtree. | §4.3 loading | `P/Security-loading.dc.html:17,30` | Put the status outside `main`, or set `aria-busy` on the groups instead. |
| C-15 | Security-passkey-waiting (handoff) | Minor | The boards make the busy spinner `aria-hidden`. §8 says `<Spinner data-icon="inline-start"/>`, and suiss Spinner is `role=status`, so a live region ends up nested inside the button. | §8 rows 10–11; suiss guide §5 | spec.md:1219; suiss-ui-guide.md:260,508 | Add "pass `aria-hidden` (no `role`) inside buttons" to §8. |
| C-16 | Profile-email-change, Profile-email-code, Stepup-code(-error, -mobile, -error-dark) | Minor | `type="submit"` buttons with no `<form>`. Enter does nothing, although the inputs say `enterkeyhint="go"`. | §4.4 code row (Enter to submit; no auto-submit) | `P/Profile-email-change.dc.html:31`, `P/Profile-email-code.dc.html:41`, `P/Stepup-code.dc.html:42` | Wrap the dialog body and footer in a `<form>`, as Main does. |
| C-17 | All sidebar pages; Account-index(-dark) | Minor | The primary navigation sits inside `<aside aria-label="Your account">` (complementary). Landmark lists then read "Your account, complementary" and then "Account, navigation". The mobile index, which *is* the navigation below 768, has no `nav` landmark. | §4.1; §3.2 | `P/Account-sidebar.dc.html:16`; `P/Account-index.dc.html:21-43` | Use a plain `div` container (or label the aside distinctly), and wrap index groups 2–3 in `<nav aria-label="Account">`. |
| C-18 | Account-index(-dark) | Minor | Invalid content model: the Sign out `<button>` contains a `<div>` and a `<p>` (buttons take phrasing content only). | HTML; §7.2 row 8 (button rows) | `P/Account-index.dc.html:42` | Use `span`s inside button rows. |
| C-19 | Profile-name-error | Minor | Its `<title>` is the document title "Error: Profile – Richie". The other 41 boards, including its own dark wrapper, use board titles. | §6.1 template (board title) vs §3.1 (document titles) | `P/Profile-name-error.dc.html:5` | Use "Profile, first name missing". The "Error: " prefix is already specified in §3.1 and §4.2. |
| C-20 | Stepup-code-locked, Security, Connected-remove(-mobile), Main | Minor | Forced-colors polish. (a) The unavailable "Send a new code in 0:41" (`aria-disabled`) is drawn exactly like an enabled button. (b) The "This device" badge’s transparent border becomes visible and clips the label. (c) On mobile, the `alert-dialog-content` outline draws a full-width line above the sheet. (d) The bubble tails break the bubble borders. | §7.7 (no rule for unavailable; badge outline dropped per NOTES) | `S/Stepup-code-locked-1440x900.png`, `S/Security-1440x900.png`, `S/Connected-remove-390x844.png`, `S/Main-1440x900.png` | (a) `color: GrayText; border-color: GrayText` for `[aria-disabled]:not([data-busy])`; (b) give the badge padding or `border: 0` in forced colors; (c) outline only the visible pieces; (d) hide the tail or keep the border continuous. |
| C-21 | Security(*), Connected(*), Data-ready | Minor | The spec’s own copy breaks the §4.0 date rule, and the boards follow the copy. Current-year dates carry the year: "Added March 12, 2026", "Connected August 14, 2026", "September 2, 2026", "Available until November 11, 2026". The same list mixes in "Added October 28". "Last active 9 days ago" exceeds the 7-day limit for relative times. | §4.0 dates vs §4.3, §4.6, §4.7 tables | `P/Security.dc.html:60,117`; `P/Connected.dc.html:37,48` | Pick one rule: "Added March 12", "Last active October 26", and so on, or relax §4.0. |
| C-22 | Cross-board | Minor | One thing, several names. The codes are called "Email codes" (h2), "sign-in codes" (Profile email row, Preferences), "a code from your email" and "Verification code" (sr). Sign out of other devices: the confirm asks "Sign out of 2 other devices?", but the toast says "Signed out of 2 devices". | §4.0 (consistent action names); §4.3 | strings dump `out/strings.txt`; spec.md:618-621 | Standardise on "sign-in code" (h2 "Sign-in codes") or say why not. Toast: "Signed out of 2 other devices". |
| C-23 | Security(*), Connected-empty | Minor | Two spec self-conflicts. (1) §1.6.3 says "password" appears exactly once, but "Google Password Manager" appears in the passkey row and the removal toast. (2) §4.0 bans the `ghost` variant, but §7.2 row 21 uses ghost for the toast’s Dismiss. | §1.6.3, §4.0 vs §4.3, §7.2 row 21 | `P/Security.dc.html:71`; `P/Connected-empty.dc.html:51` | Amend §1.6.3 to exempt provider names, and list `ghost icon-sm` as the one allowed exception in §4.0. |
| C-24 | Main and all Profile copies | Minor | Last name is optional (Q5) and first name is required, but the labels give no cue. Sighted users find out only on error (WCAG 3.3.2). | §4.2; §9 Q5 | `P/Main.dc.html:60` | Label it "Last name (optional)" and add it to §4.2. |
| C-25 | Preferences(+dark, mobile, offline) | Minor | Magic number in markup: `<p class="ra-note" style="margin-top: 12px">`. This is the only inline style that isn’t a wrapper size or the slider’s `--ra-val`. | §7.2 row 6 (foot gap 12) | `P/Preferences.dc.html:144`, `P/Preferences-offline.dc.html:150` | Put the note inside `.ra-section-foot`, or add the margin to `.ra-note` after a group. |
| C-26 | account.css | Minor | Leftovers from the removed workspace scope: `[data-page=workspaces] [data-nav=workspaces]` selectors, the `.ra-count` invitation pill, and the comments "Back to the workspace" and "(workspace names)". | Revision note; §1.6.2 (identity only) | `P/account.css:374,458,694,1233,2672,2743` | Delete them. |
| C-27 | Spec §6.2 | Minor | The board sizes and row positions in the spec are stale for 17 boards, for example Main 1000 vs 900, Security 1700 vs 1500, Preferences 1100 vs 1200. Rows sit at y 0/1400/2700/4600/5900/7600/8900, not the y values in §6.2. The boards follow the round-up rule correctly (measured content heights all fit). | §6.2 | heights table below | Update the §6.2 tables from `canvas.json`. |
| C-28 | Preferences (handoff) | Minor | The language options Deutsch, Español, Français, Português (Brasil) and ქართული need `lang` attributes so SRs pronounce them correctly (WCAG 3.1.2). Neither §4.5 nor §8 mentions it. | §4.5 language row | spec.md:711 | Add `lang`/`hreflang` per option to §4.5 and §8. |
| C-29 | Main (copy-only state) | Minor | "Photo removed" plus Undo is a timed toast (8 s), and it is the only one-step recovery. The spec says nothing about pausing on hover or focus, or a keyboard route to the toast region (WCAG 2.2.1). | §4.0 toasts; §4.2 Remove photo; §8 row 21 | spec.md:461, 1229 | Use `timeout: 0` for toasts with an action, or specify pause-on-hover/focus plus F6 to the "Notifications" region. |
| C-30 | Profile-name-saved, Profile-save-failed, Security-no-passkeys | Minor | The states don’t show their change. Saved and failed both show the already-saved "Jamie", so neither the dirty value nor the sidebar update is visible. No-passkeys also empties Recent activity, although this device is "Active now" and so signed in within 30 days. | §4.2 name states; §4.3 | `P/Profile-save-failed.dc.html` (value="Jamie"); `P/Security-no-passkeys.dc.html:98` | Show an edited name (for example "Jay") in both, and keep one "Signed in with an email code" activity row. |
| C-31 | Main (handoff) | Minor | §4.2 says "the status line reads 'Photo saved.'", but Section A’s only status line is in the name action row under the fields, away from the photo (principle 7: confirm next to what changed). A developer has to guess which slot. | §4.2 crop "After saving"; §1.6.7 | `P/Main.dc.html:65-66` | Name the slot, either the photo hint slot or a photo status line. |
| C-32 | Profile-photo-crop, Stepup (handoff) | Minor | Dialog details are unspecified. The crop dialog has no initial-focus target. Step-up screen A marks "Use a passkey", but §7.4 lists only confirm, screen B and delete. §7.2 row 18 lists `dialog-close`, yet no board or copy says whether dialogs have an × button. | §7.4; §7.2 row 18 | `P/Profile-photo-crop.dc.html` (no `.is-focused`); `P/Stepup.dc.html` | Add initial focus for crop (the crop area) and screen A to §7.4, and state the close-button rule. |

(*) means the base board plus its wrappers and full copies.

## Checked and passing

**Ids and references**
- 0 duplicate `id`s on any of the 42 pages after imports are inlined.
- 0 broken `aria-labelledby`, `aria-describedby` or `for` references.
- `lang="en"` on all pages.

**Headings and landmarks**
- Every non-overlay page has one `h1` (`tabindex=-1`) and no heading skips.
- On the 12 overlay boards the page sits in `<div inert>` and only the dialog `h2` is exposed.
- One `main` per page.
- `nav aria-label="Account"` is present, and it is hidden together with the sidebar below 768. At 390 px only `main` is exposed and the first stop is "Account".

**Controls and names**
- All controls are real `<a href>`, `<button>` or `<input>` elements with labels.
- Row buttons have specific names ("Remove the Pixel 8 passkey", "Sign out of Safari on iPhone", "Remove access for Tallybird").
- The only icon-only button, "Dismiss", is named.
- 0 SVGs lack `aria-hidden`.
- Avatars and initials tiles are `aria-hidden` (the inline-SVG equivalent of `alt=""`).

**Dialogs**
- `role=dialog` or `alertdialog`, `aria-modal`, `aria-labelledby` and `aria-describedby` are set on all of them.
- Tab stays inside every dialog (keyboard pass, 11 overlay boards).
- Initial focus is marked on Cancel for confirms and on the code slots for step-up B.

**Fields**
- First name: `given-name` / `words` / `done`.
- Last name: `family-name`.
- New email: `email` / `email` / `none` / `spellcheck=false` / `name@example.com`.

**Code entry**
- `one-time-code`, `inputmode=text`, `autocapitalize=characters`, `autocorrect=off`, `spellcheck=false`, `enterkeyhint=go`, and the sr label "Verification code, 8 letters and numbers".
- The sample `7KQ2M9XA` is valid Crockford Base32 (4 + 4).
- "Codes expire after 5 minutes", "3 tries left" and the 0:59 / 0:52 / 0:41 timers are right. "Send a new code" is the only resend verb.

**Copy**
- No "invalid", "oops", "Resend", "…" written as three dots, straight quotes, "›", "→", middle dots, ALL CAPS, UK spellings or `you@example.com`.

**`aria-current`**
- It is absent on the boards, as intended. §2.4 and §8 rows 2–3 tell implementers to set it.

## Automated a11y (axe-core 4.x)

**Coverage**
- 42 boards × 4 runs: light and dark (`.dark` on `<html>`, the §7.1 app mechanism, plus `colorScheme: dark`) at 1440×900 and 390×844. That is 168 runs, with tags wcag2a/aa, wcag21a/aa, wcag22aa and best-practice.
- Dark therefore covers *every* state, not just the drawn `-dark` boards.

**Results**
- **Critical:** 0.
- **Serious:** 5 colour-contrast hits, all transient. They are colour transitions caught mid-flight after the class toggle, for example `#0272e3` vs the `#0071e3` token. A re-run after 1.2 s, and under reduced motion, gives 0 (`recheck.mjs`).
- **Moderate:** only `landmark-one-main` and `page-has-heading-one` on the `Account-sidebar` part board, which is expected for a fragment.
- **Needs review:** contrast on 20 boards. The causes are pseudo-element backgrounds (tiles, segmented), text under the dialog overlay, and empty inputs. None reproduced as failures.
- **Forced colours:** 11 axe runs, 0 violations.

## Reduced motion (`prefers-reduced-motion: reduce`)

Probed boards: Main, Security, Stepup-code-error, Preferences and Connected-remove, plus Security-loading, Security-passkey-waiting, Stepup-code and Connected-empty for the motion they carry.

- **Transitions:** every one goes from 0.15 s / 0.2 s to 0 s (buttons, inputs, slots, switches, segmented, sidebar).
- **Skeleton:** the `ra-pulse` 2 s animation is removed.
- **Spinner:** 0.8 s becomes 2.4 s and keeps turning.
- **OTP caret:** the `ra-blink` 1 s animation is removed.
- This matches §7.6. Dialog, drawer and toast entrance motion can’t be observed on static boards.

## Forced colours (Chromium `forcedColors: active`, 1440×900 and 390×844)

**Distinguishable**

| What | Result | Screenshot |
|---|---|---|
| Focus | A 2 px Highlight outline on inputs, buttons, nav links, link rows, switches and segmented items | `S/Main-1440x900-focus-1.png`, `-focus-2.png`, `S/Security-1440x900-focus-1..3.png`, `S/Account-index-1440x900-focus-1.png`, `S/Preferences-1440x900-focus-1..4.png`, `S/Connected-remove-1440x900-focus-1.png` |
| Focus on the selected segment | The ring sits beside the Highlight fill and stays visible thanks to the 2 px offset | `S/Preferences-1440x900-focus-1.png` |
| Active nav item | Highlight fill, HighlightText, 600 weight | every shot |
| Selected segment | Highlight fill, HighlightText | `S/Preferences-1440x900.png` |
| Switches | On: Highlight track and HighlightText thumb. Off: ButtonFace with a 2 px ButtonText edge and a ButtonText thumb | `S/Preferences-1440x900.png`, `S/Preferences-1440x900-focus-2.png`, `-focus-3.png` |
| Checked states | Kept | `S/Preferences-offline-1440x900.png` |
| Errors | Invalid field gets a 2 px border; error lines keep the WarningCircle icon (red is lost, as expected) | `S/Profile-name-error-1440x900.png`, `-390x844.png` |
| Code slots | Invalid slots 2 px, the active slot gets a Highlight ring, disabled slots turn GrayText, and the empty focused slot shows the caret | `S/Stepup-code-error-1440x900.png`, `S/Stepup-code-error-1440x900-focus-1.png`, `S/Stepup-code-locked-1440x900.png`, `S/Profile-email-code-390x844.png` |
| Dialogs, sheets, groups, tiles, messenger | Keep their shapes through the transparent outlines; the scrim becomes a 50 % Canvas wash | `S/Connected-remove-*.png`, `S/Security-remove-passkey-*.png` |
| Preview | Header and bubbles lose the Signal fill but keep their borders, as §7.7 expects | `S/Main-1440x900.png` |

**Not right:** see C-20. The unavailable countdown button, the badge clip, the sheet outline line and the bubble-tail gaps.

## State coverage (spec §5 × how-we-design §6)

✓ means drawn and matching (board named), "copy ✓" means the copy is specified in §4, and **GAP** means neither drawn nor specified.

| State | Profile | Security | Step-up | Preferences | Connected | Data | Shell |
|---|---|---|---|---|---|---|---|
| Default | Main ✓ | Security ✓ | Stepup ✓ | Preferences ✓ | Connected ✓ | Data ✓ (delete copy: C-01) | Account-index, Account-sidebar ✓ |
| Empty | Profile-extreme, no photo ✓ | Security-no-passkeys ✓ | n/a | n/a | Connected-empty ✓ | Data (base) ✓ | n/a |
| Long or extreme | Profile-extreme ✓ | copy ✓ | Long email: harness, not producible (literal data) | copy ✓ | copy ✓ | n/a | Sidebar in Profile-extreme ✓. Index with a long name at 320: not drawn |
| Initial loading | "follows Security-loading" (no field or preview shapes) | Security-loading ✓ (C-14) | n/a | **GAP** (C-08) | **GAP** (C-08) | **GAP** (C-08) | n/a |
| Saving (busy) | copy ✓ | Security-passkey-waiting ✓ | copy ✓ | n/a | copy ✓ | copy ✓ (on hold) | n/a |
| Saved | Profile-name-saved ✓ (C-30) | copy ✓ | n/a | copy ✓ | Toast in Connected-empty ✓ | copy ✓ | n/a |
| Field validation | Profile-name-error (+dark) ✓ | copy ✓ (Rename) | Stepup-code-error (+dark) ✓ | n/a | n/a | on hold | n/a |
| Server error | Profile-save-failed ✓ | List load copy ✓; action failures **GAP** (C-08) | Send failed copy ✓ | copy ✓ | **GAP** (C-08) | **GAP** (C-08) | n/a |
| Offline | **GAP**: Save has no copy (C-08) | **GAP** | copy ✓ (send failed) | Preferences-offline ✓ | **GAP** | **GAP** | n/a |
| Rate limit or cooldown | Timer in Profile-email-code ✓ | copy ✓ (10-passkey limit) | Stepup-code-locked ✓; hard send cap **GAP** | n/a | n/a | copy ✓ (once a day) | n/a |
| Destructive confirmation | copy ✓ (discard) | Security-remove-passkey (+mobile) ✓ | n/a | n/a | Connected-remove ✓ | on hold | n/a |
| Step-up | Profile-email-change, Profile-email-code ✓ (C-06) | Stepup ✓ | 5 boards ✓ | n/a | n/a | copy ✓ | n/a |
| Device waiting, failed | n/a | waiting and failed ✓ | copy ✓ | n/a | n/a | n/a | n/a |
| Not allowed | n/a | n/a | n/a | "Always on" ✓ | n/a | n/a | n/a |
| Focus after the action (§4.0, §7.4) | Remove photo **GAP** (C-07) | ✓ (h2 `tabindex=-1`) | Locked **GAP** (C-07) | n/a | **GAP**: no h2 (C-07) | n/a | n/a |
| Dark | Profile-dark, Profile-name-error-dark ✓ | Security-dark ✓ | Stepup-code-error-dark ✓ | Preferences-dark ✓ | Connected-dark ✓ | Data-dark ✓ | Account-index-dark ✓ |
| Visible focus | `is-focused` on 13 boards ✓ | | | | | | |
| Forced colours, reduced motion | this pass ✓ | | | | | | |
| Widths, short height, 200 % zoom | testers A/B | | | | | | |

In the last four rows, one entry covers every column. For dark, every other state also passed in this pass, with `.dark` applied to all 42 boards.

## Prototype and link integrity

**Board files vs `canvas.json`**
- 42 board files and 44 `canvas.json` entries.
- The 2 entries without files are the on-hold `Data-delete-confirm` and `Data-deleted`. They are kept on purpose, per the canvas NOTES.
- No board file is missing an entry.

**Sizes**
- `$preview` equals the `canvas.json` w×h on all 42 boards.
- Measured content height at 1440 fits every frame: for example Security 1452/1500, Preferences 1176/1200, Profile-name-error 908/1000, Profile-extreme 888/900.
- Only the spec’s §6.2 table is stale (C-27).
- Every board’s imports match the §6.2 Imports column.

**Links**
- 5 board link targets, plus `Main.dc.html#email`, all resolve. `#email` exists on Main and Profile-extreme.
- Out-of-area links use `#richie`, `#help`, `#privacy` and `#download-zip`.

**Navigation per spec**
- The sidebar’s 5 items go to Main, Security, Preferences, Connected and Data (§4.1).
- The mobile back link "Account" goes to `Account-index` on all 16 page boards (§3.3).
- The index rows go to the 5 pages, and "Back to Richie" goes to `#richie`.
- Security’s "Change email" goes to `Main.dc.html#email` (§4.3).

**Not clickable (information only)**
- Overlay boards can’t be reached by clicking in the prototype, because dialog triggers are buttons. That is the intended trade-off (NOTES gotcha 7), so reviewers open overlays from the canvas.

## Handoff readiness

**Buildable from board, spec and `account.css`:** everything except the points below. The `aria-current` requirement is documented.

**Before handoff**
1. C-01, C-02, C-03: copy that disagrees with the spec.
2. C-11: spec and CSS drift, with the decisions recorded only in local NOTES.
3. Mapping fixes in §8: C-09 (segmented), C-12 (ItemSeparator), C-15 (Spinner), C-10 (`EmailText`).
4. C-07 and C-08: focus targets and the missing state copy that a developer would otherwise invent.

**Inline styles:** only the wrapper sizes (canvas-only), the slider’s `--ra-val` state value, and C-25.
