# How Richie designs

**Revision 2 (2026-11-05).** Rewritten after account settings reached Ship (8.7/10). Revision 1 (2026-11-04) described only the identity canvas, before settings existed; its advice to rename `Main.dc.html` was wrong (§5) and its gap list is re-checked in §6. This revision records the pipeline as we actually ran it, compares the two design systems with suiss, and sets rules for the next identity area: edge pages, recovery and SSO entry points.

Section numbers changed: the state checklist is now §4 (was §6) and testing is §5 (was §8). The settings spec and QA reports cite the Revision 1 numbers.

This is unfinished work, described as it is. Both canvases ship with known gaps (§6), and several recommendations below still need a product or visual check before they become decisions.

**Sources** (read 2026-11-05; treat the canvases as the record, this file as the summary):

| Source | Version / location |
|---|---|
| **Richie Identity** canvas, `project/richie.css` (`rc-*`) | https://claude.ai/artifact/9EbW44Zt3zAyXumY4m4Rr2, version `1793882377-cd80`, **35 boards** (25 full markup, 10 wrappers) |
| Identity review ("Ship with fixes", all five groups) | https://claude.ai/artifact/WBt4MiWTL6hjzD2B6gfVFD ("Still open" panel, UX evaluation, guidelines audit) |
| **Richie Account Settings** canvas, `project/account.css` (`ra-*` + suiss `data-slot`) | https://claude.ai/artifact/SQiXuTqyASonZHTVzVFW3y, version `1793877977-4d5d`, 56 boards. Local mirror `.design/canvas/project/` matches it byte for byte |
| Settings spec, evaluation, QA | `account-settings/spec.md` (Rev 5), `evaluation.md` (rounds 1–4), `qa/qa-{a,b,c,regression,final-check}.md`, builder notes `.design/canvas/NOTES.md` |
| suiss | [`suiss-ui-guide.md`](suiss-ui-guide.md) §2, [`suiss-blocks-catalog.md`](suiss-blocks-catalog.md) |

Values marked **(m)** were measured for this revision: both canvases built with the rig and probed in Chromium on macOS (SF Pro), or ratios computed with the WCAG formula. Other ratios come from `.design/wp3-1/contrast-report.md`.

Product facts every design respects: accounts are personal; identity pages carry nothing workspace-related. Sign-in is passwordless: an 8-character Crockford Base32 **email code** (5 minutes, 4 tries, at least 1 minute between sends) or a passkey. The brand mark is Richie, a pixel shiba.

**What both canvases agree on** (Revision 1 principles that survived two areas): one filled primary per screen and always a way out; state changes never move the page (reserved slots); say what happened and what to do next, with real numbers; accessible as drawn (real controls, labels, `aria-describedby`, `aria-disabled`); inputs that work with the platform (`autocomplete`, `inputmode`, one real OTP input); departures from suiss only for a measured reason, written down; the shiba is the only brand element.

---

## 1. Pipeline as practised

### What we do (account settings, 2026-11-04 to 11-05)

| # | Step | What happened | Result |
|---|---|---|---|
| 1 | **Spec** | WP3-1 wrote IA, layout, copy, a state matrix (§5: every checklist item maps to a board, "copy", "harness" or "n/a"), the board list with canvas positions (§6.2), a CSS plan with every contrast pair computed (`contrast.mjs`, 49 pass), the suiss handoff map (§8) and open questions with defaults (§9) | `spec.md`, Rev 1 → Rev 5 in two days |
| 2 | **Foundation** | The lead published `canvas.json` for every planned board, `account.css`, the shared `Account-sidebar` part and one finished base (`Main`), plus tested kits (`.design/qa-src/foundation/Kit-*.dc.html`) and builder notes | Version `1793819128-f0e5`; `Main`, `Profile-dark` and `Profile-mobile` at 0 blockers, 0 majors; kits with no blockers |
| 3 | **Row builders** | Parallel builders copied the kits row by row, filed CSS gaps in `CSS-REQUESTS.md` and layout changes in `CANVAS-CHANGES.md`; the lead consolidated | 42 boards; rig 0 blockers, 395 majors (all false positives) |
| 4 | **Rig QA round 1** | Three independent testers: A visual/responsive (23 boards), B (19 boards), C code and content (42 boards) | 1 blocker, 20 majors, 42 minors in total |
| 5 | **Fix + rig QA round 2** | fix-1 (51 boards), rig tuned, regression QA-R (R-01–R-23), fix-2, final check QA-V | 22 of 23 resolved, 1 partly |
| 6 | **Independent evaluation** | A fresh evaluator with a fixed 8-criterion rubric and ship bar, who never reads QA or notes. R1 8.3 **No-ship** (ST-1 lost mailbox, DL-1 export link) → fix-3 (56 boards, spec Rev 4) → R2 8.6 → polish → R3 8.7 → R4 (Profile only) **8.7 Ship** | `evaluation.md`; four rounds to Ship |

Identity ran a lighter version: four QA rounds of three testers split by page group (14 viewports, keyboard, axe, forced colours), then an unscored UX evaluation plus a Web Interface Guidelines audit by a separate agent, then product decisions. It has **no spec in the repo**; its reasoning lives only in the review artifact and CSS comments.

### What worked

1. **Contrast computed in the spec, before any board.** No QA or evaluation round found a contrast failure in settings.
2. **Foundation and tested kits before rows.** Builders copied markup that had already passed the rig, and `NOTES.md` gave them skeletons, gotchas and icon snippets.
3. **A state matrix with a board or a reason in every cell** (spec §5). Coverage was reviewable; the evaluator scored States 8.0–9.5.
4. **Layout shift as a measurement.** Recording the y of the next heading across states (`#email-title` 676 at 1280 across four Profile states) made "states don't move" testable.
5. **An evaluator blind to QA found the journey gaps.** ST-1 and DL-1 were dead ends that three rig rounds missed: the rig tests renders, not journeys.
6. **Working assumptions written as defaults** (§9 Q1–Q8), so design never stopped on a product question.

### What cost time or rework

1. **Scope moved mid-build.** The identity-only decision removed the Workspaces row after 4 boards were published; deletion went on hold, then came back as Row 8 (8 boards). Row positions changed three times (`NOTES.md`).
2. **Rig noise.** 395 majors at consolidation and 472 after fix-1 were all false positives (overlay text-overlap, `.is-focused`, intended truncation), discarded by hand until QA-R tuned the rig.
3. **Per-row stylesheets** (`account-security.css`, `account-data.css`) had to be folded back into `account.css`.
4. **Fixes broke spec–board–CSS agreement.** X-1 (two sources of truth, 142 lines of review IDs in the spec); 4 of 8 round-2 issues (N2-01, -02, -05, -06) and the only round-3 issue (N3-01) were mismatches introduced by fix passes.
5. **The phone Profile order took four passes** (PR-2 → CSS reorder → N2-02 → N3-01) because it began as a CSS reorder that broke DOM order.
6. **Five "empty space" findings** (PR-1, PR-4, SE-2, ST-2, PF-1) ran three rounds until the spec wrote down why reserved space stays (R3.4).
7. **Full-copy drift.** QA-C C-02: the preview conversation differed from the spec on 11 Profile boards.

### What we should do better

1. Settle recovery, lifecycles, enumeration and rate limits, and walk every journey in the spec for dead ends, **before** the foundation. Both round-1 Majors were journey gaps.
2. Tune the rig and freeze the false-positive list (§5) before QA round 1.
3. One source of truth per concern from Revision 1: the stylesheet for values, the spec for behaviour, copy and states, `canvas.json` for positions. Review IDs go in a changelog.
4. After every fix pass, check each changed board against the spec text before re-grading (evaluation R2.7).
5. Write the rationale for deliberate choices (reserved space, DOM order) into the spec before the evaluator sees them.
6. Brief builders to run the rig. The Design type's own `SKILL.md` says "never verify unless the user asked", so builders skip checks unless the brief asks.
7. Give identity work a spec and keep its QA and evaluation under `docs/design/<area>/`, as settings does.

---

## 2. The two systems and suiss, side by side

Identity tokens live on `.rc` (`richie.css`, 19,794 bytes). Settings tokens live on `:root`/`.dark` with suiss `data-slot` recipes (`account.css`, 90,230 bytes). Light tokens are identical except `success` and the link colour. "New pages" is the recommendation for new identity pages.

| Aspect | Identity (`rc-*`) | Settings (`ra-*`) | suiss | New pages | Why |
|---|---|---|---|---|---|
| Light tokens | bg `#fff`, fg `#1d1d1f`, muted `#f5f5f7`/`#6e6e73`, border `#d2d2d7`, input `#86868b`, fill `#0071e3` (hover `#0068d6`), danger `#d0000a`, success `#1e8e3e` (richie.css:15-33) | Same; success `#16a34a`, icons only (spec §7.1) | Same; no success | **Settings** | Only success differs, and settings defines it for both themes |
| Dark tokens | muted `#1d1d1f`; fill stays `#0071e3`; link and focus `#2997ff`; danger `#ff5447`; **no success** (richie.css:53-69) | muted `#161617`, popover `#1d1d1f`, success `#4ade80`, fill `#0071e3` | muted `#161617`, primary `#2997ff` | **Settings** | Identity's `#1d1d1f` muted is undocumented; suiss and settings agree. Both keep `#0071e3` fill (white on `#2997ff` is 3.0) |
| Link colour (light) | `#0071e3` (4.70 on white, **4.31** on `#f5f5f7`); the chip overrides to `#0066cc` (richie.css:26, 505) | `#0066cc` everywhere (5.57 page, 5.11 Mist) | `#0071e3` | **Settings** | One value that passes on every surface; identity already needed the override |
| Type scale | **Height-based.** Title 34/38 phones; 40/44 at ≥768 w and ≥640 h; 48/52 at ≥768 w and ≥860 h; 26 at ≤540 h (richie.css:172, 694-729). Body, input, button 17; lede 17 (19 desktop); label 14/600; hint 13; error 14; notice 15; legal, note, footer 12 | **Fixed.** Title 30/36/600 −0.025em; heading 18/28; body, label 16/24 (label 600); detail 14/20 (hints, errors, status, button labels); caption 12/16; `lg` button 16 (spec §1.3) | `text-sm` 14/20, `text-base` 16/24; auth heroes `text-4xl md:text-5xl` (catalog, auth frame) | **Settings** | §2.1 |
| Field | 52 tall, outlined 1 px `--input` on the page, radius 12, 17 px, padding 16; 44 at ≤540 h (richie.css:244, 705) | 44 tall, filled `--secondary` (black 4 % / white 10 %) **plus** 1 px `--input` edge, radius 10, 16 px, padding 12 (account.css:993) | 44 filled, no edge (1.09:1 vs page), radius 10 | **Settings** | §2.1 |
| Code entry | Slots `clamp(44px, 13cqi, 60px)` tall (57 at a 442 row (m)), ≤48 wide, radius 12, glyph 20–28/500, gap 4–6 (richie.css:599) | Slots 52 tall (44 below 360 cqi), `min(44px, …)` wide, radius 10, filled + edge, glyph 20/600, gap 6 (spec §7.2 row 20) | 52 × 44, radius 10, 20 px | **Settings** | One code row for sign-in, step-up, change email and export |
| Buttons | Above 480 px: pill, min 240 × 44 (**280** on passkey pages), centred. At ≤480: full width, **50** tall, radius 12 (richie.css:317-358) | Rows: pills 32/36. Forms and solo pages: `lg` 44, 16 px; block buttons full column width, radius 10 (`lg`) or 8 (account.css:817-925) | 24/32/36/44/56 pills; block radius 8 | **Settings `lg` block** | One shape at every width, the same width and height as the field above it ((m) `Export-start` 460×44 field and button at 1280); ends the 240/280 split (review C5) |
| Radii | 12 (fields, slots, phone buttons), 999 (desktop buttons, chip), circles | suiss scale 8/10/12/18/28, pills | Same as settings | **Settings** | suiss scale; maps 1:1 to `@repo/ui` |
| Column and padding | Column 460, lede 400, OTP 442, note 380; sides 20 (16 below 360, 24 desktop); top `max(32px, 8vh)` rising to `max(64px, 12vh)` | Solo: column 460, sides 16, top 32 (64 at ≥768), footer 48 above; lede ≤560 | `w-115` (460) | **460 column and 400 lede from identity; padding from settings** | vh padding sinks content in tall windows ((m) Signin's button at y 436 at 1280×800, 492 at 1280×1000) |
| Solo-page frame | Centred column; **centred** mark, h1 and lede; fields start-aligned (richie.css:127, 213) | Centred column; **everything start-aligned**, mark at the start (account.css:2480-2525) | Centred column, centred text | **Identity** | These are identity-app pages, and settings spec §8 row 28 says to build them with identity's `rc-main`/`rc-col`. Otherwise Signin → `Signin-deleted` jumps ((m) mark x 164 → 16, title 34 centred → 30 start, at 390) |
| Shiba | SVG blob `68a5a50d…` (31×39 grid, `crispEdges`) at 62×78; 93×117 at ≥768 w and ≥640 h; 31×39 at ≤540 h; `alt="Richie"`; ok/neutral badges | PNG blob `70084467…` (identity's old PNG) at 31×39 at every width, `alt=""`, start-aligned | Icon disc | **Identity SVG at 62×78**; 31×39 at ≤540 h and on deletion-lifecycle pages (DL-5); `alt=""` | 3× exists only for the 48 px title and sits on half pixels ((m) x 593.5 at 1280). The SVG stays crisp at 125 %. `alt=""` because the footer names Richie |
| Footer | "Richie" (600, Ink) + Help, Privacy, Terms; 12 px | Help, Privacy, Terms; 12/16 | — | **Identity** | Names the product where the h1 doesn't (review C4) |
| Dark mode | `.dark` on a wrapper (boards); `.rc.rc--system:not(.light)` under `prefers-color-scheme` for production, used by no board (richie.css:53-91) | `.dark` on any ancestor; `[data-appearance="system"]:not(.light)` under the media query; base boards always light (account.css:7-13, 162-216) | `.dark` on `<html>` | **Settings mechanism; production identity always System** | Same shape in both. Tokens on `:root` reach imports; one hook name across apps. Before sign-in the OS is the only signal |
| Icons | Hand-drawn stroke SVGs: key, circle-!, check, ! | Phosphor regular `@phosphor-icons/core@2.1.1`, inline, `fill="currentColor"`, 16/20/24 | Phosphor | **Settings** | The repo declares Phosphor. The passkey glyph is still open (FIDO icon, review) |
| Focus ring | Buttons and links: 2 px outline, offset 2, no halo. Fields and slots: border + 2 px ring + 3 px halo. Invalid field: **red** ring (richie.css:259-271, 337) | Buttons: 2 px outline, offset 2, 5 px halo spread. Fields: border + 1 px ring + 3 px halo. Invalid + focused: **blue** wins. Programmatic targets: no ring (account.css:2602-2625) | `ring-3 ring/50`; red on destructive | **Settings** | One focus colour; the ring is 4.70 / 6.96 in both |
| Disabled / unavailable | `.is-unavailable` at opacity .5: **2.09:1** light, 2.94 label / 1.81 fill dark (m). Locked and expired code input `disabled` | No opacity states. Unavailable: transparent, `--input` edge (3.62/4.14), Slate label (5.07/8.16), countdown in the label. Locked input `readonly` + `aria-disabled` | `opacity-50` | **Settings** | Fixes the dimmed primary and keeps focus |
| Busy | CSS ring spinner 18 px, 0.8 s linear; gerund + "…"; `aria-disabled` | suiss 8-spoke SVG 16 px, `steps(8)` 0.8 s, `aria-hidden`, no role; gerund + "…"; `aria-disabled` + `data-busy` | 8-spoke | **Settings** | It is the suiss `Spinner` |
| Status and live regions | Verify: polite wrapper `#code-status` **plus** a second polite `.rc-sr`; passkey pages `p[role=status]`; send failure in `role=status` | One `.ra-slot` grid cell: `p[role=status]` and `p[role=alert]`, reserved height | — | **Settings** | §4 |

### 2.1 Confirm or challenge: fields and the type scale

**44 px fields (settings spec §2.2): confirmed.**
- Identity already drops to 44 at ≤540 px height (richie.css:705; (m) 224×44 at 1280×540), so 44 is proven in its own frame.
- 52 px fields above 44 px desktop buttons never lined up (Revision 1 §4); (m) Signin at 1280×800 draws a 460×52 field over a 240×44 pill. Settings' `Export-start` draws a 460×44 field over a 460×44 button.
- The edge carries the boundary in both styles: 3.62 light and 4.14 dark. The 4 % fill only lowers the placeholder from 5.07 to 4.64, still a pass. 16 px text is the floor that stops iOS zooming on focus.

**Drop the height-based scale: confirmed.** (m) At 1440×900 the 48 px title wraps to two lines on `Main`, `Passkey-offer` and `Passkey-exists`, but stays on one at 1280×800 (40 px). The reservations `.rc-title--2l` and `.rc-lede--2l` (richie.css:194-203, 671-674, 728) exist only to absorb that.

**Challenge: "fixed" does not have to mean settings' 30.** suiss auth heroes scale by **width** (`text-4xl md:text-5xl`). Measured line counts across the 9 non-wrapper title layouts (m):

| Title size | 1280×800 and 1440×900 | 390×844 | 320×640 |
|---|---|---|---|
| Current (34 / 40 / 48) | none at 1280×800; 3 wrap at 1440×900 | 1 wraps | 5 wrap |
| 48 (suiss `md:text-5xl`) | 3 wrap | 7 wrap | 9 wrap (`Main` to 3 lines) |
| 36 | none wrap | 1 wraps | 6 wrap |
| **30 (settings)** | **none wrap** | **none wrap** | **3 wrap** |

**Recommendation: fixed 30/36 at every size.** It wraps least, matches the solo pages already drawn on the settings canvas, and leaves the `--2l` reservations needed only at 320. The cost is a smaller desktop hero; the 62×78 mascot carries the presence. Compare one rendered 1440 board at 30 and 36 before the spec commits.

**Contrast pairs specific to identity** (m; the full settings table is in spec §7.1):

| Pair | Light | Dark |
|---|---|---|
| Identity `.is-unavailable` label on its own fill / fill vs page | 2.09 / 2.09 | 2.94 / 1.81 |
| Link `#0071e3` on `#f5f5f7` (why the chip overrides) / `#0066cc` on `#f5f5f7` | 4.31 / 5.11 | — |
| White check on identity success `#1e8e3e` | 4.21 | no dark token |
| Mascot badge with settings success: white on `#16a34a` / white on `#4ade80` / Ink on `#4ade80` | 3.30 | **1.74** / 9.66, so the dark badge glyph must be Ink |
| Disabled slot edge `#d2d2d7` on Mist / dark `#424245` on muted (identity, settings) | 1.38 | 1.68, 1.81 (disabled: exempt; never use for live controls) |

---

## 3. Copy voice

### Rules (one set for both apps)

1. **US English** ("canceled", "recognize", "color"). Sentence case everywhere, buttons and headings included.
2. **"You" for the person, "we" for Richie** in prose ("Enter the code we sent to…"). **No "me" or "my" in controls**: labels are verb + object ("Keep account", not "Keep my account").
3. Contractions, curly apostrophes and a real ellipsis. Digits for numbers; m:ss for timers. No Oxford comma ("face, fingerprint or screen lock").
4. **Errors say what to do, with real numbers.** Never "invalid", "oops" or "!" (sample customer messages excepted).
5. **Failures use the three settings patterns:** "We couldn’t {phrase} because you’re offline." / "We couldn’t {phrase}. Check your connection." / "We couldn’t {phrase}. Try again in a moment." (spec §4.0).
6. **Countdowns start at 1:00**, driven by the server's next-allowed time. Never draw 0:59.
7. **Busy labels** are a gerund plus "…": "Sending code…", "Checking code…", "Waiting for your device…".
8. **Headlines:** an imperative for a task page ("Sign in to Richie", "Check your email", "Download your data"), a full sentence for an outcome page ("Couldn’t sign you in", "This account was deleted"). No noun labels such as "Passkey created".
9. **Outcome pages say what happened, then the next step**, and name what stops working (settings principle 5).
10. **"Password"** appears only in "password manager" and in "There’s no password to forget."
11. **Dates:** "March 12"; "January 4, 2027"; "9:14 AM"; "Today", "yesterday", "2 hours ago" within 7 days.
12. **Screen-reader text** starts with the visible label and stays specific ("Email code, 8 letters and numbers"). Wrap emails and names in `translate="no"`.

### One name per thing

| Thing | Use | Not |
|---|---|---|
| The 8-character code | **email code**; inside a flow, "the code"; sr label "Email code, 8 letters and numbers" | sign-in code, verification code, code from your email |
| First send | **"Send code"** (proposed) | Send sign-in code, Email me a code |
| Any later send | **"Send a new code"**, "Send a new code in 1:00" | Resend code |
| Code instead of a passkey | **"Get a code by email instead"** | Email me a code instead |
| Passkey instead of a code | **"Use a passkey instead"** | |
| Making an account | **"Create account"** (button), "Create an account" (link) | sign up, register |
| Signing in | **sign in** (verb), **sign-in** (noun) | log in, login |
| The address | label **"Email"**; example **name@example.com** in placeholder and error | Email address (label), you@example.com |
| A signed-in browser / an OAuth grant / the export / deleting | **device** / **connected app** / **your data**, **download** / **delete your account** | session / integration / export, archive / close, deactivate |

### Inconsistencies between the canvases

| Topic | Identity (boards) | Settings (boards) | Use |
|---|---|---|---|
| First send | "Send sign-in code" (`Signin:30` and its 5 states) | "Email me a code" (`Export-start:27`, `Export-request:27`) | "Send code" |
| Fallback to a code | "Get a code by email instead" (`Passkey-signin`, `-waiting`, `-failed`) | "Email me a code instead" (`Stepup:28` and the step-up boards, spec §4.4) | Identity's |
| Code sr label | "Code from your email, 8 letters and numbers" (all 8 `Verify*`) | "Email code, 8 letters and numbers" (`Stepup-code`, `Export-request-code`) | Settings' |
| Code lede | "Enter the code sent to…" (`Verify*` l.23) | "Enter the code we sent to…" (`Export-request-code`, `Profile-email-code`) | Settings' |
| Countdown start | 0:59 (`Verify:36`, `Verify-sent:36`) | 1:00 (spec §4.0) | 1:00 |
| Locked | "No tries left for this code." + separate line "You can send a new code in 0:27" + dimmed button (`Verify-locked:36,40`) | "That was the last try for this code." + "Send a new code in 0:41" in the button (`Stepup-code-locked`) | Settings' |
| Expired | "This code has expired. Send a new one to continue." (`Verify-expired`) | "This code has expired." with a "Send a new code" button (spec §4.4) | Settings' |
| New code sent | "New code sent. Use the code in the newest email." (`Verify-sent`) | "We sent a new code." (spec §4.4, no board) | Identity's: it says which code to use |
| Send failed | "We couldn’t send a code. Try again in a minute." (`Signin-send-failed:31`) | The three patterns (spec §4.0) | Settings' |
| Passkey failed | "The request timed out or you cancelled it." (`Passkey-not-created:23`, `Passkey-signin-failed:23`) | "The passkey prompt closed. Try again." (`Security-passkey-failed`); "…canceled or timed out, or this device has no passkey…" (spec §4.4 screen A) | One sentence: "The request timed out or you canceled it." |
| Spelling | "cancelled" (the two boards above; canvas title "Sign in, passkey failed or cancelled") | "canceled" | US |
| Example address | Placeholder "you@example.com" (9 boards); error "name@example.com" | "name@example.com" in both | Settings' |
| Email error, label | "Enter a full email, like…" (`Signin-error`, `Signup-error`); label "Email" | "Enter a full email address, like…" (spec §4.8); label "Email address" (`Export-start:24`) | Identity's label; settings' error sentence |
| Headline forms | Noun labels "Passkey created", "Passkey not created", "Passkey already set up" | Sentences and imperatives (solo pages) | Rule 8 |
| First person | — | "Keep my account" (`Deletion-pending:27`) | "Keep account" (proposed; pairs with "Delete account") |
| `<title>` | App titles mixed with state names; `Signin-error:6` "Sign in to Richie, check your email" | Always the board title (spec §6.1) | Settings'; the spec lists document titles as "{h1} – Richie" |

---

## 4. States and accessibility

### State checklist (every new identity page)

Draw a board, or write "copy", "harness" or "n/a" with a reason, in the spec's state matrix (settings spec §5 is the model).

- [ ] **Data:** default with realistic data · long or extreme (a 64-character email, a long domain, a long name) · prefilled from context (`login_hint`, return URL) versus empty.
- [ ] **Arrival:** where focus lands (the h1 on outcome pages; the field on form pages with a fine pointer).
- [ ] **Async:** submitting (busy) · success or next page · server error · network error · offline · rate limit or cooldown (countdown in the button) · **sign-in request expired** (IdentityServer).
- [ ] **Codes:** wrong with 3, 2 and 1 tries left · incomplete · locked · expired · new code sent · send failed · can't get the code (recovery to support).
- [ ] **Passkeys:** waiting · failed or canceled · already set up · browser without WebAuthn (hide the button) · conditional UI (autofill).
- [ ] **Account cases:** unknown email · already registered · deleted in the last 30 days (`Signin-deleted`) · scheduled for deletion (`Deletion-pending`) · remembered account and "Not you?".
- [ ] **Enumeration and limits:** state per page what it reveals and its rate limit.
- [ ] **Presentation:** 320, 375/390, 768, 1280, 1440 · ≤540 height (844×390, 1280×540) · dark for every state (harness; **draw** dark for at least every error) · forced colours · reduced motion · 200 % zoom · visible focus drawn on one board per flow.

**Identity today (35 boards):** default, field errors, unknown and registered email, send failed, passkey waiting and failed on sign-in, every Verify state except 1 try left, passkey offer, created, not created and already set up, passkey sign-in with waiting and failed, mobile and dark wrappers. **Still missing:** busy email submit; the send cooldown (review C2: within 60 s, go straight to Verify with the countdown); WebAuthn unavailable; 1 try left; desktop renders of states (every state is 390 wide); dark error states; long data; all edge, recovery and SSO pages.

### Accessibility patterns that survived QA

| Pattern | Rule | Proven on |
|---|---|---|
| Status slot | One grid cell holds an always-present `p[role=status]` and a `p[role=alert]`. Reserved height by column width (20/40/60); every status fits one line at a 480 px column (`measure-status.mjs`) | Profile, Security, Preferences; positions unchanged across states (evaluation R1, strength 2) |
| Busy button | Keeps its variant; spinner `aria-hidden` with no role; gerund + "…"; `aria-disabled="true"`, never `disabled`; the build guards clicks | `Security-passkey-waiting`, `Verify-checking`, `Passkey-*-waiting` |
| Unavailable countdown | `aria-disabled`, outline look with a Slate label, the countdown in the label, `min-width` set by the widest label | `Stepup-code-locked` |
| Locked code input | `readonly` + `aria-disabled`; focus moves to the error message (`tabindex="-1"`, no role, no ring); the slots draw the ring when the input has focus | `Stepup-code-locked` (QA R-02, R-03) |
| Focus after an action | Outcome h1 `tabindex="-1"` on arrival, no ring; removed row → section h2; per-dialog initial focus; one rule per step-up screen | Spec §7.4; evaluation N2-05 |
| Field errors | Submit moves focus to the first invalid field; the error is read through `aria-describedby` and has no role. `role="alert"` only when focus stays elsewhere | `Profile-name-error`; identity `Signin-error`, `Signup-error` (`autofocus`) |
| Live regions | A countdown is never live; announce "You can send a new code now." once; change text inside existing regions, never remount them; one polite region per area | `Verify-resend`; spec §4.4 |
| Long email | One sr-only full address plus `aria-hidden` visual halves; ellipsis only on the local part via `text-overflow`; the domain never truncates; prose wraps after the @ | SH-2, `Profile-extreme`; identity chip |
| Accessible names | Start with the visible label; add context as an sr suffix or through `aria-describedby` | QA R-11; identity "Not you?" |
| Skip link | First stop on shell pages, to `#page-title`. Solo pages have one `main` and need none (guidelines audit) | SH-1 |
| Dialogs | Every dialog is a form; Enter submits; Escape cancels; outside clicks do nothing; one height across states | Step-up, delete |
| Forced colours, motion | Transparent outlines keep shapes; `GrayText` for unavailable; the spinner slows to 2.4 s | Spec §7.6–7.7; richie.css:733-755 |

---

## 5. Canvas conventions and testing

| Convention | Identity | Settings | For a new canvas |
|---|---|---|---|
| `canvas.json` | `v` 3, `launch` canvas, explicit `order`, 5 `title1` notes; **no** `is_interactive` although boards link | Editor format (`attachments`, `noteOrder`, note `w`, `maxW` ≤8000); 30 boards `is_interactive`; `launch` still canvas | Keep the editor's format; mark linked boards `is_interactive` |
| Entry board | `Main` = Create account | `Main` = Profile | `Main.dc.html` is the entry the Design type requires (`SKILL.md`). Revision 1 was wrong to rename it; title it after its screen |
| File names | `<Flow>[-<state>]`, wrappers `-mobile`, `-dark` | `<Page>[-<state>][-mobile\|-dark]` | `<Area>-<screen>[-<state>][-mobile\|-dark]` |
| Rows and columns | Rows are flows at a 1280 pitch; base 1440×900 at x 0, then 390×844 boards from x 1520 at a 470 pitch | Rows are pages at a pitch of tallest + 400; columns desktop, mobile, dark, states; 80 px gaps | Settings'; title notes 260 above each row (the type wants ≥223) |
| Board sizes, `$preview` | Bases 1440×900; states 390×844 with `fill` | Measured height rounded up to 100 (min 900); states at 1440 | Settings'; `$preview` equals the board's w×h |
| Wrappers | 390 mobile and 390 dark only | 390 mobile; dark at 1440 with no width so `fill` works; dark helmet `body{margin:0;background:#000}` | Settings' |
| Overlays | None | `<div inert>` around the import, then a sibling `.ra-layer`; layers sit outside size containers; mobile overlay boards wrap the desktop one | Settings' |
| States | All 25 non-wrapper boards are full markup | 12 full copies + 3 own-sidebar copies; overlays import | Full copies only for content changes; re-copy, never hand-patch; run `drift.mjs` |
| Imports and props | Wrappers only | Shared sidebar part. `format.md` lets imports take props, but props need `{{holes}}`, which the rig can't render, so boards stay static | Static markup only: no holes, `sc-if`, `sc-for` or Tweaks |
| Multi-state, conditions | `--2l` min-heights | `.ra-steps`/`.ra-step-ghost` (`inert`, `aria-hidden`); `data-when="has-passkey"` on content the app drops | Settings' |
| Canvas-only class | `.is-focused` | `.is-focused`, never on programmatic targets | Same |
| Assets | Mascot SVG `/_blob/68a5a50dd8aeb4a01b6a7b84346af827` | Mascot PNG `/_blob/70084467a4a633969708f2f323162f2d` | Blobs are per canvas: upload the identity SVG and record its new hash in the spec |
| Icons | Hand-drawn inline | Phosphor inline from `.design/canvas-icons/snippets.html`, checked with `check-icons.py` | Settings' |
| Stylesheet | `richie.css`, linked in `<head>` | `account.css` only; per-row sheets forbidden | One sheet per canvas, with a comment for each departure |

### Testing

```
cd .design/qa-rig && npm i && npx playwright install chromium        # once
node .design/qa-rig/build.mjs <project> <out> <blob-dir>               # boards → X.html (fluid) + X.fixed.html
lsof -i :<port>                                                        # must print nothing
node .design/qa-rig/run.mjs --out <out> --port <port> [--boards A,B] [--variant fluid|fixed|both] [--forced-colors] [--motion reduce]
node .design/qa-rig/drift.mjs <project> [--strict]                     # shared chrome copied literally
node .design/wp3-1/measure-status.mjs <strings.json> 458,328,266       # status lines fit their slot
python3 -I .design/canvas-tools/check-icons.py .design/canvas-icons <boards>
```

- **Dark for every state:** build a second copy with `.dark` on `<body>` (as the evaluator's `eval-dark`). Base boards are always light, so `emulateMedia` doesn't change them.
- **Viewports used to reach Ship:** 320×640, 375×812, 390×844, 768×1024, 1024×768, 1280×800, 1440×900, 1920×1080, 1280×540, 844×390, and 640×400 (200 %).
- **Layout shift:** record the y of the heading after each status slot across state boards.
- `.design/` is git-ignored (`.git/info/exclude`) and exists only in this worktree. Copy the rig before relying on it elsewhere.

**Known false positives** (read as noise; anything else is real until disproved):
- Text overlap between the inert page and a dialog or toast; dialog body text scrolled under a sticky footer.
- "No visible focus" on `.is-focused` elements and on the hidden OTP input whose slots draw the ring (`Stepup-code-locked`).
- Clipped text on intended clamps (the `Profile-extreme` sidebar name).
- Target size: 32 px desktop row buttons, 36 px sidebar items, inert page buttons; segmented radios measured on the input (30) instead of the label (45).
- `.fixed` 390 wrappers overflowing at 320 and 375; axe contrast "needs review" over alpha surfaces; inline legal links under 24 px (exempt).
- Keyboard listings that include `aria-hidden` ghost labels in a name.
- **Identity only:** boards that switch on `.dark` alone look the same in both schemes, so test through the `-dark` wrappers. **The rig renames every blob to `.png` (build.mjs:78, 122), so identity's SVG mascot renders broken** ((m) `naturalWidth` 0). Layout is unaffected (`.rc-mark` sizes the box); check the mascot by eye on the canvas, or fix the rig.

---

## 6. Known gaps: do not copy

### Identity canvas (`1793882377-cd80`)

1. **Dimmed primary on the locked screen.** `Verify-locked:40` uses `.is-unavailable` (richie.css:353), 2.09:1, with the countdown on a separate line (`:36`).
2. **Code input `disabled`** in `Verify-locked:27` and `Verify-expired:27`, which drops focus. Both keep the lede "Enter the code sent to…" (`:23`) above a dead input.
3. **"use a different email" always goes to `Signin.dc.html`** (`Verify:43` and every Verify state), even from sign-up. The review says the build returns to the origin, but no board or note says so.
4. **`alt="Richie"` on all 25 non-wrapper boards** (e.g. `Signin:21`): the brand is announced before every h1.
5. **The height-based scale** (richie.css:694-729) and its reservations (`:194-203`, `:671-674`, `:728`).
6. **The 3× mascot centres on half pixels** ((m) x 593.5 at 1280, 673.5 at 1440). Open in the review.
7. **Two desktop button widths:** 240 on form pages, 280 (`--wide`) on passkey pages (richie.css:322, 340). Review C5.
8. **`Signin-send-failed:31` puts the error in `role="status"`** although focus stays on the button. The C2 cooldown path is not drawn.
9. **Two polite live regions on every Verify board:** `#code-status` and `.rc-sr` (e.g. `Verify:37`).
10. **Dark:** only 390 dark wrappers of the 5 bases; no dark error board; no dark `--success`.
11. **Document titles** mix app titles with state names; `Signin-error:6` collides with Verify's h1.
12. **Copy drift** listed in §3: "sign-in code", "Code from your email", "cancelled", "you@example.com", 0:59, noun-label headlines.
13. **No `is_interactive`** on any board, although boards link to each other; hand-drawn icons (key, circle-!, badge glyphs) instead of Phosphor.
14. **Enumeration recorded twice, differently.** `Signin-unknown` says "No Richie account uses this email" (product decision: accepted, rate-limit the lookup), while the review's engineering notes still say "Nothing anywhere says account not found". Follow the decision.

### Settings canvas items that reach identity (`1793877977-4d5d`)

1. **Solo pages** (`Data-deleted`, `Deletion-pending`, `Export-*`, `Signin-deleted`) are start-aligned, with a 1× PNG mascot and no "Richie" in the footer (account.css:2480-2550), although spec §8 row 28 says they reuse identity's frame. Redraw them in the identity frame when they move.
2. **`Signin-deleted` is an identity handoff** (spec §4.8) that exists only on the settings canvas.
3. **Labels against the one-name rules:** `Export-start:24` "Email address"; `Export-start:27` and `Export-request:27` "Email me a code"; `Deletion-pending:27` "Keep my account".
4. **`Stepup-cant-get-code:27`** (and the ghost copies in `Stepup`, `Stepup-code`, `-error`, `-locked`) reads "Use a passkey instead, if you’ve set one up." Spec §4.4 says "Use a passkey instead."; the clause is redundant because the item only shows when a passkey exists.
5. **`launch` is still `canvas`**, although spec §6.2 asks for a focused or pages entry at `Main`.
6. **Signed-out recovery:** evaluation R2.5 item 4. A signed-out person with no passkey and no mailbox can't reach step-up screen C.

### Still open from the identity review

- **Not designed:** edge pages (sign-in request expired, error, offline, too many requests, signed out); recovery when someone loses their email; where SSO sits. Passkey and email settings now exist on the settings canvas.
- **Production head:** self-host Inter; add `color-scheme` and `theme-color` metas; follow the OS before first paint.
- **Visual:** swap the key glyph for the FIDO passkey icon; centre the mascot on whole pixels; Create account with field errors scrolls a few pixels in phone landscape.
- **Belongs to the Richie app:** workspace invites, which workspace you're signing in to, and where people land after sign-in.
- **Product must confirm** (evaluation R2.5): support's verification procedure for a lost mailbox, how support replies, the expected response time on screen C, and the legal check on keeping data for 60 days (spec §9 Q3).

---

## 7. Rules for the next area (edge pages, recovery, SSO entry points)

1. **Write a spec first** in `docs/design/identity-edges/`, shaped like the settings spec: plan, departures, page specs with copy, a state matrix (§4), the board list, the CSS plan and the handoff. Put the rationale for deliberate choices in it.
2. **Walk every journey for dead ends before drawing.** Every error has a next step, every page has a way out, and every recovery path reaches support from a signed-out page.
3. **Use the identity frame:** a centred 460 column; a centred mascot, h1 and lede (lede at most 2 lines at 390); start-aligned forms and body; a footer with the "Richie" name and separate Help, Privacy and Terms links (no separators).
4. **Use the settings tokens** (copy `account.css` §0, not `richie.css`): dark muted `#161617`, link `#0066cc`/`#2997ff`, fill `#0071e3` in both themes, success `#16a34a`/`#4ade80` for icons only.
5. **Use the fixed type scale:** title 30/36/600; body and labels 16/24; hints, errors, status and button labels 14/20; footer and legal 12/16. Nothing depends on viewport height.
6. **Use settings fields:** 44 px, filled, with the 1 px `--input` edge and radius 10. Labels go above; hints and errors go below, linked with `aria-describedby`.
7. **Use settings buttons:** `lg` 44, block, full column width, radius 10. One filled primary per page; outline for secondary; plain for low stakes. Never show disabled with opacity.
8. **Draw the mascot from identity's SVG** at 62×78, `alt=""`, pixelated, snapped to whole pixels. Use 31×39 at ≤540 px height and on deletion-lifecycle pages (DL-5). Badges mark outcomes only, and the dark ok badge uses an Ink glyph.
9. **Inline Phosphor icons** from the snippets and run `check-icons.py`. Decide the passkey glyph once, for both apps.
10. **Use the one-name table and the US rules** (§3). Failure lines use the three settings patterns; countdowns start at 1:00.
11. **Code entry is settings' code entry** with spec §4.4 behaviour: 4 + 4 slots; when blocked, the countdown sits in the button; a locked input is `readonly` + `aria-disabled`, and focus moves to the error.
12. **Status goes in one reserved slot** (`role=status` + `role=alert`). No state change moves the page; check every string with `measure-status.mjs`.
13. **Focus:** an outcome page focuses its h1 (`tabindex="-1"`, no ring); a form page focuses its field on a fine pointer; after a failed submit, focus the first invalid field. One blue ring everywhere.
14. **Edge pages keep the person's context** (email, return URL, the sign-in request) and say what happened and the next step, with real numbers. Never a generic error without a way back.
15. **Write down what each page reveals and its rate limit.** Sign-in already reveals unknown emails; recovery and SSO pages state their own rule and show limits with the countdown button.
16. **SSO and provider buttons never add a second filled primary.** Draw them as outline buttons under the "or" divider until the product decisions (WP3.0) say otherwise.
17. **Dark:** draw `.dark` wrappers at 390 and 1440, and draw dark for every error state. Tokens live on `:root`/`.dark`; production identity follows the OS before first paint.
18. **Boards:** one base per screen; full copies only for content changes, re-copied after base edits; overlays use `inert` plus a layer; static markup only.
19. **Canvas:** `Main.dc.html` is the entry; files are `<Area>-<screen>[-<state>][-mobile|-dark]`; rows are screens with desktop, mobile, dark and states in columns, 80 px apart; title notes sit 260 above; linked boards are `is_interactive`; a board's `<title>` is its board title.
20. **Test with the rig before handoff:** light, the forced-dark build, forced colours and the 11 viewports. Record layout shift, discard only the listed false positives, and check the mascot by eye.
21. **One source of truth per concern:** the stylesheet for values, the spec for behaviour, copy and states, `canvas.json` for positions. Review IDs go in a changelog. After each fix pass, re-check changed boards against the spec.
22. **Grade independently** with the settings rubric and ship bar. The evaluator doesn't read QA notes; publish the verdict as an artifact and link it from the spec.
23. **Redraw handoff items in the identity frame:** `Signin-deleted`, `Deletion-pending`, the export pages and signed-out recovery. Don't copy them from the settings canvas.
